using Benchy.Framework;

namespace Sample.Benchy.Tests
{
    [BenchmarkFixture(Category = "Superheros", Ignore = true)]
    public class CaptainPlanetBenchmarkTest
    {
        [Benchmark(ExecutionCount = 100, WarningTimeInMilliseconds = 10, FailureTimeInMilliseconds = 20)]
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