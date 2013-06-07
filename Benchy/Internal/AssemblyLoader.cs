using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Benchy.Internal
{
    static class AssemblyLoader
    {
        /// <summary>
        /// Given file paths to assemblies, it returns a set of BenchmarkTests.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="filePaths">The files to attempt to load.</param>
        /// <returns></returns>
        public static IEnumerable<IBenchmarkTest> LoadTests(ILogger logger, params string[] filePaths)
        {
            var builder = new TestBuilder();
            var tests = new List<IBenchmarkTest>();
            foreach (var assembly in filePaths.Select(Assembly.LoadFrom))
            {
                tests.AddRange(builder.BuildTests(assembly, logger));
            }
            return tests;
        }

    }
}
