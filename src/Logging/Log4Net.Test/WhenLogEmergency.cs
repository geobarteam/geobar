using System.IO;
using GoSimple.Logging;
using NUnit.Framework;

namespace Log4Net.Test
{
    [TestFixture]
    public class WhenLogEmergency : GivenTextLayout
    {
        protected string LogMessage = "This is an Emergency message";

        public EmergencyEventArg EmergencyEvent { get; set; }

        public override void When()
        {
            Logger.EmergencyHandler += Logger_EmergencyHandler;
            Logger.Emergency(this, LogMessage);
        }

        public void Logger_EmergencyHandler(object sender, EmergencyEventArg e)
        {
            this.EmergencyEvent = e;
        }

        [Test]
        public void ThenLogShouldContainValidEmergencyMessage()
        {
            string result = File.ReadAllText(GivenTextLayout.LogFilePath);
            Assert.IsTrue(result.Contains(this.LogMessage), "log does not contain Emergency message!");
        }

        [Test]
        public void ThenLogShouldRaiseEmergencyEvent()
        {
            Assert.IsNotNull(this.EmergencyEvent);            
        }
    }
}
