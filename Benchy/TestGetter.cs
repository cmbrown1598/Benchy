using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Benchy
{
    public class TestGetter
    {
        public IEnumerable<IBenchmarkTest> GetTests(params string[] filePaths)
        {
            var builder = new TestBuilder();
            var tests = new List<IBenchmarkTest>();
            foreach (var file in filePaths)
            {
                var assembly = Assembly.LoadFrom(file);
                var iBenchmarkTestType = typeof (IBenchmarkTest);
                
                tests.AddRange(from type in assembly.GetTypes() where iBenchmarkTestType.IsAssignableFrom(type) select Activator.CreateInstance(type) as IBenchmarkTest);
                tests.AddRange(builder.BuildTests(assembly));
            }
            return tests;
        }

    }
}
