using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Benchy.Framework;
using CommandLine;
using CommandLine.Text;

namespace Benchy.Runner
{
    class CommandLineOptions
    {
        public static ExecutionOptions GetCommandLineOptions(string[] args)
        {
            var options = new CommandLineOptions();
            if (Parser.Default.ParseArguments(args, options))
            {
                if(!options.AssemblyFiles.Any())
                {
                    
                    Console.WriteLine(options.GetHelpText());

                    return null;
                }

                var logger = new Logger(options.LogLevel);

                var exOptions = new ExecutionOptions(
                    (from m in options.AssemblyFiles select m).ToArray(),
                    logger);

                return exOptions;
            }


            Console.WriteLine(options.GetHelpText());

            return null;
        }


        [ValueList(typeof(List<string>), MaximumElements = -1)]
        public IList<string> AssemblyFiles { get; set; }

        [Option('l', DefaultValue = LogLevel.Full, HelpText = "Logging message level.")]
        public LogLevel LogLevel { get; set; }

        public string GetHelpText()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
