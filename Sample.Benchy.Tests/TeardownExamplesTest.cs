﻿using System;
using System.Threading;
using Benchy.Framework;

namespace Sample.Benchy.Tests
{
    [BenchmarkFixture]
    class TeardownExamplesTest
    {
        [Teardown(ExecutionScope = ExecutionScope.OncePerFixture)]
        public void DoThisMethodOneTimeOnly()
        {
            Console.WriteLine("One time only.");
        }

        [Teardown(ExecutionScope = ExecutionScope.OncePerMethod)]
        public void DoThisOncePerBenchmarkMethod()
        {
            Console.WriteLine("Two times.");
        }

        [Teardown(ExecutionScope = ExecutionScope.OncePerPass)]
        public void DoThisOncePerBenchmarkPass()
        {
            Console.WriteLine("10 times. (5 per method)");
        }

        [Benchmark(ExecutionCount = 5)]
        public void ActualTest()
        {
            Console.WriteLine("Running First");
            Thread.Sleep(100);
        }

        [Benchmark(ExecutionCount = 5)]
        public void SecondActualTest()
        {
            Console.WriteLine("Running second");
            Thread.Sleep(100);
        }
    }
}