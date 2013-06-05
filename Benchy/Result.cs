using System;
using System.Collections.Generic;
using System.Linq;

namespace Benchy
{
    public class Result
    {
        private readonly List<TimeSpan> _completionTimes = new List<TimeSpan>();
        private readonly Dictionary<string, int> _exceptionCounts = new Dictionary<string, int>();

        public ResultStatus ResultStatus { get; internal set; }

        public bool ThrewExceptionOnSetup
        {
            get { return SetupException != null; }
        }

        public SetupException SetupException
        {
            get; internal set;
        }

        public bool ThrewExceptionOnTeardown
        {
            get { return TeardownException != null; }
        }

        public TeardownException TeardownException
        {
            get; internal set;
        }

        public string Name
        {
            get; set;
        }

        public bool ThrewExecutionExceptions
        {
            get { return _exceptionCounts.Keys.Any(); }
        }

        public int IterationCount
        {
            get { return _completionTimes.Count; }
        }
        
        internal void AddExecutionTime(TimeSpan executionTime)
        {
            _completionTimes.Add(executionTime);
        }


        public TimeSpan LongestTime {
            get { return IterationCount > 0 ? _completionTimes.Max() : TimeSpan.MinValue; }
        }

        public TimeSpan ShortestTime {
            get { return IterationCount > 0 ? _completionTimes.Min() : TimeSpan.MinValue; }
        }

        public double MeanTime {
            get { return IterationCount > 0 ? _completionTimes.Average(m => m.Ticks) : 0; }
        }

        public TimeSpan[] Data
        {
            get { return _completionTimes.ToArray(); }
        }

        public DataBreakout[] GetBreakout(int countOfBreakoutItems = 5)
        {
            return DataBreakout.GetBreakouts(_completionTimes.Select(m => (double) m.Ticks).ToArray(), countOfBreakoutItems);
        }

        public double StdDev {
            get
            {
                var ret = 0d;
                if (_completionTimes.Count > 1)
                {
                    var avg = MeanTime;
                    var count = IterationCount;
                    var sum = _completionTimes.Sum(d => Math.Pow(d.Ticks - avg, 2));   
                    ret = Math.Sqrt((sum) / (count - 1));
                }
                return ret;
            }
        }

        public bool ThrewExceptions
        {
            get { return (ThrewExceptionOnSetup || ThrewExceptionOnTeardown) || ThrewExecutionExceptions; }
        }

        public IEnumerable<ExecutionExceptionInformation> GetExecutionExceptions()
        {
            return _exceptionCounts.Select(item => new ExecutionExceptionInformation {Occurances = item.Value, ExceptionTypeName = item.Key });
        }

        internal void AddExecutionException(string exceptionName)
        {
            if (_exceptionCounts.ContainsKey(exceptionName))
            {
                _exceptionCounts[exceptionName]++;
            }
            else
            {
                _exceptionCounts[exceptionName] = 1;
            }
        }
    }
}
