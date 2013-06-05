using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Benchy
{
    class TestBuilder
    {
        public IEnumerable<ExternalBenchmarkTest> BuildTests(Assembly assembly)
        {
            var types = assembly.GetTypes().Where(type => type.GetCustomAttributes(typeof(BenchmarkFixtureAttribute), true).Length > 0).Where(m => !typeof(IBenchmarkTest).IsAssignableFrom(m));

            var list = new List<ExternalBenchmarkTest>();
            foreach (Type type in types)
            {
                var benchmarkMethods = type.GetMethods().Where(m => m.GetCustomAttributes<BenchmarkAttribute>().Any()).Where(m => !m.GetParameters().Any());
                var setupMethods = type.GetMethods().Where(m => m.GetCustomAttributes<SetupAttribute>().Any()).Where(m => !m.GetParameters().Any()).ToArray();
                var teardownMethods = type.GetMethods().Where(m => m.GetCustomAttributes<TeardownAttribute>().Any()).Where(m => !m.GetParameters().Any()).ToArray();

                foreach (var benchmarkMethod in benchmarkMethods)
                {
                    var obj = Activator.CreateInstance(type);
                    var attr = benchmarkMethod.GetCustomAttribute<BenchmarkAttribute>();

                    var setupMethod = CreateAction(setupMethods, obj);
                    var teardownMethod = CreateAction(teardownMethods, obj);
                    var executeMethod = CreateAction(new[] {benchmarkMethod}, obj);

                    list.Add(new ExternalBenchmarkTest(setupMethod, executeMethod, teardownMethod)
                        {
                            ExecutionCount = attr.ExecutionCount, 
                            Name = obj.GetType().Name,
                            FailBy = GetTimespan(attr.FailureTimeInTicks, attr.FailureTimeInMilliseconds,
                                                  attr.FailureTimeInSeconds),
                            WarnBy = GetTimespan(attr.WarningTimeInTicks, attr.WarningTimeInMilliseconds,
                                                  attr.WarningTimeInSeconds)                            
                        });
                }
            }
            return list;
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
            if(seconds > 0)
                return TimeSpan.FromSeconds(seconds);

            if (milliseconds > 0)
                return TimeSpan.FromMilliseconds(milliseconds);

            if (ticks > 0)
                return TimeSpan.FromTicks(ticks);
            
            return null;
        }

    }
}
