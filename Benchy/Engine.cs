﻿using System;
using System.Collections.Generic;
using Benchy.Internal;

namespace Benchy
{
    /// <summary>
    /// The Benchy Engine, which loads the tests and executes them.
    /// </summary>
    public sealed class Engine : IDisposable
    {
        private IExecutionOptions _options;

        /// <summary>
        /// Engine distructor.
        /// </summary>
        /// <param name="options">The options to use.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public Engine(IExecutionOptions options)
        {
            if (options == null) throw new ArgumentNullException("options");

            _options = options;
        }

        /// <summary>
        /// Executes the tests.
        /// </summary>
        /// <returns>An enumerable list <see cref="Benchy.IExecutionResults"/> for each executed test.</returns>
        public IEnumerable<IExecutionResults> Execute()
        {
            var tests = AssemblyLoader.LoadTests(_options.Logger, _options.Files);
            var runner = new TestRunner(_options.Logger, _options.ResultsFormatter);

            return runner.ExecuteTests(tests);
        }

        /// <summary>
        /// Disposes the Engine.
        /// </summary>
        public void Dispose()
        {
            _options = null;
        }
    }
}
