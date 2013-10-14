using Benchy.Framework;

namespace Sample.Benchy.Tests
{
    [BenchmarkFixture(Category = "Superheros")]
    public class CaptainPlanetBenchmarkTest
    {
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