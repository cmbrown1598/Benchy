using System;
using System.Collections.Generic;

namespace Benchy
{
    public interface IExecutionResults
    {
        ResultStatus ResultStatus { get; }
        bool ThrewExceptionOnSetup { get; }
        bool ThrewExceptionOnTeardown { get; }
        SetupException SetupException { get; }
        TeardownException TeardownException { get; }
        bool ThrewExecutionExceptions { get; }
        bool HasExceptions { get; }
        int TestPassesCount { get; }
        TimeSpan StdDev { get; }
        TimeSpan LongestTime { get; }
        TimeSpan ShortestTime { get; }
        TimeSpan MeanTime { get; }
        ITestPass[] Data { get; }
        string Name { get; set; }
        string ResultText { get; }
        IDataBreakout[] GetBreakout();
        IEnumerable<IExecutionExceptionInformation> GetExecutionExceptions();
    }
}