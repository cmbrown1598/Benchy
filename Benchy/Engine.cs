using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Benchy.Internal;

namespace Benchy
{
    /// <summary>
    /// The Benchy Engine, which loads the tests and executes them.
    /// </summary>
    public sealed class Engine : IDisposable
    {
        private IExecutionOptions _options;
        private IExecutionOptionsValidator _validator = new ExecutionOptionsValidator();
        
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
            _validator.Validate(_options);

            var builder = new TestBuilder(_options.Logger);
            var loader = new AssemblyLoader(builder);
            var runner = new TestRunner(_options.Logger, _options.ResultsFormatter);
           
            var tests = loader.LoadTests(_options.Files);
            var results = runner.ExecuteTests(tests);
           
            return results;
        }

        /// <summary>
        /// Disposes the Engine.
        /// </summary>
        public void Dispose()
        {
            _options = null;
            _validator = null;
        }

    }
}
