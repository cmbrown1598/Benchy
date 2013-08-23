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
            IEnumerable<IExecutionResults> results = null;
            if (options != null)
            {
                using (var engine = new Engine(options))
                {
                    results = engine.Execute();
                }
            }

            if (results != null)
            {
                retValue = results.Aggregate(retValue, (current, result) => Math.Max(current, (int)result.ResultStatus));
                Console.WriteLine("Benchmark Runner Test status: {0}", (ResultStatus)retValue);
            }
            else
            {
                Console.WriteLine("No benchmark tests run.");
            }
            Console.WriteLine("Press any key to continue.");
            Console.ReadLine();
            return retValue;
        }
    }
}
