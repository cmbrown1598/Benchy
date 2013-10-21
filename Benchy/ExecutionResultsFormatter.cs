using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Benchy.Framework
{
    public sealed class ExecutionResultsFormatter : IExecutionResultsFormatter
    {
        public string FormatResult(IExecutionResults item)
        {
            var builder = new StringBuilder();
            builder.AppendFormat("\r\nCategory: {0}",
                                 string.IsNullOrWhiteSpace(item.Category) ? "Benchmark Tests" : item.Category);
            builder.AppendLine();
            
            builder.AppendFormat("{0}", item.TypeName);
            builder.AppendLine();
            
            builder.AppendFormat("{0} Status: {1}\r\n", item.Name, item.ResultStatus);
            builder.AppendLine();
            
            builder.AppendLine(item.ResultText);
            
            

            if (item.ResultStatus >= ResultStatus.Warning)
            {
                var breakdown = new Dictionary<ResultStatus, int>();
                foreach (var execution in item.Data)
                {
                    if (!breakdown.ContainsKey(execution.Status))
                        breakdown[execution.Status] = 0;
                    breakdown[execution.Status]++;
                }

                var i = from m in breakdown orderby m.Key select string.Format("\t{0}: {1}", m.Key, m.Value);
                foreach (var j in i)
                {
                    builder.AppendLine(j);
                }
            }

            builder.AppendFormat("Longest time:\r\n\t{0} ({1})", item.LongestTime, TimeSpanText(item.LongestTime));
            builder.AppendLine();
            

            builder.AppendFormat("Shortest time:\r\n\t{0} ({1})", item.ShortestTime, TimeSpanText(item.ShortestTime));
            builder.AppendLine();
            

            builder.AppendFormat("Average time:\r\n\t{0} ({1})", item.MeanTime, TimeSpanText(item.MeanTime));
            builder.AppendLine();


            builder.AppendFormat("Standard Deviation:\r\n\t{0} ({1})", item.StdDev, TimeSpanText(item.StdDev));
            builder.AppendLine();

            builder.AppendLine("Execution Time Breakout:");

            foreach (var brek in item.GetBreakout())
            {
                builder.AppendLine("\t" + brek.GetText());
            }

            if (item.ThrewExceptionOnSetup)
            {
                builder.AppendLine(item.Name + " Threw Exceptions on Setup.");
                builder.AppendLine(item.SetupException.ToString());
            
            }

            if (item.ThrewExecutionExceptions)
            {
                builder.AppendLine(item.Name + " Threw Exceptions during testing.");
            
                foreach (var exceptionRecord in item.GetExecutionExceptions())
                {
                    builder.AppendFormat("{0} thrown", exceptionRecord.ExceptionTypeName);
                    builder.AppendLine();
            

                    builder.AppendFormat("\t{0} occurrence(s).", exceptionRecord.Occurances);
                    builder.AppendLine();
            
                }
            }
            
            if (!item.ThrewExceptionOnTeardown) return builder.ToString();

            builder.AppendLine(item.Name + " Threw Exceptions on Teardown.");

            builder.AppendLine(item.TeardownException.ToString());
            return builder.ToString();
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