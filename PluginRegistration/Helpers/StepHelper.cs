using Dynamics.Basic;
using PluginRegistration.Interfaces;
using PluginRegistration.Models;
using PluginRegistration.Requests;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PluginRegistration.Helpers
{
    public class StepHelper
    {
        public static async Task<List<SdkMessageProcessingStep>> FindUnwantedSteps(Crm crm, List<SdkMessageProcessingStep> wantedSteps)
        {
            var pluginId = wantedSteps.First().PluginTypeId;
            var request = new StepRequest().WithPluginId(pluginId);
            return await Registration.FindRecordsNotInList(crm, request, wantedSteps);
        }

        public async static Task<RecordResponse> DeleteStep(Crm crm, SdkMessageProcessingStep step)
        {
            var images = await crm.GetList<SdkMessageProcessingStepImage>(new ImageRequest().WithStepId(step.Id));
            await Registration.DeleteRecords(crm, images);
            var response = await crm.Delete(new StepRequest(step));
            return new RecordResponse(response, step.GetType());
        }

        public async static Task DeleteStepsNotInList(Crm crm, List<SdkMessageProcessingStep> steps)
        {
            var unwantedSteps = await FindUnwantedSteps(crm, steps);
            foreach (var step in unwantedSteps)
                await DeleteStep(crm, step);
        }


        public async static Task<RecordResponse> UpsertStep(Crm crm, SdkMessageProcessingStep step)
        {
            var request = new StepRequest(step);
            return await Registration.Upsert<SdkMessageProcessingStep>(crm, request);
        }
    }
}
