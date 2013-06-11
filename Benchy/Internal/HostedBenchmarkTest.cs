using System;
using System.Diagnostics;

namespace Benchy.Internal
{
    internal class HostedBenchmarkTest
    {
        private readonly IBenchmarkTest _hostedTest;
        private readonly ILogger _logger;

        public HostedBenchmarkTest(IBenchmarkTest hostedTest, ILogger logger)
        {
            _hostedTest = hostedTest;
            _logger = logger;
        }

        public TimeSpan ExecutionTime { get; private set; }
        public string ExceptionName { get; private set; }
        public bool ThrewException { get; private set; }
        public string TypeName
        {
            get { return _hostedTest.TypeName; }
        }
        public string Category { get { return _hostedTest.Category; } }

        public TimeSpan Fail
        {
            get { return _hostedTest.FailTime ?? TimeSpan.MaxValue; }
        }

        public TimeSpan Warn
        {
            get { return _hostedTest.WarnTime ?? TimeSpan.MaxValue; }
        }

        public ResultStatus GetResult()
        {
            // if fail is null, and warning is null, return Success, if there wasn't an exception.
            if (ThrewException || ExecutionTime.Ticks > Fail.Ticks)
                return ResultStatus.Failed;

            return ExecutionTime.Ticks > Warn.Ticks ? ResultStatus.Warning : ResultStatus.Success;
        }

        public uint ExecutionCount
        {
            get { return _hostedTest.ExecutionCount; }
        }

        public string Name
        {
            get { return _hostedTest.Name; }
        }

        public void Setup()
        {
            try
            {
                _logger.WriteEntry("SETUP START", LogLevel.Setup);
                _hostedTest.Setup();
                _logger.WriteEntry("SETUP COMPLETE", LogLevel.Setup);
            }
            catch (Exception e)
            {
                throw new SetupException(e);
            }
        }

        public void Teardown()
        {
            try
            {
                _logger.WriteEntry("TEARDOWN START", LogLevel.Teardown);
                _hostedTest.Teardown();
                _logger.WriteEntry("TEARDOWN COMPLETE", LogLevel.Teardown);
            }
            catch (Exception e)
            {
                throw new TeardownException(e);
            }
        }

        public void Execute()
        {
            var watch = new Stopwatch();
            try
            {
                _logger.WriteEntry("EXECUTION START", LogLevel.Execution);
                watch.Start();
                _hostedTest.Execute();
                watch.Stop();
            }
            catch (Exception e)
            {
                ThrewException = true;
                ExceptionName = e.GetType().Name;
                _logger.WriteEntry("EXECUTION EXCEPTION", LogLevel.Execution | LogLevel.Exception);
            }
            finally
            {
                if (watch.IsRunning)
                {
                    watch.Stop();

                }
                ExecutionTime = watch.Elapsed;
                _logger.WriteEntry("EXECUTION COMPLETE", LogLevel.Execution);

            }
        }
    }
}