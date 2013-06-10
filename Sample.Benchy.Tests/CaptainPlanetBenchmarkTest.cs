using System;
using Benchy;

namespace Sample.Benchy.Tests
{
    [BenchmarkFixture(Category = "Superheros")]
    public class CaptainPlanetBenchmarkTest
    {
        [Setup]
        public void Setup()
        {
            Console.WriteLine("This sets up nothing.");
        }

        [Benchmark(10000, ExecutionCount = 20, WarningTimeInMilliseconds = 20, FailureTimeInMilliseconds = 30)]
        [Benchmark(20000, ExecutionCount = 20, WarningTimeInMilliseconds = 20, FailureTimeInMilliseconds = 30)]
        public void Execute(long maxValue)
        {
            var j = 0;
            for (var i = 0; i < maxValue; i++)
            {
                j = j * i;
            }
        }
        [Teardown]
        public void Teardown()
        {
            Console.WriteLine("This tears down nothing.");
        }
    }
}