using System;

namespace Benchy
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BenchmarkFixtureAttribute : Attribute
    {
        public string Category { get; set; }
    }
}