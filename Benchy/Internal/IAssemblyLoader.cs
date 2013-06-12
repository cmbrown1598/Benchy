using System.Collections.Generic;

namespace Benchy.Internal
{
    internal interface IAssemblyLoader
    {
        /// <summary>
        /// Given file paths to assemblies, it returns a set of BenchmarkTests.
        /// </summary>
        /// <param name="filePaths">The files to attempt to load.</param>
        /// <returns></returns>
        IEnumerable<IBenchmarkTest> LoadTests(params string[] filePaths);
    }
}