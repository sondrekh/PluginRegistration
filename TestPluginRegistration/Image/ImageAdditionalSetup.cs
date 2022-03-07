using Dynamics.Basic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PluginRegistration;
using PluginRegistration.Helpers;
using PluginRegistration.Interfaces;
using PluginRegistration.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestPluginRegistration.Setup;

namespace TestPluginRegistration.Image
{
    [TestClass]
    public class Image_tests_additional_setup : ImageSharedSetup
    {
        IRequest imageArequest { get; set; }
        IRequest imageBrequest { get; set; }
        List<SdkMessageProcessingStepImage> images { get; set; }
        List<SdkMessageProcessingStepImage> wantedImages { get; set; }


        [TestInitialize]
        public async Task RegisterTwoImages_ReturnWantedList()
        {
            images = ObjectExamples.ImageList(stepRequest.recordId);
            imageArequest = new ImageRequest(images[0]);
            imageBrequest = new ImageRequest(images[1]);
            var imageA = await crm.Post(imageArequest);
            var imageB = await crm.Post(imageBrequest);
            imageArequest.recordId = imageA.GetCreatedId();
            imageBrequest.recordId = imageB.GetCreatedId();
            wantedImages = new List<SdkMessageProcessingStepImage>();
            wantedImages.Add(images[0]);
        }


        [TestMethod]
        public async Task find_unwanted_images()
        {
            // Act
            var unwantedImages = await ImageHelper.FindUnwantedImages(crm, wantedImages);

            // Assert
            Assert.AreEqual(images[1], unwantedImages[0]);
            await crm.Delete(imageArequest);
            await crm.Delete(imageBrequest);
        }

        [TestMethod]
        public async Task delete_unwanted_images()
        {
            // Arrange
            var unwantedImages = await ImageHelper.FindUnwantedImages(crm, wantedImages);

            // Act 
            List<RecordResponse> deletionResponses = await Registration.DeleteRecords(crm, unwantedImages);

            // Assert
            Assert.AreEqual(1, deletionResponses.Count);
            Assert.AreEqual(204, deletionResponses.First().statusCode);
            await crm.Delete(imageArequest);
        }

        [TestMethod]
        public async Task upsert_step_already_existing()
        {
            // Arrange
            var step = ObjectExamples.Step(pluginRequest.recordId);
            await step.ResolveIds(crm);

            // Act
            var response = await StepHelper.UpsertStep(crm, step);

            // Assert
            Assert.AreEqual(204, response.statusCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(response.recordId));
        }
    }
}
