﻿using System;

namespace Benchy
{
    /// <summary>
    /// Indicates that the decorated class is a benchmark class
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class BenchmarkFixtureAttribute : Attribute
    {
        /// <summary>
        /// The category for the running test.
        /// </summary>
        public string Category { get; set; }
        
        /// <summary>
        /// Tells the engine not to run the test.
        /// </summary>
        public bool Ignore { get; set; }
    }
}