Benchy
======

A simple performance testing framework to be written for .NET code.

Preface
=======

I'm a TDD fanatic. I've been using Unit-testing tools for years now, and I honestly believe I'm a better coder for it. I use Unit testing frameworks to help me design my code, to help me execute my code, and for a while, I've been haphazardly using them to help me gauge the performance numbers of my code.  The problem with using unit testing frameworks for benchmarking is that, at best, it's inconsistent, and unfortunately, rarely integrated into build processes.  The Benchy project is meant to be a performance and code benchmark testing tool, so that users familiar with the unit-test structures can easily add performance tests to their applications.

Getting started
---------------

Using Benchy for performance testing should be easy for anyone who's done even the most basic unit testing.

1. In a test assembly, reference Benchy.
2. Mark a test class with the BenchmarkFixture attribute, then attribute a method you'd like to test with the Benchmark attribute.
3. Execute the Benchy.Runner tool against your assembly (command line Benchy.Runner.exe -files YourAssembly.dll)
4. Congratulations, you've just benchmarked your method!
 
BenchmarkFixture
----------------
When Benchy loads the files you specified, it will iterate through classes you've decorated with the BenchmarkFixture attribute, and look for Benchmarked methods specifically in those classes.  This allows you to put your BenchMark tests in the same assembly as your other unit tests.  The BenchmarkFixture attribute supports _Category_ and _Ignore_ as modifiers.  _Category_ allows you to specify a category for your Benchmark tests. _Ignore_ allows you to tell Benchy not to test the methods in class.

Benchmark
---------
The Benchmark attribute is the central attribute you use to indicate that a method is to be tested.  Benchy will treat every Benchmark-attributed method as an individual test, and gather results on them individually.  Benchmark supports the following modifiers:

* ExecutionCount - The number of times to execute the method in test.  Defaults to 10.
* WarningTime(InTicks),(InMilliseconds),(InSeconds) - The number of (ticks/milliseconds/seconds) which, when passed, will cause the test to be status as a Warning.
* FailureTime(InTicks),(InMilliseconds),(InSeconds) - The number of (ticks/milliseconds/seconds) which, when passed, will cause the test to be status as a Failure.

 
Setup
-----
The setup attribute is a method attribute that you use in a BenchmarkFixture to specify a method that is to run before the Benchmark methods run.  This allows you to perform any setup before the benchmarked method actually executes.

Teardown
--------
With the Setup attribute, must come the teardown attribute, to allow you to clean up any resources you used in your performance test.

Setup and Teardown methods both support a parameter called "ExecutionScope", which flags the method to run either 1) once per fixture, or 2) once per benchmarked method in the fixture.

Notes about Benchy-attributed classes and methods.

1. BenchmarkFixture attributed classes should have a default public constructor.
2. Benchmark, Setup, and Teardown all can take parameters, by simply passing them in to the attribute (see examples below.)  Note: Benchy can't support out or ref parameters.
3. You can place multiple benchmark attributes on a given method, and combined with the above, allow for reuse.

Examples
--------
A simple example of a Benchy Attributed class.

<pre>
using System.Data;
using Benchy;

namespace Sample.Benchy.Tests
{
    [BenchmarkFixture]
    class ExampleBenchmarkTest
    {
        private DataSet _ds;

        [Setup]
        public void Setup()
        {
            _ds = new DataSet("My Dataset");
            _ds.Tables.Add(new DataTable("My Table"));
            _ds.Tables[0].Columns.Add(new DataColumn("Column1"));
        }

        [Teardown]
        public void Teardown()
        {
            _ds = null;
        }

        [Benchmark]
        public void Execute()
        {
            var row = _ds.Tables[0].NewRow();
            row["Column1"] = 43;
            _ds.Tables[0].Rows.Add(row);
            _ds.Tables[0].AcceptChanges();
            _ds.Tables[0].Rows.Clear();
            _ds.Tables[0].AcceptChanges();
        }
    }
}
</pre>

An example of parameterized Setup and Teardown methods, and multiple benchmark test cases.

<pre>
using System;
using Benchy;

namespace Sample.Benchy.Tests
{
    [BenchmarkFixture(Category = "Superheros")]
    public class CaptainPlanetBenchmarkTest
    {
        [Setup(100)]
        public void Setup(int value)
        {
            Console.WriteLine("This sets up nothing." + value);
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

        [Teardown(1000)]
        public void Teardown(int value)
        {
            Console.WriteLine("This tears down nothing." + value);
        }
    }
}
</pre>


