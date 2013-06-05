using System;
using Benchy;

namespace Sample.Benchy.Tests
{
    public class CaptainPlanetBenchmarkTest : IBenchmarkTest
    {
        public uint ExecutionCount {
            get { return 5; }
        }

        public string Name {
            get { return "Captain Planet"; }
        }
        public void Setup()
        {
            Console.WriteLine("This sets up nothing.");
        }

        public void Execute()
        {
            var j = 0;
            for (var i = 0; i < 10000000; i++)
            {
                j = j * i;
            }
        }

        public void Teardown()
        {
            Console.WriteLine("This tears down nothing.");
        }
    }
}