using System;
using System.IO;
using System.Linq;

namespace Benchy.Internal
{
    class ExecutionOptionsValidator : IExecutionOptionsValidator
    {
        public bool Validate(IExecutionOptions options)
        {
            if (options.Files == null)
                throw new ApplicationException("Files not specified.");
            if (options.Files.Length == 0)
                throw new ApplicationException("Files not specified.");
            if (options.Logger == null)
                throw new ApplicationException("Logger not specified.");
            if (options.ResultsFormatter == null)
                throw new ApplicationException("Results formatter not specified.");

            if(options.Files.Any(m => !File.Exists(m)))
                throw new FileNotFoundException("Assembly file(s) not found.");
            return true;
        }
    }
}
