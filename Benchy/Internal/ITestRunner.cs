using System;
using System.Collections.Generic;

namespace Benchy.Framework
{
    internal interface ITestRunner : IDisposable
    {
        /// <summary>
        /// Main method that executes the tests.
        /// </summary>
        /// <param name="tests"></param>
        /// <returns></returns>
        IExecutionResults[] ExecuteTests(IEnumerable<IFixture> tests);
    }
}