using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Benchy
{
    public sealed class ExecutionResultsWriter : IExecutionResultsWriter
    {
        private readonly ILogger _logger;

        public ExecutionResultsWriter(ILogger logger)
        {
            _logger = logger;
        }

        public void WriteResult(IExecutionResults item)
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
    }
}