using System;
using System.Collections.Generic;
using System.Linq;

namespace Benchy.Internal
{
    sealed class TestRunner : IDisposable
    {
        private ILogger _logger;
        private IExecutionResultsFormatter _formatter;

        public TestRunner(ILogger logger, IExecutionResultsFormatter formatter)
        {
            _logger = logger;
            _formatter = formatter;
        }

        /// <summary>
        /// Main method that executes the tests.
        /// </summary>
        /// <param name="tests"></param>
        /// <returns></returns>
        public IExecutionResults[] ExecuteTests(IEnumerable<IBenchmarkTest> tests)
        {
            _logger.WriteEntry(String.Format("Test runner starting. {0}", DateTime.Now), LogLevel.Setup);

            var list = new List<IExecutionResults>();
            foreach (var performanceTestPass in PerformTests(tests))
            {
                _logger.WriteEntry(_formatter.FormatResult(performanceTestPass), LogLevel.Results);
                list.Add(performanceTestPass);
            }
            _logger.WriteEntry(String.Format("Test runner complete. {0}", DateTime.Now), LogLevel.Setup);

            
            return list.ToArray();
        }

        private IEnumerable<IExecutionResults> PerformTests(IEnumerable<IBenchmarkTest> tests)
        {
            foreach (var test in tests.Select(t => new HostedBenchmarkTest(t, _logger)))
            {
                var result = new ExecutionResults { 
                    Name = test.Name, 
                    TypeName = test.TypeName, 
                    Category = test.Category };

                var resultStatus = ResultStatus.Indeterminate;

                try
                {
                    _logger.WriteEntry(string.Format("{0}.{1}: Setup", test.TypeName, test.Name),
                        LogLevel.Setup);
                    
                    test.Setup();

                    for (var i = 0; i <= test.ExecutionCount; i++)
                    {
                        var testPassName = i == 0
                                                ? string.Format("{0} Initialization", test.Name)
                                                : string.Format("{1} Execution, Pass #{0}", i, test.Name);

                        _logger.WriteEntry(testPassName + " Start", LogLevel.Execution);

                        test.Execute();

                        var testPass = new TestPass { 
                            TestName = testPassName, 
                            ExceptionOccurred = test.ThrewException, 
                            ExceptionTypeName = test.ExceptionName, 
                            ExecutionTime = test.ExecutionTime,
                            Status = test.GetResult()
                        };

                        _logger.WriteEntry(testPass.TestName + " " + testPass.Status, LogLevel.Execution);

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

                SetStatus(result, resultStatus, test.Warn, test.Fail);

                
                yield return result;
            }
        }

        private static void SetStatus(ExecutionResults result, ResultStatus resultStatus, TimeSpan warnTime, TimeSpan failTime)
        {
            var status = new Dictionary<ResultStatus, string>
                {
                    {
                        ResultStatus.Indeterminate, 
                        "UNKNOWN: Could not determine results."
                    },                    {
                        ResultStatus.Success, 
                        "SUCCESS: Test succeeded."
                    },
                    {
                        ResultStatus.Warning,
                        string.Format("WARNING: Maximum execution time was: {0}, past the warning time {1}", result.LongestTime, warnTime)
                    },
                    {
                        ResultStatus.Success,
                        string.Format("FAILED: Maximum execution time was: {0}, past the failure time {1}", result.LongestTime, failTime)
                    }
                };


            result.ResultStatus = resultStatus;
            result.ResultText = status[resultStatus];
        }

        public void Dispose()
        {
            _logger = null;
            _formatter = null;
        }

        
    }
}
