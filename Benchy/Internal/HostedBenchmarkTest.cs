using System;
using System.Diagnostics;

namespace Benchy.Framework
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
        public Type ExceptionType { get; private set; }
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

        public bool CollectGarbage {
            get { return _hostedTest.CollectGarbage; }
        }
        public bool RunInParallel
        {
            get { return _hostedTest.RunInParallel; }
        }


        public void PerPassSetup()
        {
            try
            {
                if (!_hostedTest.HasSetup(ExecutionScope.OncePerPass)) return;
                _logger.WriteEntry("PER PASS SETUP START", LogLevel.Setup);
                _hostedTest.PerPassSetup();
                _logger.WriteEntry("PER PASS SETUP COMPLETE", LogLevel.Setup);
            }
            catch (Exception e)
            {
                throw new SetupException(e);
            }
        }

        public void PerPassTeardown()
        {
            try
            {
                if (!_hostedTest.HasTeardown(ExecutionScope.OncePerPass)) return;
                _logger.WriteEntry("PER PASS TEARDOWN START", LogLevel.Teardown);
                _hostedTest.PerPassTeardown();
                _logger.WriteEntry("PER PASS TEARDOWN COMPLETE", LogLevel.Teardown);
            }
            catch (Exception e)
            {
                throw new TeardownException(e);
            }
        }

        public void Setup()
        {
            try
            {
                if (!_hostedTest.HasSetup(ExecutionScope.OncePerMethod)) return;
                _logger.WriteEntry("PER METHOD SETUP START", LogLevel.Setup);
                _hostedTest.Setup();
                _logger.WriteEntry("PER METHOD SETUP COMPLETE", LogLevel.Setup);
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
                if (!_hostedTest.HasTeardown(ExecutionScope.OncePerMethod)) return;
                _logger.WriteEntry("PER METHOD TEARDOWN START", LogLevel.Teardown);
                _hostedTest.Teardown();
                _logger.WriteEntry("PER METHOD TEARDOWN COMPLETE", LogLevel.Teardown);
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
                ExceptionType = e.GetType();
                _logger.WriteEntry("EXECUTION EXCEPTION", LogLevel.Execution | LogLevel.Exception);
                _logger.WriteEntry(e.ToString(), LogLevel.Execution | LogLevel.Exception);
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