using System;
using System.Collections.Generic;
using System.Linq;
using Benchy.Framework;

namespace Benchy.Runner
{
    class Program
    {
        static int Main(string[] args)
        {
            // Options and filepaths of assemblies to load.
            var retValue = 0;
            var options = CommandLineOptions.GetCommandLineOptions(args);
            if (options != null)
            {
                IEnumerable<IExecutionResults> results;
                using (var engine = new Engine(options))
                {
                    results = engine.Execute();
                }
           
                if (results != null)
                {
                    retValue = results.Aggregate(retValue, (current, result) => Math.Max(current, (int)result.ResultStatus));
                    options.Logger.WriteEntry(string.Format("Benchmark Runner Test status: {0}", (ResultStatus)retValue), LogLevel.Results);
                }
                else
                {
                    options.Logger.WriteEntry("No benchmark tests run.", LogLevel.Results);
                }
            }

            return retValue;
        }
    }
}
