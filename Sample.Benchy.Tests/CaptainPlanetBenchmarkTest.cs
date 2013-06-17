using System;
using Benchy;

namespace Sample.Benchy.Tests
{
    [BenchmarkFixture(Category = "Superheros")]
    public class CaptainPlanetBenchmarkTest
    {
        [Setup(ExecutionScope = ExecutionScope.OncePerFixture)]
        public void SetupAllTestRuns()
        {
            Console.WriteLine("This is fixture level.");
        }

        [Setup(ExecutionScope = ExecutionScope.OncePerMethod)]
        public void SetupEachTestRun()
        {
            Console.WriteLine("This executes once per Benchmark.");
        }

        [Setup(ExecutionScope = ExecutionScope.OncePerPass)]
        public void Setup()
        {
            Console.WriteLine("This executes once per test pass.");
        }

        [Benchmark(ExecutionCount = 20, WarningTimeInMilliseconds = 20, FailureTimeInMilliseconds = 30)]
        [Benchmark(ExecutionCount = 20, WarningTimeInMilliseconds = 20, FailureTimeInMilliseconds = 30)]
        public void Execute()
        {
            const int maxValue = 2500000;
            var j = 0;
            for (var i = 0; i < maxValue; i++)
            {
                j = j * i;
            }
        }

    }
}