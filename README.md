Benchy
======

A simple performance testing framework to enable unit tests to be written for .NET code.


Preface
=======

I love testing. I'm a TDD fanatic. I've been using Unit-testing tools for years now, and I honestly believe I'm a better coder for it. I use Unit testing frameworks to help me design my code, to help me execute my code, and for a while, I've been haphazardly using them to help me gauge the performance numbers of my code.

The problem with using unit testing frameworks for benchmarking is that the benchmark stuff is homegrown, and inconsistent, so I've decided to start the Benchy project.

The Benchy project is meant to be a performance and code benchmark testing tool, so that users familiar with the unit-test structures can easily add performance tests to their applications.


Getting started
---------------

Using Benchy for performance testing should be easy for anyone who's done even the most basic unit testing.

1) Create a test assembly.
2) Create a new class in that assembly, and decorate it with the [BenchFixture] attribute.
3) Add a method (cannot take parameters right yet) to that class, and decorate that method with the [Benchmark] attribute.
     This method will be your first 'benchmark' method, so start with something easy (count to 10 million or something).
4) Build your test assembly.

Execute the Benchy.Runner.exe file, with the path of your test assembly as it's only parameter.

To be continued.
