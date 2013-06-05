using System;

namespace Benchy
{
    [AttributeUsage(AttributeTargets.Method)]
    public class BenchmarkAttribute : Attribute
    {
        public ushort ExecutionCount { get; set; }

        public long WarningTimeInTicks { get; set; }
        public long WarningTimeInSeconds { get; set; }
        public long WarningTimeInMilliseconds { get; set; }

        public long FailureTimeInTicks { get; set; }
        public long FailureTimeInSeconds { get; set; }
        public long FailureTimeInMilliseconds { get; set; }
    }
}
