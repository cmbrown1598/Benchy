using System;

namespace Benchy
{
    [AttributeUsage(AttributeTargets.Method)]
    public class BenchmarkAttribute : Attribute
    {
        public ushort ExecutionCount { get; set; }

    }
}
