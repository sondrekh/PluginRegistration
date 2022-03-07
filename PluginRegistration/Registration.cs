using Dynamics.Basic;
using PluginRegistration.Helpers;
using PluginRegistration.Interfaces;
using PluginRegistration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace PluginRegistration
{
    public class Registration
    {
        public static async Task<List<T>> FindRecordsNotInList<T>(Crm crm, IRequest request, List<T> list)
        {
            var existingRecords = await crm.GetList<T>(request);
            var unwantedRecords = new List<T>();

            foreach (var record in existingRecords)
                if (!list.Contains(record))
                    unwantedRecords.Add(record);

            return unwantedRecords;
        }

        public static string GetIdFromGenericObjectT<T>(T record)
        {
            Type t = typeof(T);
            PropertyInfo[] propInfos = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var info in propInfos)
            {
                if (info.Name == "Id")
                    return (string)info.GetValue(record);
            }
            return ""; 
        }

        public static async Task<List<RecordResponse>> DeleteRecords<T>(Crm crm, List<T> records)
        {
            var recordResponses = new List<RecordResponse>();

            foreach (var record in records)
            {
                if (typeof(T) == typeof(SdkMessageProcessingStepImage))
                    recordResponses.Add(await ImageHelper.DeleteImage(crm, (SdkMessageProcessingStepImage)(object)record));

                else if (typeof(T) == typeof(SdkMessageProcessingStep))
                    recordResponses.Add(await StepHelper.DeleteStep(crm, (SdkMessageProcessingStep)(object)record));

                else if (typeof(T) == typeof(PluginType))
                    recordResponses.Add(await PluginHelper.DeletePlugin(crm, (PluginType)(object)record));
            }

            return recordResponses;
        }

        public static async Task<RecordResponse> Upsert<T>(Crm crm, IRequest request)
        {
            var list = await crm.GetList<T>(request);
            HttpResponseMessage response = null;

            if (list.Count == 0)
                response = await crm.Post(request);

            else if (list.Count == 1)
            {
                request.recordId = GetIdFromGenericObjectT(list.First());
                response = await crm.Patch(request);
            }

            return new RecordResponse(response, typeof(T));
        }

        public async static Task FullRegistration(Crm crm, IAssembly setup)
        {
            var assemblyResponse = await AssemblyHelper.UpsertAssembly(crm, setup.Assembly);
            setup.AddAssemblyIdToPlugins(assemblyResponse.recordId);
            await PluginHelper.DeleteAssemblyPluginsNotInList(crm, setup.Plugins);

            foreach (var pluginsetup in setup.Plugins)
            {
                var pluginResponse = await PluginHelper.UpsertPlugin(crm, pluginsetup.plugin);
                pluginsetup.AddSteps(pluginResponse.recordId);

                foreach (var step in pluginsetup.steps)
                    await step.ResolveIds(crm);

                await StepHelper.DeleteStepsNotInList(crm, pluginsetup.steps);

                foreach (var stepSetup in pluginsetup.steps)
                {
                    var stepResponse = await StepHelper.UpsertStep(crm, stepSetup);

                    if (stepSetup.Images.Count == 0)
                        continue;

                    stepSetup.AddStepIdToImages(stepResponse.recordId);
                    await ImageHelper.DeleteImagesNotInList(crm, stepSetup.Images);

                    foreach (var image in stepSetup.Images)
                        await ImageHelper.UpsertImage(crm, image);
                }
            }
        }
    }
}
