using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Benchy.Framework;

namespace Benchy.Internal
{
    sealed class TestRunner : ITestRunner
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
        /// <param name="testFixtures">Fixtures to execute.</param>
        /// <returns>The results of those tests.</returns>
        public IExecutionResults[] ExecuteTests(IEnumerable<IFixture> testFixtures)
        {
            if (testFixtures == null) throw new ArgumentNullException("testFixtures");

            _logger.WriteEntry(String.Format("Test runner starting. {0}", DateTime.Now), LogLevel.Setup);

            var list = new List<IExecutionResults>();
            foreach (var fixture in testFixtures)
            {
               
                fixture.Setup();
               

                foreach (var performanceTestPass in PerformTests(fixture.BenchmarkTests))
                {
                    _logger.WriteEntry(_formatter.FormatResult(performanceTestPass), LogLevel.Results);
                    list.Add(performanceTestPass);
                }

               
                fixture.Teardown();
            }
            _logger.WriteEntry(String.Format("Test runner complete. {0}", DateTime.Now), LogLevel.Setup);

            
            return list.ToArray();
        }


        private IEnumerable<IExecutionResults> PerformTests(IEnumerable<IBenchmarkTest> tests)
        {
            foreach (var test in tests.Select(t => new HostedBenchmarkTest(t, _logger)))
            {
                var localTest = test;
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


                    var executionCount = test.ExecutionCount + 1;
                    var localResultStatus = resultStatus;
                    var parallelOptions = new ParallelOptions();
                    
                    if (!test.RunInParallel)
                    {
                        //ignore parallelism.
                        parallelOptions.MaxDegreeOfParallelism = 1;
                    }

                    Parallel.For(0, executionCount, parallelOptions, indexer =>
                        {
                            var testPass = ExecuteTest(indexer, localTest);
                            if (!localTest.ThrewException && indexer <= 0) return;

                            localResultStatus = (ResultStatus)
                                           Math.Max(
                                               (int)localTest.GetResult(),
                                               (int)localResultStatus
                                               );

                            result.AddTestPass(testPass);
                        });

                    resultStatus = localResultStatus;

                    _logger.WriteEntry(string.Format("{0}: Teardown", test.Name),
                        LogLevel.Teardown);

                    test.Teardown();
                }
                catch (SetupException setupException)
                {
                    _logger.WriteEntry(string.Format("{0}: Setup Exception\r\n{1}", test.Name, setupException),
                        LogLevel.Exception | LogLevel.Setup);

                    result.SetupException = setupException;

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

        private TestPass ExecuteTest(long indexer, HostedBenchmarkTest test)
        {
            if (test.CollectGarbage)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            } 
            
            var testPassName = indexer == 0
                                   ? string.Format("{0} Initialization", test.Name)
                                   : string.Format("{1} Execution, Pass #{0}", indexer, test.Name);

            _logger.WriteEntry(testPassName + " Start", LogLevel.Execution);

            test.PerPassSetup();

            test.Execute();

            test.PerPassTeardown();

            var testPass = new TestPass
                {
                    TestName = testPassName,
                    ExceptionOccurred = test.ThrewException,
                    ExceptionTypeName = test.ExceptionName,
                    ExecutionTime = test.ExecutionTime,
                    Status = test.GetResult()
                };

            _logger.WriteEntry(testPass.TestName + " " + testPass.Status, LogLevel.Execution);
            
            return testPass;
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
                        ResultStatus.Failed,
                        result.HasExceptions ? "FAILED: Threw exceptions during execution." : string.Format("FAILED: Maximum execution time was: {0}, past the failure time {1}", result.LongestTime, failTime)
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
