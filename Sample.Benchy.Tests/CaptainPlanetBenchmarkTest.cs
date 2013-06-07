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

        [Benchmark(ExecutionCount = 10)]
        public void Execute()
        {
            var j = 0;
            for (var i = 0; i < 10000000; i++)
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