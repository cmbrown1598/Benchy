using System;
using System.IO;
using System.Reflection;
using Benchy.Internal;
using NUnit.Framework;
using Rhino.Mocks;

namespace Benchy.UnitTests.Internal
{
    [TestFixture]
    public class ExecutionOptionsValidatorTest
    {
        private IExecutionOptions GetOptions(string[] files, bool mockLogger, bool mockFormatter)
        {
            var options = MockRepository.GenerateMock<IExecutionOptions>();
            options.Stub(m => m.Files).Return(files);
            if (mockFormatter)
            {
                options.Stub(m => m.ResultsFormatter).Return(MockRepository.GenerateMock<IExecutionResultsFormatter>());
            }
            if (mockLogger)
            {
                options.Stub(m => m.Logger).Return(MockRepository.GenerateMock<ILogger>());
            }

            return options;
        }

        [Test]
        public void TestEngineFailsOnNullFilePaths()
        {
            var options = GetOptions(null, true, true);
            var validator = new ExecutionOptionsValidator();
            
            Assert.Throws<ApplicationException>(() => validator.Validate(options));
        }
        [Test]
        public void TestEngineFailsWithNoFilePaths()
        {
            var options = GetOptions(new string[0], true, true);
            
            var validator = new ExecutionOptionsValidator();
            Assert.Throws<ApplicationException>(() => validator.Validate(options));
        }

        [Test]
        public void TestEngineFailsWithNullLogger()
        {
            var options = GetOptions(new[] { "A" }, false, true); 
            var validator = new ExecutionOptionsValidator();
            Assert.Throws<ApplicationException>(() => validator.Validate(options));
        }

        [Test]
        public void TestEngineFailsWithNullFormatter()
        {
            var options = GetOptions(new[] { "A" }, true, false); 
            var validator = new ExecutionOptionsValidator();
            Assert.Throws<ApplicationException>(() => validator.Validate(options));
        }


        [Test]
        public void TestEngineFailsWithBsFiles()
        {
            var options = GetOptions(new[] { "A" }, true, true); 
            var validator = new ExecutionOptionsValidator();
            Assert.Throws<FileNotFoundException>(() => validator.Validate(options));
        }


        [Test]
        public void TestSuccess()
        {
            var options = GetOptions(new[] { Assembly.GetExecutingAssembly().Location }, true, true);
            var validator = new ExecutionOptionsValidator();
            Assert.That(validator.Validate(options), Is.True);
        }
         
    }
}