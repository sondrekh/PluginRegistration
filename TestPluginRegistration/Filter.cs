using Microsoft.VisualStudio.TestTools.UnitTesting;
using PluginRegistration.Helpers;
using PluginRegistration.Models;
using PluginRegistration.Requests;
using System.Threading.Tasks;
using TestPluginRegistration.Setup;

namespace TestPluginRegistration
{
    [TestClass]
    public class Filter_tests : TestBase
    {
        public SdkMessage message;

        [TestInitialize]
        public async Task setup()
        {
            message = await SharedSetup.Message(crm);
        }

        [TestMethod]
        public async Task get_filter()
        {
            // Arrange
            var filter = ObjectExamples.Filter(message.Id);
            var filterRequest = new FilterRequest(filter);

            // Act 
            var messageFilter = await crm.GetFirst<SdkMessageFilter>(filterRequest);

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(messageFilter.Id));
            Assert.IsFalse(string.IsNullOrEmpty(messageFilter.SdkMessageId));
        }

        [TestMethod]
        public async Task get_filterid()
        {
            // Act
            var filterId = await FilterHelper.GetFilterId(crm, message.Id, "contact");

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(filterId));
        }
    }
}
