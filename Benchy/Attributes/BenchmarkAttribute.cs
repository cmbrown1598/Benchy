using System;

// ReSharper disable CheckNamespace
namespace Benchy.Framework
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
        private bool _collectGarbage = true;

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
        /// Run the tests in this set in parallel.
        /// </summary>
        public bool Parallelize { get; set; }


        /// <summary>
        /// Number of times the test should execute.
        /// </summary>
        public uint ExecutionCount
        {
            get { return _executionCount; }
            set { _executionCount = value; }
        }

        /// <summary>
        /// Ensures that garbage collection occurs prior to each execution.
        /// </summary>
        public bool CollectGarbage
        {
            get { return _collectGarbage; }
            set { _collectGarbage = value; }
        }

        /// <summary>
        /// Length of time (in ticks) the test takes that flags the result as a warning.
        /// </summary>
        public uint WarningTimeInTicks { get; set; }
        /// <summary>
        /// Length of time (in seconds) the test takes that flags the result as a warning.
        /// </summary>
        public uint WarningTimeInSeconds { get; set; }
        
        /// <summary>
        /// Length of time (in milliseconds) the test takes that flags the result as a warning.
        /// </summary>
        public uint WarningTimeInMilliseconds { get; set; }

        /// <summary>
        /// Length of time (in ticks) the test takes that flags the result as a failure.
        /// </summary>
        public uint FailureTimeInTicks { get; set; }
        
        /// <summary>
        /// Length of time (in seconds) the test takes that flags the result as a failure.
        /// </summary>
        public uint FailureTimeInSeconds { get; set; }
        /// <summary>
        /// Length of time (in milliseconds) the test takes that flags the result as a failure.
        /// </summary>
        public uint FailureTimeInMilliseconds { get; set; }
    }
}
