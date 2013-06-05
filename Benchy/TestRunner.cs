using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Benchy
{
    public class TestRunner : IDisposable
    {
        private ILogger _logger;

        public TestRunner(ILogger logger)
        {
            _logger = logger;
        }

        public Result[] ExecuteTests(IEnumerable<IBenchmarkTest> tests)
        {
            var returnList = new List<Result>();
            var performanceTestPasses = PerformTests(tests);

            foreach (var item in performanceTestPasses)
            {
                LogResult(item);
                returnList.Add(item);
            }

            return returnList.ToArray();
        }
        
        protected virtual void LogResult(Result item)
        {


            _logger.WriteEntry(item.Name + " Statistics",
                LoggingStrategy.Results);

            _logger.WriteEntry(string.Format("Longest time (in ticks): {0}", item.LongestTime),
                LoggingStrategy.Results);

            _logger.WriteEntry(string.Format("Shortest time (in ticks): {0}", item.ShortestTime),
                LoggingStrategy.Results);

            _logger.WriteEntry(string.Format("Mean time (in ticks): {0}", item.MeanTime),
                LoggingStrategy.Results);

            _logger.WriteEntry(string.Format("Std Dev (in ticks): {0}", item.StdDev),
                LoggingStrategy.Results);

            _logger.WriteEntry(item.Name + " Time Breakout",
                LoggingStrategy.Results);

            foreach (var brek in item.GetBreakout())
            {
                _logger.WriteEntry(brek.GetText(),
                    LoggingStrategy.Results);
            }

            if (item.ThrewExceptionOnSetup)
            {
                _logger.WriteEntry(item.Name + " Threw Exceptions on Setup.",
                    LoggingStrategy.Results | LoggingStrategy.Setup | LoggingStrategy.Exception);

                _logger.WriteEntry(item.SetupException.ToString(),
                    LoggingStrategy.Results | LoggingStrategy.Setup | LoggingStrategy.Exception);
            }

            if (item.ThrewExecutionExceptions)
            {
                _logger.WriteEntry(item.Name + " Threw Exceptions during testing.",
                                   LoggingStrategy.Results | LoggingStrategy.Execution | LoggingStrategy.Exception);

                foreach (var exceptionRecord in item.GetExecutionExceptions())
                {
                    _logger.WriteEntry(string.Format("{0} thrown", exceptionRecord.ExceptionTypeName),
                                       LoggingStrategy.Results | LoggingStrategy.Execution | LoggingStrategy.Exception);

                    _logger.WriteEntry(string.Format("\t{0} occurrence(s).", exceptionRecord.Occurances),
                                       LoggingStrategy.Results | LoggingStrategy.Execution | LoggingStrategy.Exception);
                }
            }
            
            if (!item.ThrewExceptionOnTeardown) return;
            
            _logger.WriteEntry(item.Name + " Threw Exceptions on Teardown.",
                               LoggingStrategy.Results | LoggingStrategy.Teardown | LoggingStrategy.Exception);

            _logger.WriteEntry(item.TeardownException.ToString(),
                               LoggingStrategy.Results | LoggingStrategy.Teardown | LoggingStrategy.Exception);
        }

        private IEnumerable<Result> PerformTests(IEnumerable<IBenchmarkTest> tests)
        {
            foreach (var test in tests.Select(t => new HostedBenchmarkTest(t)))
            {
                var result = new Result { Name = test.Name };

                try
                {
                    _logger.WriteEntry(string.Format("{0}: Setup", test.Name), LoggingStrategy.Setup);
                    
                    test.Setup();

                    for (var i = 0; i <= test.ExecutionCount; i++)
                    {
                        _logger.WriteEntry(i == 0
                            ? "Initializing"
                            : string.Format("{1} Executing, Pass #{0}", i, test.Name), LoggingStrategy.Execution);

                        test.Execute();
                    
                        if (test.ThrewException)
                        {
                            result.AddExecutionException(test.ExceptionName);
                        }

                        result.AddExecutionTime(test.ExecutionTime);
                    }
                    
                    _logger.WriteEntry(string.Format("{0}: Teardown", test.Name), LoggingStrategy.Teardown);

                    test.Teardown();
                }
                catch (SetupException setup)
                {
                    _logger.WriteEntry(string.Format("{0}: Setup Exception\r\n{1}", test.Name, setup), LoggingStrategy.Exception | LoggingStrategy.Setup);

                    result.SetupException = setup;
                }
                catch (TeardownException teardownException)
                {
                    _logger.WriteEntry(string.Format("{0}: Teardown Exception\r\n{1}", test.Name, teardownException), LoggingStrategy.Exception | LoggingStrategy.Teardown);

                    result.TeardownException = teardownException;
                }

                // set the status
                if (result.ThrewExceptions)
                {
                    result.ResultStatus = ResultStatus.Failed;
                }

                


                yield return result;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            _logger = null;
        }

        private class HostedBenchmarkTest : IBenchmarkTest
        {
            private readonly IBenchmarkTest _hostedTest;

            public HostedBenchmarkTest(IBenchmarkTest hostedTest)
            {
                _hostedTest = hostedTest;
            }

            public TimeSpan ExecutionTime { get; private set; }
            public string ExceptionName { get; private set; }
            public bool ThrewException { get; private set; }

            public uint ExecutionCount {
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
                    _hostedTest.Setup();
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
                    _hostedTest.Teardown();
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
                    watch.Start();
                    _hostedTest.Execute();
                    watch.Stop();
                }
                catch (Exception e)
                {
                    ThrewException = true;
                    ExceptionName = e.GetType().Name;
                }
                finally
                {
                    if (watch.IsRunning)
                    {
                        watch.Stop();
                    }
                    ExecutionTime = watch.Elapsed;
                }
            }
        }
    }
}
