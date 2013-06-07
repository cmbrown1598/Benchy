using System;
using System.Collections.Generic;

namespace Benchy.Internal
{
    internal interface ITestRunner : IDisposable
    {
        /// <summary>
        /// Main method that executes the tests.
        /// </summary>
        /// <param name="tests"></param>
        /// <returns></returns>
        ITestResults[] ExecuteTests(IEnumerable<IBenchmarkTest> tests);
    }
}