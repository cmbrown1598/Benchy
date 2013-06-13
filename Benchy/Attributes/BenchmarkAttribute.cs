using System;
using Benchy.Attributes;

// ReSharper disable CheckNamespace
namespace Benchy
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Attribute that indicates a method is a benchmark test method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class BenchmarkAttribute : Attribute, IBenchyAttribute
    {
        /// <summary>
        /// Parameters to pass to the attrubted method.
        /// </summary>
        public object[] Parameters { get; set; }

        private uint _executionCount = 10;

        /// <summary>
        /// Default attribute constructor.
        /// </summary>
        public BenchmarkAttribute()
        {
        }

        /// <summary>
        /// Attribute constructor.the
        /// </summary>
        /// <param name="parameters">Parameters to pass to the attrubted method.</param>
        public BenchmarkAttribute(params object[] parameters)
        {
            Parameters = parameters;
        }

        /// <summary>
        /// Number of times the test should execute.
        /// </summary>
        public uint ExecutionCount
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
