using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

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
            _logger.WriteEntry(String.Format("Test runner starting. {0}", DateTime.Now), LogLevel.Setup);
            var performanceTestPasses = PerformTests(tests);

            foreach (var item in performanceTestPasses)
            {
                returnList.Add(item);
                LogResult(item);
            }
            _logger.WriteEntry(String.Format("Test runner complete. {0}", DateTime.Now), LogLevel.Setup);
            return returnList.ToArray();
        }
        
        void LogResult(IExecutionResults item)
        {
            
            _logger.WriteEntry(string.Format("\r\nCategory: {0}", string.IsNullOrWhiteSpace(item.Category) ? "Benchmark Tests" : item.Category),
                LogLevel.Results);


            _logger.WriteEntry(string.Format("{0}", item.TypeName),
                LogLevel.Results);

            _logger.WriteEntry(string.Format("{0} Status: {1}\r\n", item.Name, item.ResultStatus),
                LogLevel.Results);

            _logger.WriteEntry(string.Format("{0}", item.ResultText),
                LogLevel.Results);

            if (item.ResultStatus >= ResultStatus.Warning)
            {
                var breakdown = new Dictionary<ResultStatus, int>();
                foreach (var execution in item.Data)
                {
                    if (!breakdown.ContainsKey(execution.Status))
                        breakdown[execution.Status] = 0;
                    breakdown[execution.Status]++;
                }
                var sb = new StringBuilder();
                var i = from m in breakdown orderby m.Key select string.Format("\t{0}: {1}", m.Key, m.Value);
                foreach (var j in i)
                {
                    sb.AppendLine(j);
                }
                _logger.WriteEntry(sb.ToString(), LogLevel.Results);
            }

            _logger.WriteEntry(string.Format("Longest time:\r\n\t{0} ({1})", item.LongestTime, TimeSpanText(item.LongestTime)),
                LogLevel.Results);

            _logger.WriteEntry(string.Format("Shortest time:\r\n\t{0} ({1})", item.ShortestTime, TimeSpanText(item.ShortestTime)),
                LogLevel.Results);

            _logger.WriteEntry(string.Format("Average time:\r\n\t{0} ({1})", item.MeanTime, TimeSpanText(item.MeanTime)),
                LogLevel.Results);

            _logger.WriteEntry(string.Format("Standard Deviation:\r\n\t{0} ({1})", item.StdDev, TimeSpanText(item.StdDev)),
                LogLevel.Results);

            _logger.WriteEntry("Execution Time Breakout:",
                LogLevel.Results);

            foreach (var brek in item.GetBreakout())
            {
                _logger.WriteEntry("\t" + brek.GetText(),
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

        private static string TimeSpanText(TimeSpan t)
        {
            if (t.Seconds > 0)
                return t.Seconds + 1 + " seconds";

            if (t.Milliseconds > 0)
                return t.Milliseconds + 1 + " milliseconds";

            if (t.Ticks > 0)
                return t.Ticks + 1 + " ticks";

            return "Infinity";
        }

        private IEnumerable<ExecutionResults> PerformTests(IEnumerable<IBenchmarkTest> tests)
        {
            foreach (var test in tests.Select(t => new HostedBenchmarkTest(t, _logger)))
            {
                var result = new ExecutionResults { Name = test.Name, TypeName = test.TypeName, Category = test.Category };
                var resultStatus = ResultStatus.Indeterminate;

                try
                {
                    _logger.WriteEntry(string.Format("\r\n{0}.{1}: Setup", test.TypeName, test.Name),
                        LogLevel.Setup);
                    
                    test.Setup();

                    for (var i = 0; i <= test.ExecutionCount; i++)
                    {
                        test.Execute();

                        var testPassName = i == 0
                                              ? string.Format("{0} Initialization", test.Name)
                                              : string.Format("{1} Execution, Pass #{0} : {2}", i, test.Name, test.GetResult());

                        _logger.WriteEntry(testPassName,
                            LogLevel.Execution);

                        var testPass = new TestPass { 
                            TestName = testPassName, 
                            ExceptionOccurred = test.ThrewException, 
                            ExceptionTypeName = test.ExceptionName, 
                            ExecutionTime = test.ExecutionTime,
                            Status = test.GetResult()
                        };

                        if (!test.ThrewException && i <= 0) continue;
                        
                        resultStatus = (ResultStatus)
                                       Math.Max(
                                           (int)test.GetResult(),
                                           (int)resultStatus
                                           );
                        result.AddTestPass(testPass);
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
                    result.ResultText = "Test threw an exception during Setup.";
                    resultStatus = ResultStatus.Failed;
                }
                catch (TeardownException teardownException)
                {
                    _logger.WriteEntry(string.Format("{0}: Teardown Exception\r\n{1}", test.Name, teardownException),
                        LogLevel.Exception | LogLevel.Teardown);

                    result.TeardownException = teardownException;
                    result.ResultText = "Test threw an exception during Teardown.";
                    resultStatus = ResultStatus.Failed;
                }

                // set the status
                result.ResultStatus = resultStatus;
                if (result.ResultStatus == ResultStatus.Success)
                {
                    result.ResultText = "SUCCESS: Test succeeded.";
                }
                if (result.ResultStatus == ResultStatus.Warning)
                {
                    result.ResultText = string.Format("WARNING: Maximum execution time was: {0}, past the warning time {1}", result.LongestTime, test.Warn);
                }
                
                if (result.ResultStatus == ResultStatus.Failed)
                {
                    result.ResultText = string.Format("FAILED: Maximum execution time was: {0}, past the failure time {1}", result.LongestTime, test.Fail);
                }
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
            private readonly ILogger _logger;

            public HostedBenchmarkTest(IBenchmarkTest hostedTest, ILogger logger)
            {
                _hostedTest = hostedTest;
                _logger = logger;
            }

            public TimeSpan ExecutionTime { get; private set; }
            public string ExceptionName { get; private set; }
            public bool ThrewException { get; private set; }
            public string TypeName {
                get { return _hostedTest.TypeName; }
            }
            public string Category { get { return _hostedTest.Category; } }

            public TimeSpan Fail {
                get { return _hostedTest.FailTime ?? TimeSpan.MaxValue; }
            }

            public TimeSpan Warn
            {
                get { return  _hostedTest.WarnTime ?? TimeSpan.MaxValue; }
            }

            public ResultStatus GetResult()
            {
                // if fail is null, and warning is null, return Success, if there wasn't an exception.
                if(ThrewException || ExecutionTime.Ticks > Fail.Ticks)
                    return ResultStatus.Failed;
                
                return ExecutionTime.Ticks > Warn.Ticks ? ResultStatus.Warning : ResultStatus.Success;
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
}
