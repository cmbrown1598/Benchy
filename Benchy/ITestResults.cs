using System;
using System.Collections.Generic;

namespace Benchy.Framework
{
    /// <summary>
    /// The results of a benchmark test run.
    /// </summary>
    public interface IExecutionResults
    {
        /// <summary>
        /// The overall status of the run
        /// </summary>
        ResultStatus ResultStatus { get; }
        /// <summary>
        /// Flag indicating whether the test throw an exception during the Setup method.
        /// </summary>
        bool ThrewExceptionOnSetup { get; }
        /// <summary>
        /// Flag indicating whether the test throw an exception during the Teardown method.
        /// </summary>
        bool ThrewExceptionOnTeardown { get; }
        /// <summary>
        /// Exception thrown during the Setup Method.
        /// </summary>
        SetupException SetupException { get; }
        /// <summary>
        /// Exception throw during the Teardown Method.
        /// </summary>
        TeardownException TeardownException { get; }
        /// <summary>
        /// Flag indicating whether the test threw any exceptions during execution of the Benchmarked method.
        /// </summary>
        bool ThrewExecutionExceptions { get; }
        /// <summary>
        /// Flag indicating whether any exceptions occurred at all.
        /// </summary>
        bool HasExceptions { get; }
        /// <summary>
        /// Number of times the test was executed.
        /// </summary>
        int TestPassesCount { get; }
        /// <summary>
        /// The Standard Deviation off all test passes.
        /// </summary>
        TimeSpan StdDev { get; }
        /// <summary>
        /// The Longest Time taken of all test passes.
        /// </summary>
        TimeSpan LongestTime { get; }
        /// <summary>
        /// The Shortest Time taken of all test passes.
        /// </summary>
        TimeSpan ShortestTime { get; }
        
        /// <summary>
        /// The Average Time taken of all test passes.
        /// </summary>
        TimeSpan MeanTime { get; }
        
        /// <summary>
        /// The baseline Test Pass data.
        /// </summary>
        ITestPass[] Data { get; }
        /// <summary>
        /// The name of the test run.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// The typename of the fixture.
        /// </summary>
        string TypeName { get;  }
        /// <summary>
        /// The category of the fixture.
        /// </summary>
        string Category { get; }
        /// <summary>
        /// The result text.
        /// </summary>
        string ResultText { get; }
        /// <summary>
        /// Method which breaks out the timespan data.
        /// </summary>
        /// <returns>An array of <see cref="IDataBreakout"/> instances.</returns>
        IDataBreakout[] GetBreakout();
        /// <summary>
        /// Method which returns Execution information.
        /// </summary>
        /// <returns>An array of <see cref="IExecutionExceptionInformation"/> instances.</returns>
        IEnumerable<IExecutionExceptionInformation> GetExecutionExceptions();
    }
}