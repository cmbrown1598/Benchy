using System;
using System.Collections.Generic;
using Benchy.Internal;

namespace Benchy
{
    public interface ITestResults
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
        IDataBreakout[] GetBreakout(int countOfBreakoutItems = 5);
        IEnumerable<IExecutionExceptionInformation> GetExecutionExceptions();
    }
}