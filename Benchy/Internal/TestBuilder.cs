using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Benchy.Attributes;

namespace Benchy.Internal
{
    class TestBuilder
    {
        public IEnumerable<ExternalBenchmarkTest> BuildTests(Assembly assembly, ILogger logger)
        {
            var list = new List<ExternalBenchmarkTest>();
            try
            {
                var types = assembly.GetTypes().Where(type => type.GetCustomAttributes(typeof(BenchmarkFixtureAttribute), true).Length > 0);

                foreach (var type in types)
                {
                    var benchmarkFixtureAttr = type.GetCustomAttribute<BenchmarkFixtureAttribute>();
                    if (benchmarkFixtureAttr.Ignore)
                        continue;


                    object obj;
                    try
                    {
                        obj = Activator.CreateInstance(type);
                    }
                    catch
                    {
                        logger.WriteEntry(string.Format("Could not create instance of type '{0}'", type.Name)
                            , LogLevel.FixtureSetup);
                        continue;
                    }

                    var setupMethods = GetValidMethods<SetupAttribute>(type, logger, typeof(TeardownAttribute), typeof(BenchmarkAttribute)).ToArray();
                    var teardownMethods = GetValidMethods<TeardownAttribute>(type, logger, typeof(SetupAttribute), typeof(BenchmarkAttribute)).ToArray();
                    var benchmarkMethods = GetValidMethods<BenchmarkAttribute>(type, logger, typeof(SetupAttribute), typeof(TeardownAttribute)).ToArray();

                    for (var index = 0; index < benchmarkMethods.Length; index++)
                    {
                        var benchmarkMethod = benchmarkMethods[index];
                        var benchmarkAttributes = benchmarkMethod.GetCustomAttributes<BenchmarkAttribute>().ToArray();

                        for (var i = 0; i < benchmarkAttributes.Count(); i++)
                        {
                            var att = benchmarkAttributes[i];
                            list.Add(new ExternalBenchmarkTest
                                {
                                    Category = benchmarkFixtureAttr.Category,
                                    SetupAction = CreateAction<SetupAttribute>(setupMethods, obj),
                                    ExecuteAction = () => benchmarkMethod.Invoke(obj, att.Parameters),
                                    TeardownAction = CreateAction<TeardownAttribute>(teardownMethods, obj),
                                    ExecutionCount = att.ExecutionCount,
                                    TypeName = obj.GetType().Name,
                                    Name = benchmarkMethod.Name + " #" + (i + 1),
                                    FailTime =
                                        GetTimespan(att.FailureTimeInTicks, att.FailureTimeInMilliseconds,
                                                    att.FailureTimeInSeconds),
                                    WarnTime =
                                        GetTimespan(att.WarningTimeInTicks, att.WarningTimeInMilliseconds,
                                                    att.WarningTimeInSeconds)
                                });
                        }
                    }
                }
            }
            catch (ReflectionTypeLoadException)
            {
                logger.WriteEntry(string.Format("Could not get types from assembly {0}.", assembly.FullName)
                        , LogLevel.FixtureSetup);
                
            }
            return list;
        }

        private static IEnumerable<MethodInfo> GetValidMethods<T>(Type type, ILogger logger, params Type[] invalidAttributes) where T : Attribute, IBenchyAttribute
        {
            var methods = type.GetMethods().Where(m => m.GetCustomAttributes<T>().Any());

            return (from method in methods 
                    let ignore = method.GetCustomAttributes()
                                            .Aggregate(false, (current, attribute) => current || invalidAttributes.Contains(attribute.GetType()))
                    where !ignore && MethodIsValid<T>(method, logger)
                    select method).ToList();
        }

        private static bool MethodIsValid<T>(MethodBase methodInfo, ILogger logger)
        {
            if (methodInfo.GetParameters().Any(t => t.IsOut))
            {
                logger.WriteEntry(string.Format("{0} is an invalid {1} method. Method cannot have out parameters.", methodInfo.Name, typeof(T).Name)   
                                    , LogLevel.FixtureSetup);
                return false;
            }
            return true;
        }

        private static Action CreateAction<T>(IEnumerable<MethodInfo> methods, object typeInstance) where T : Attribute, IBenchyAttribute 
        {
            return () =>
                {
                    foreach (var method in methods)
                    {
                        var attribute = method.GetCustomAttributes<T>().First();
                        method.Invoke(typeInstance, attribute.Parameters);
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
