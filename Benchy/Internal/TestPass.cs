using System;

namespace Benchy.Framework
{
    sealed class TestPass : ITestPass
    {
        /// <summary>
        /// The name of the test.
        /// </summary>
        public string TestName { get; internal set; }
        /// <summary>
        /// Flag indicating whether or not an exception occurred while executing the test.
        /// </summary>
        public bool ExceptionOccurred { get; internal set; }
        /// <summary>
        /// The type name of the exception that occurred during execution of the test.
        /// </summary>
        public string ExceptionTypeName { get; internal set; }
        /// <summary>
        /// The time the individual test ran in.
        /// </summary>
        public TimeSpan ExecutionTime { get; internal set; }

        public ResultStatus Status { get;
            internal set;
        }
    }
}