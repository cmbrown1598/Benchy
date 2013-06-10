using System;

namespace Benchy
{
    /// <summary>
    /// Attribute that indicates a method is a benchmark test method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class BenchmarkAttribute : Attribute
    {
        public object[] Parameters { get; set; }

        private ushort _executionCount = 1;

        public BenchmarkAttribute()
        {
        }

        public BenchmarkAttribute(params object[] parameters)
        {
            Parameters = parameters;
        }

        /// <summary>
        /// Number of times the test should execute.
        /// </summary>
        public ushort ExecutionCount
        {
            get { return _executionCount; }
            set { _executionCount = value; }
        }

        /// <summary>
        /// Length of time (in ticks) the test takes that flags the result as a warning.
        /// </summary>
        public long WarningTimeInTicks { get; set; }
        /// <summary>
        /// Length of time (in seconds) the test takes that flags the result as a warning.
        /// </summary>
        public long WarningTimeInSeconds { get; set; }
        
        /// <summary>
        /// Length of time (in milliseconds) the test takes that flags the result as a warning.
        /// </summary>
        public long WarningTimeInMilliseconds { get; set; }

        /// <summary>
        /// Length of time (in ticks) the test takes that flags the result as a failure.
        /// </summary>
        public long FailureTimeInTicks { get; set; }
        
        /// <summary>
        /// Length of time (in seconds) the test takes that flags the result as a failure.
        /// </summary>
        public long FailureTimeInSeconds { get; set; }
        /// <summary>
        /// Length of time (in milliseconds) the test takes that flags the result as a failure.
        /// </summary>
        public long FailureTimeInMilliseconds { get; set; }
    }
}
