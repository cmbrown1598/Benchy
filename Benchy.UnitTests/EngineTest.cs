using System;
using System.Reflection;
using NUnit.Framework;
using Rhino.Mocks;

namespace Benchy.UnitTests
{
    [TestFixture]
    class EngineTest
    {
        [Test]
        public void TestEngineThrowsOnNullOptions()
        {
            Assert.Throws<ArgumentNullException>(() => new Engine(null));
        }
        

        [Test]
        public void TestEngineWorksWithOptionsAndAValidFilePath()
        {
            var options = MockRepository.GenerateMock<IExecutionOptions>();
            options.Stub(m => m.Files).Return(new[] { Assembly.GetExecutingAssembly().Location });
            options.Stub(m => m.Logger).Return(MockRepository.GenerateMock<ILogger>());
            options.Stub(m => m.ResultsFormatter).Return(MockRepository.GenerateMock<IExecutionResultsFormatter>());
            var engine = new Engine(options);
            engine.Execute();
            Assert.Pass();
        }
    }
}
