using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Benchy.Internal;

namespace Benchy.Framework
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
        /// <returns>An enumerable list <see cref="IExecutionResults"/> for each executed test.</returns>
        public IEnumerable<IExecutionResults> Execute()
        {
            _validator.Validate(_options);

            var newConsole = new NewConsole(_options.Logger);
            var builder = new TestBuilder(_options.Logger);
            var loader = new AssemblyLoader(builder);
            var runner = new TestRunner(newConsole, _options.ResultsFormatter);

            var currentOutput = Console.Out;

            Console.SetOut(newConsole);
            var tests = loader.LoadTests(_options.Files);
            var results = runner.ExecuteTests(tests);
            Console.SetOut(currentOutput);

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


        private class NewConsole : StringWriter, ILogger
        {
            private readonly ILogger _logger;

            public NewConsole(ILogger logger)
            {
                _logger = logger;
            }

            public void WriteEntry(string text, LogLevel level)
            {
                _logger.WriteEntry(text, level);
            }

            private void Rite(object obj)
            {
                WriteEntry("CONSOLE - " +  obj, LogLevel.Full);
            }

            #region Overrides
            

            public override void Write(bool value)
            {
                Rite(value);
            }
            public override void Write(char value)
            {
                Rite(value);
            }
            public override void Write(char[] buffer)
            {
                Rite(new string(buffer));
            }
            public override void Write(char[] buffer, int index, int count)
            {
                Rite(new string(buffer, index, count));
            }
            public override void Write(decimal value)
            {
                Rite(value);
            }
            public override void Write(double value)
            {
                Rite(value);
            }
            public override void Write(float value)
            {
                Rite(value);
            }
            public override void Write(int value)
            {
                Rite(value);
            }
            public override void Write(long value)
            {
                Rite(value);
            }
            public override void Write(object value)
            {
                Rite(value);
            }
            public override void Write(string format, object arg0)
            {
                Rite(string.Format(format, arg0));
            }
            public override void Write(string format, object arg0, object arg1)
            {
                Rite(string.Format(format, arg0, arg1));
            }
            public override void Write(string format, object arg0, object arg1, object arg2)
            {
                Rite(string.Format(format, arg0, arg1, arg2));
            }
            public override void Write(string format, params object[] arg)
            {
                Rite(string.Format(format, arg));
            }
            public override void Write(string value)
            {
                Rite(value);
            }
            public override void Write(uint value)
            {
                Rite(value);
            }
            public override void Write(ulong value)
            {
                Rite(value);
            }
            public override System.Threading.Tasks.Task WriteAsync(char value)
            {
                return new Task(() => Rite(value));
            }
            public override System.Threading.Tasks.Task WriteAsync(char[] buffer, int index, int count)
            {
                 return new Task(() => Rite(new string(buffer, index, count)));
            }
            public override System.Threading.Tasks.Task WriteAsync(string value)
            {
                 return new Task(() => Rite(value));
            }
            public override void WriteLine(bool value)
            {
                Rite(value);
            }
            public override void WriteLine(char value)
            {
                Rite(value);
            }
            public override void WriteLine(char[] buffer)
            {
                Rite(new string(buffer));
            }
            public override void WriteLine(char[] buffer, int index, int count)
            {
                Rite(new string(buffer, index, count));
            }
            public override void WriteLine(decimal value)
            {
                Rite(value);
            }
            public override void WriteLine(double value)
            {
                Rite(value);
            }
            public override void WriteLine(float value)
            {
                Rite(value);
            }
            public override void WriteLine(int value)
            {
                Rite(value);
            }
            public override void WriteLine(long value)
            {
                Rite(value);
            }
            public override void WriteLine(object value)
            {
                Rite(value);
            }
            public override void WriteLine(string format, object arg0)
            {
                Rite(string.Format(format, arg0));
            }
            public override void WriteLine(string format, object arg0, object arg1)
            {
                Rite(string.Format(format, arg0, arg1));
            }
            public override void WriteLine(string format, object arg0, object arg1, object arg2)
            {
                Rite(string.Format(format, arg0, arg1, arg2));
            }
            public override void WriteLine(string format, params object[] arg)
            {
                Rite(string.Format(format, arg));
            }
            public override void WriteLine(string value)
            {
                Rite(value);
            }
            public override void WriteLine(uint value)
            {
                Rite(value);
            }
            public override void WriteLine(ulong value)
            {
                Rite(value);
            }
            public override System.Threading.Tasks.Task WriteLineAsync(char value)
            {
                return new Task(() => Rite(value));
            }
            public override System.Threading.Tasks.Task WriteLineAsync(char[] buffer, int index, int count)
            {
                return new Task(() => Rite(new string(buffer, index, count)));
            }
            public override System.Threading.Tasks.Task WriteLineAsync(string value)
            {
                return new Task(() => Rite(value));
            }
            #endregion
        }

    }
}
