using System.Collections.Generic;

namespace Benchy.Framework
{
    internal interface IAssemblyLoader
    {
        /// <summary>
        /// Given file paths to assemblies, it returns a set of BenchmarkTests.
        /// </summary>
        /// <param name="filePaths">The files to attempt to load.</param>
        /// <returns></returns>
        IEnumerable<IFixture> LoadTests(params string[] filePaths);
    }
}