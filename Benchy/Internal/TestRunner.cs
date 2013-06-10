using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Benchy.Internal
{
    sealed class TestRunner : ITestRunner
    {
        private ILogger _logger;
        private readonly IExecutionResultsWriter _writer;

        public TestRunner(ILogger logger, IExecutionResultsWriter writer)
        {
            _logger = logger;
            _writer = writer;
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
            }
            _logger.WriteEntry(String.Format("Test runner complete. {0}", DateTime.Now), LogLevel.Setup);
            return returnList.ToArray();
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
                
                _writer.WriteResult(result);
                
                yield return result;
            }
        }

        public void Dispose()
        {
            _logger = null;
        }

        
    }
}
