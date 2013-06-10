using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Benchy.Internal
{
    class TestBuilder
    {
        public IEnumerable<ExternalBenchmarkTest> BuildTests(Assembly assembly, ILogger logger)
        {
            var types = assembly.GetTypes().Where(type => type.GetCustomAttributes(typeof(BenchmarkFixtureAttribute), true).Length > 0);
            var list = new List<ExternalBenchmarkTest>();
            foreach (var type in types)
            {
                object obj;
                try
                {
                    obj = Activator.CreateInstance(type);
                }
                catch
                {
                    logger.WriteEntry(string.Format("Could not create instance of type '{0}', as it has no default constructor.", type.Name)
                        , LogLevel.FixtureSetup);
                    continue;
                }

                var setupMethods = GetAttributedMethods<SetupAttribute>(type);
                var teardownMethods = GetAttributedMethods<TeardownAttribute>(type);
                var benchmarkMethods = GetAttributedMethods<BenchmarkAttribute>(type);

                list.AddRange(from benchmarkMethod in benchmarkMethods.Where(m => IsValidBenchmarkMethod(m, logger))
                              let benchmarkAttr = benchmarkMethod.GetCustomAttribute<BenchmarkAttribute>()
                              let benchmarkFixtureAttr = type.GetCustomAttribute<BenchmarkFixtureAttribute>()
                              where !benchmarkFixtureAttr.Ignore
                              select new ExternalBenchmarkTest
                                  {
                                      Category = benchmarkFixtureAttr.Category,
                                      SetupAction = CreateAction(setupMethods, obj),
                                      ExecuteAction = () => benchmarkMethod.Invoke(obj, null),
                                      TeardownAction = CreateAction(teardownMethods, obj),
                                      ExecutionCount = benchmarkAttr.ExecutionCount, 
                                      Name = obj.GetType().Name, 
                                      FailTime = GetTimespan(benchmarkAttr.FailureTimeInTicks, benchmarkAttr.FailureTimeInMilliseconds, benchmarkAttr.FailureTimeInSeconds), 
                                      WarnTime = GetTimespan(benchmarkAttr.WarningTimeInTicks, benchmarkAttr.WarningTimeInMilliseconds, benchmarkAttr.WarningTimeInSeconds)
                                  });
            }
            return list;
        }



        private static MethodInfo[] GetAttributedMethods<T>(Type type) where T : Attribute
        {
            return type.GetMethods().Where(m => m.GetCustomAttributes<T>().Any()).ToArray();
        }


        private static bool IsValidBenchmarkMethod(MethodInfo benchmarkMethod, ILogger logger)
        {
            if (benchmarkMethod == null) throw new ArgumentNullException("benchmarkMethod");
            if (logger == null) throw new ArgumentNullException("logger");

            if (benchmarkMethod.GetParameters().Any())
            {
                logger.WriteEntry(string.Format("{0} is an invalid Benchmark method. Benchmark method cannot have parameters.", benchmarkMethod.Name)
                    , LogLevel.FixtureSetup);
                return false;
            }
            return true;
        }

        private static Action CreateAction(MethodInfo[] methods, object typeInstance)
        {
            return () =>
                {
                    foreach (var method in methods)
                    {
                        method.Invoke(typeInstance, null);
                    }
                };
        }
        
        public static TimeSpan? GetTimespan(long ticks, long milliseconds, long seconds)
        {
            if (ticks > 0)
                return TimeSpan.FromTicks(ticks);
            if (milliseconds > 0)
                return TimeSpan.FromMilliseconds(milliseconds);
            if(seconds > 0)
                return TimeSpan.FromSeconds(seconds);
            return null;
        }

    }
}
