using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Benchy.Attributes;

namespace Benchy.Internal
{
    class TestBuilder : ITestBuilder
    {
        private readonly ILogger _logger;

        public TestBuilder(ILogger logger)
        {
            _logger = logger;
        }

        public IEnumerable<IFixture> BuildTests(Assembly assembly)
        {
            var list = new List<IFixture>();
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
                        _logger.WriteEntry(string.Format("Could not create instance of type '{0}'", type.Name)
                            , LogLevel.FixtureSetup);
                        continue;
                    }


                    
                    var setupMethods = GetValidMethods<SetupAttribute>(type, typeof(TeardownAttribute), typeof(BenchmarkAttribute)).ToArray();
                    var teardownMethods = GetValidMethods<TeardownAttribute>(type, typeof(SetupAttribute), typeof(BenchmarkAttribute)).ToArray();
                    var benchmarkMethods = GetValidMethods<BenchmarkAttribute>(type, typeof(SetupAttribute), typeof(TeardownAttribute)).ToArray();

                    var fixture = new ExternalBenchmarkFixture(_logger)
                        {
                            SetupAction = CreateAction<SetupAttribute>(setupMethods, obj, ExecutionScope.OncePerFixture),
                            TeardownAction = CreateAction<TeardownAttribute>(teardownMethods, obj, ExecutionScope.OncePerFixture)
                        };

                    for (var index = 0; index < benchmarkMethods.Length; index++)
                    {
                        var benchmarkMethod = benchmarkMethods[index];
                        var benchmarkAttributes = benchmarkMethod.GetCustomAttributes<BenchmarkAttribute>().ToArray();
                        var benchmarkList = new List<IBenchmarkTest>();
                        for (var i = 0; i < benchmarkAttributes.Count(); i++)
                        {
                            var att = benchmarkAttributes[i];

                            benchmarkList.Add(new ExternalBenchmarkTest
                                {
                                    Category = benchmarkFixtureAttr.Category,
                                    CollectGarbage = att.CollectGarbage,
                                    PerPassSetupAction = CreateAction<SetupAttribute>(setupMethods, obj, ExecutionScope.OncePerPass),
                                    SetupAction = CreateAction<SetupAttribute>(setupMethods, obj, ExecutionScope.OncePerMethod),
                                    
                                    ExecuteAction = () => benchmarkMethod.Invoke(obj, att.Parameters),

                                    TeardownAction = CreateAction<TeardownAttribute>(teardownMethods, obj, ExecutionScope.OncePerMethod),
                                    PerPassTeardownAction = CreateAction<TeardownAttribute>(teardownMethods, obj, ExecutionScope.OncePerPass),
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
                        fixture.BenchmarkTests = benchmarkList;
                    }
                    if (fixture.BenchmarkTests.Any())
                    {
                        list.Add(fixture);
                    }
                }
            }
            catch (ReflectionTypeLoadException)
            {
                _logger.WriteEntry(string.Format("Could not get types from assembly {0}.", assembly.FullName)
                        , LogLevel.FixtureSetup);
                
            }
            return list;
        }

        private IEnumerable<MethodInfo> GetValidMethods<T>(Type type, params Type[] invalidAttributes) where T : Attribute, IBenchyAttribute
        {
            var methods = type.GetMethods().Where(m => m.GetCustomAttributes<T>().Any());

            return (from method in methods 
                    let ignore = method.GetCustomAttributes()
                                            .Aggregate(false, (current, attribute) => current || invalidAttributes.Contains(attribute.GetType()))
                    where !ignore && MethodIsValid<T>(method)
                    select method).ToList();
        }

        private bool MethodIsValid<T>(MethodBase methodInfo)
        {
            if (methodInfo.GetParameters().Any(t => t.IsOut))
            {
                _logger.WriteEntry(string.Format("{0} is an invalid {1} method. Method cannot have out parameters.", methodInfo.Name, typeof(T).Name)   
                                    , LogLevel.FixtureSetup);
                return false;
            }
            return true;
        }

        private static Action CreateAction<T>(IEnumerable<MethodInfo> methods, object typeInstance, ExecutionScope scope) where T : Attribute, IBenchyAttribute, IScopedAttribute
        {
            var ms = methods.ToArray();

            if (ms.Any(m => m.GetCustomAttributes<T>().Any(a => a.ExecutionScope == scope)))
            {
                return () =>
                {
                    foreach (var method in ms)
                    {
                        var attribute = method.GetCustomAttributes<T>().FirstOrDefault(m => m.ExecutionScope == scope);
                        if (attribute != null)
                        {
                            method.Invoke(typeInstance, attribute.Parameters);
                        }
                    }
                };
            }
            return null;


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
