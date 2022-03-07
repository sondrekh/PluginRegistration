using Dynamics.Basic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PluginRegistration;
using PluginRegistration.Helpers;
using PluginRegistration.Models;
using System.Threading.Tasks;

namespace TestPluginRegistration.Image
{
    [TestClass]
    public class Image_tests : ImageSharedSetup
    {
        [TestMethod]
        public async Task register_image()
        {
            // Act 
            var response = await crm.Post(imageRequest);
            new RecordResponse(response, typeof(SdkMessageProcessingStepImage));

            // Assert
            imageRequest.recordId = response.GetCreatedId();
            Assert.AreEqual(204, (int)response.StatusCode);
        }

        [TestMethod]
        public async Task upsert_image_not_existing()
        {
            // Act
            RecordResponse response = await ImageHelper.UpsertImage(crm, image);

            // Assert
            Assert.AreEqual(204, response.statusCode);
            Assert.IsFalse(string.IsNullOrEmpty(response.recordId));
            imageRequest.recordId = response.recordId;
        }
    }
}