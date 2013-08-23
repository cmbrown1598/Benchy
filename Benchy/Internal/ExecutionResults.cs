using System;
using System.Collections.Generic;
using System.Linq;

namespace Benchy.Framework
{
    class ExecutionResults : IExecutionResults
    {
        private readonly List<ITestPass> _testPasses = new List<ITestPass>();
        private ResultStatus _resultStatus = ResultStatus.Indeterminate;

        public ResultStatus ResultStatus
        {
            get { return _resultStatus; }
            internal set { _resultStatus = value; }
        }
        public string ResultText { get; internal set; }

        public bool ThrewExceptionOnSetup
        {
            get { return SetupException != null; }
        }
        public bool ThrewExceptionOnTeardown
        {
            get { return TeardownException != null; }
        }

        public SetupException SetupException
        {
            get;
            internal set;
        }
        public TeardownException TeardownException
        {
            get; internal set;
        }
        public bool ThrewExecutionExceptions
        {
            get { return _testPasses.Any(m => m.ExceptionOccurred); }
        }
        public bool HasExceptions
        {
            get { return (ThrewExceptionOnSetup || ThrewExceptionOnTeardown) || ThrewExecutionExceptions; }
        }

        public int TestPassesCount
        {
            get { return _testPasses.Count; }
        }
        
        internal void AddTestPass(TestPass testPass)
        {
            _testPasses.Add(testPass);
        }

        public TimeSpan StdDev {
            get
            {
                var ret = 0d;
                if (_testPasses.Count > 1)
                {
                    var avg = _testPasses.Average(m => m.ExecutionTime.Ticks);
                    var count = TestPassesCount;
                    var sum = _testPasses.Sum(d => Math.Pow(d.ExecutionTime.Ticks - avg, 2));   
                    ret = Math.Sqrt((sum) / (count - 1));
                }
                return TimeSpan.FromTicks((long)ret);
            }
        }

        public TimeSpan LongestTime {
            get { return TestPassesCount > 0 ? _testPasses.Max(m =>m.ExecutionTime) : TimeSpan.MinValue; }
        }

        public TimeSpan ShortestTime {
            get { return TestPassesCount > 0 ? _testPasses.Min(m => m.ExecutionTime) : TimeSpan.MinValue; }
        }

        public TimeSpan MeanTime {
            get { return TestPassesCount > 0 ? TimeSpan.FromTicks((long)_testPasses.Average(m => m.ExecutionTime.Ticks)) : TimeSpan.MinValue; }
        }

        public ITestPass[] Data
        {
            get { return _testPasses.ToArray(); }
        }

        public string Name { get; set; }

        public string TypeName { get; internal set; }

        public string Category { get; internal set; }


        public IDataBreakout[] GetBreakout()
        {
            return DataBreakout.GetBreakouts(_testPasses.Select(m => m.ExecutionTime).ToArray());
        }

        public IEnumerable<IExecutionExceptionInformation> GetExecutionExceptions()
        {
            return _testPasses.GroupBy(m => m.ExceptionTypeName)
                              .Select( n => new ExecutionExceptionInformation {ExceptionTypeName = n.Key, Occurances = n.Count()});
        }
    }
}
