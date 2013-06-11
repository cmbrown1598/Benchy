using System;
using System.Collections.Generic;
using Benchy.Internal;

namespace Benchy
{
    public sealed class Engine : IDisposable
    {
        private IExecutionOptions _options;

        public Engine(IExecutionOptions options)
        {
            _options = options;
        }

        public IEnumerable<IExecutionResults> Execute()
        {
            var tests = AssemblyLoader.LoadTests(_options.Logger, _options.Files);
            var runner = new TestRunner(_options.Logger, _options.ResultsFormatter);

            return runner.ExecuteTests(tests);
        }

        public void Dispose()
        {
            _options = null;
        }
    }
}
