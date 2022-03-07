using Microsoft.VisualStudio.TestTools.UnitTesting;
using PluginRegistration.Helpers;
using PluginRegistration.Models;
using PluginRegistration.Requests;
using System.Threading.Tasks;
using TestPluginRegistration.Setup;

namespace TestPluginRegistration
{
    [TestClass]
    public class Message_tests : TestBase
    {
        public SdkMessage message;

        [TestInitialize]
        public void setup()
        {
            message = new SdkMessage().WithName("create");
        }

        [TestMethod]
        public async Task get_sdk_message()
        {
            // Arrange
            var messageRequest = new MessageRequest(message);

            // Act 
            message = await crm.GetFirst<SdkMessage>(messageRequest);

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(message.Id));
        }

        [TestMethod]
        public async Task get_sdkmessage_by_name()
        {
            // Arrange
            var name = "create";

            // Act
            message.Id = await MessageHelper.GetByName(crm, name);

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(message.Id));
        }
    }
}
