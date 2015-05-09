using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoSimple.Logging;
using NUnit.Framework;

namespace Log4Net.Test
{
    [TestFixture]
    public class WhenLogGivenTextLayout : GivenTextLayout
    {
        
        public override void When()
        {
            Logger.Debug(this, "Hello World!");
        }

        [Test]
        public void LoggerDidNotRaiseAnExceptionEvent()
        {
            Assert.IsNull(this.LoggerExceptionEvent);
        }

        [Test]
        public void ThenFileShouldExist()
        {
            Assert.IsTrue(File.Exists(LogFilePath), "Log file was not found on:" + LogFilePath);
        }

        [Test]
        public void ThenFileShouldContainHelloWorld()
        {
            string result = File.ReadAllText(LogFilePath);
            Assert.IsTrue(result.Contains("Hello World!"), "Hello World! was not found in log file");
        }
    }
}
