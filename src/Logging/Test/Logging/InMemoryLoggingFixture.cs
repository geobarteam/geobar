using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace GoSimple.Logging.Test.Logging
{

    public class InMemoryLogger : ILog
    {
        public static StringBuilder Content { get; set; }

        static InMemoryLogger()
        {
            InMemoryLogger.Content = new StringBuilder();
        }
        
        public void Log(string source, LogLevel level, string message, string correlationId = null)
        {
            string logTemplate = "Source: {0} - Level: {1} - Message:{2}";

            Content.AppendLine(String.Format(logTemplate, source, level, message));

            if (!String.IsNullOrEmpty(correlationId))
            {
                Content.Append(String.Format(" - CorrelationId: {0}", correlationId));
            }
        }
    }

    
    public class GivenInMemoryLogger : GivenWhenThen
    {
        public ILog MemoryLogger { get; set; }

        public override void Given()
        {
            this.MemoryLogger = new InMemoryLogger();
            InMemoryLogger.Content = new StringBuilder();
            Logger.Initialize(this.MemoryLogger);
        }
    }

    [TestFixture]
    public class WhenLogException : GivenInMemoryLogger
    {
        public Exception Ex { get; set; }

        public override void Given()
        {
            base.Given();
            this.Ex = new Exception("This is a dummy exception!", new Exception("This is the innerexception!"));
            
        }

        public override void When()
        {
            Logger.Error(this, this.Ex);
        }

        [Test]
        public void ErrorLogWasAddedToLogContent()
        {
            Assert.IsTrue(InMemoryLogger.Content.ToString().Contains(this.Ex.Message));
        }

        [Test]
        public void LevelWasError()
        {
            Assert.IsTrue(InMemoryLogger.Content.ToString().Contains("- Level: " + LogLevel.Error));
        }
    }

    public class GivenLogMessage : GivenInMemoryLogger
    {
        public string Message { get; set; }

        public override void Given()
        {
            base.Given();
            this.Message = "This is a log message with guid:" + Guid.NewGuid();
        }
    }

    [TestFixture]
    public class WhenLogDebugWithProperties : GivenLogMessage
    {
        
        public override void When()
        {
            Logger.Debug(this, this.Message);
        }

        [Test]
        public void ErrorLogWasAddedToLogContent()
        {
            Assert.IsTrue(InMemoryLogger.Content.ToString().Contains(this.Message));
        }

        [Test]
        public void LevelWasDebug()
        {
            Assert.IsTrue(InMemoryLogger.Content.ToString().Contains("- Level: " + LogLevel.Debug));
        }
    }

    [TestFixture]
    public class WhenLogWarnWithProperties : GivenLogMessage
    {

        public override void When()
        {
            Logger.Warn(this, this.Message);
        }

        [Test]
        public void ErrorLogWasAddedToLogContent()
        {
            Assert.IsTrue(InMemoryLogger.Content.ToString().Contains(this.Message));
        }

        [Test]
        public void LevelWasDebug()
        {
            Assert.IsTrue(InMemoryLogger.Content.ToString().Contains("- Level: " + LogLevel.Warning));
        }
    }   
}
