using Dynamics.Basic;
using PluginRegistration.Interfaces;
using PluginRegistration.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PluginRegistration.Helpers
{
    public class ImageHelper
    {
        public static async Task<List<SdkMessageProcessingStepImage>> FindUnwantedImages(Crm crm, List<SdkMessageProcessingStepImage> wantedImages)
        {
            var stepId = wantedImages.First().SdkMessageProcessingStepId;
            var request = new ImageRequest().WithStepId(stepId);
            return await Registration.FindRecordsNotInList(crm, request, wantedImages);
        }

        public static async Task<RecordResponse> DeleteImage(Crm crm, SdkMessageProcessingStepImage image)
        {
            var request = new ImageRequest(image);
            var response = await crm.Delete(request);
            return new RecordResponse(response, image.GetType());
        }

        public async static Task<RecordResponse> UpsertImage(Crm crm, SdkMessageProcessingStepImage image)
        {
            var request = new ImageRequest(image);
            return await Registration.Upsert<SdkMessageProcessingStepImage>(crm, request);
        }

        public async static Task DeleteImagesNotInList(Crm crm, List<SdkMessageProcessingStepImage> images)
        {
            var unwantedImages = await FindUnwantedImages(crm, images);
            await Registration.DeleteRecords(crm, unwantedImages);
        }
    }
}
