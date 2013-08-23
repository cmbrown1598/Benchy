using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Benchy.Framework
{
    internal class AssemblyLoader : IAssemblyLoader
    {
        private readonly ITestBuilder _testBuilder;

        public AssemblyLoader(ITestBuilder testBuilder)
        {
            _testBuilder = testBuilder;
        }


        /// <summary>
        /// Given file paths to assemblies, it returns a set of BenchmarkTests.
        /// </summary>
        /// <param name="filePaths">The files to attempt to load.</param>
        /// <returns></returns>
        public IEnumerable<IFixture> LoadTests(params string[] filePaths)
        {
            var tests = new List<IFixture>();
            foreach (var assembly in filePaths.Select(Assembly.LoadFrom))
            {
                tests.AddRange(_testBuilder.BuildTests(assembly));
            }
            return tests;
        }

    }
}
