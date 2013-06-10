using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Benchy.Internal
{
    sealed class TestRunner : ITestRunner
    {
        private ILogger _logger;

        public TestRunner(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Main method that executes the tests.
        /// </summary>
        /// <param name="tests"></param>
        /// <returns></returns>
        public IExecutionResults[] ExecuteTests(IEnumerable<IBenchmarkTest> tests)
        {
            var returnList = new List<IExecutionResults>();
            var performanceTestPasses = PerformTests(tests);

            foreach (var item in performanceTestPasses)
            {
                returnList.Add(item);
                LogResult(item);
            }

            return returnList.ToArray();
        }
        
        void LogResult(IExecutionResults item)
        {
            _logger.WriteEntry(string.Format("{0} Status: {1}", item.Name, item.ResultStatus),
                LogLevel.Results);

            _logger.WriteEntry(string.Format("Longest time: {0}", item.LongestTime),
                LogLevel.Results);

            _logger.WriteEntry(string.Format("Shortest time: {0}", item.ShortestTime),
                LogLevel.Results);

            _logger.WriteEntry(string.Format("Average time: {0}", item.MeanTime),
                LogLevel.Results);

            _logger.WriteEntry(string.Format("Standard Deviation: {0}", item.StdDev),
                LogLevel.Results);

            _logger.WriteEntry(item.Name + " Time Breakout",
                LogLevel.Results);

            foreach (var brek in item.GetBreakout())
            {
                _logger.WriteEntry(brek.GetText(),
                    LogLevel.Results);
            }

            if (item.ThrewExceptionOnSetup)
            {
                _logger.WriteEntry(item.Name + " Threw Exceptions on Setup.",
                    LogLevel.Results | LogLevel.Setup | LogLevel.Exception);

                _logger.WriteEntry(item.SetupException.ToString(),
                    LogLevel.Results | LogLevel.Setup | LogLevel.Exception);
            }

            if (item.ThrewExecutionExceptions)
            {
                _logger.WriteEntry(item.Name + " Threw Exceptions during testing.",
                                   LogLevel.Results | LogLevel.Execution | LogLevel.Exception);

                foreach (var exceptionRecord in item.GetExecutionExceptions())
                {
                    _logger.WriteEntry(string.Format("{0} thrown", exceptionRecord.ExceptionTypeName),
                                       LogLevel.Results | LogLevel.Execution | LogLevel.Exception);

                    _logger.WriteEntry(string.Format("\t{0} occurrence(s).", exceptionRecord.Occurances),
                                       LogLevel.Results | LogLevel.Execution | LogLevel.Exception);
                }
            }
            
            if (!item.ThrewExceptionOnTeardown) return;
            
            _logger.WriteEntry(item.Name + " Threw Exceptions on Teardown.",
                               LogLevel.Results | LogLevel.Teardown | LogLevel.Exception);

            _logger.WriteEntry(item.TeardownException.ToString(),
                               LogLevel.Results | LogLevel.Teardown | LogLevel.Exception);
        }

        private IEnumerable<ExecutionResults> PerformTests(IEnumerable<IBenchmarkTest> tests)
        {
            foreach (var test in tests.Select(t => new HostedBenchmarkTest(t)))
            {
                var result = new ExecutionResults { Name = test.Name };
                var resultStatus = ResultStatus.Indeterminate;

                try
                {
                    _logger.WriteEntry(string.Format("{0}: Setup", test.Name),
                        LogLevel.Setup);
                    
                    test.Setup();

                    for (var i = 0; i <= test.ExecutionCount; i++)
                    {
                        var testPassName = i == 0
                                               ? string.Format("{0} Initialization", test.Name)
                                               : string.Format("{1} Execution, Pass #{0}", i, test.Name);

                        _logger.WriteEntry(testPassName,
                            LogLevel.Execution);

                        test.Execute();

                        var testPass = new TestPass { 
                            TestName = testPassName, 
                            ExceptionOccurred = test.ThrewException, 
                            ExceptionTypeName = test.ExceptionName, 
                            ExecutionTime = test.ExecutionTime };
                        
                        if (test.ThrewException || i > 0)
                        {
                            result.AddTestPass(testPass);
                        }

                        // maybe a touch brittle, but works. Any failure = a failure.
                        resultStatus = (ResultStatus)
                            Math.Max(
                                (int)test.GetResult(), 
                                (int)resultStatus
                            );
                    }
                    
                    _logger.WriteEntry(string.Format("{0}: Teardown", test.Name),
                        LogLevel.Teardown);

                    test.Teardown();
                }
                catch (SetupException setup)
                {
                    _logger.WriteEntry(string.Format("{0}: Setup Exception\r\n{1}", test.Name, setup),
                        LogLevel.Exception | LogLevel.Setup);

                    result.SetupException = setup;
                    resultStatus = ResultStatus.Failed;
                }
                catch (TeardownException teardownException)
                {
                    _logger.WriteEntry(string.Format("{0}: Teardown Exception\r\n{1}", test.Name, teardownException),
                        LogLevel.Exception | LogLevel.Teardown);

                    result.TeardownException = teardownException;
                    resultStatus = ResultStatus.Failed;
                }

                // set the status
                result.ResultStatus = resultStatus;
                
                yield return result;
            }
        }

        public void Dispose()
        {
            _logger = null;
        }

        private class HostedBenchmarkTest
        {
            private readonly IBenchmarkTest _hostedTest;

            public HostedBenchmarkTest(IBenchmarkTest hostedTest)
            {
                _hostedTest = hostedTest;
            }

            public TimeSpan ExecutionTime { get; private set; }
            public string ExceptionName { get; private set; }
            public bool ThrewException { get; private set; }

            public ResultStatus GetResult()
            {
                // if fail is null, and warning is null, return Success, if there wasn't an exception.
                var fail = _hostedTest.FailTime ?? TimeSpan.MaxValue;
                var warn = _hostedTest.WarnTime ?? TimeSpan.MaxValue;
                
                if(ThrewException || ExecutionTime > fail)
                    return ResultStatus.Failed;
                
                return ExecutionTime > warn ? ResultStatus.Warning : ResultStatus.Success;
            }

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
