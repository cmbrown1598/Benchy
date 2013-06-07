using System;

namespace Benchy
{
    /// <summary>
    /// Representation of an interation over a benchmark test.
    /// </summary>
    public interface ITestPass
    {
        /// <summary>
        /// The name of the test.
        /// </summary>
        string TestName { get; }

        /// <summary>
        /// Flag indicating whether or not an exception occurred while executing the test.
        /// </summary>
        bool ExceptionOccurred { get; }

        /// <summary>
        /// The type name of the exception that occurred during execution of the test.
        /// </summary>
        string ExceptionTypeName { get; }

        /// <summary>
        /// The time the individual test ran in.
        /// </summary>
        TimeSpan ExecutionTime { get; }
    }
}