using Dynamics.Basic;
using PluginRegistration.Interfaces;
using PluginRegistration.Models;
using PluginRegistration.Requests;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PluginRegistration.Helpers
{
    public class PluginHelper
    {
        public async static Task<List<PluginType>> FindUnwantedPlugins(Crm crm, List<PluginType> list)
        {
            var assemblyId = list.First().PluginAssemblyId;
            var getPluginRequest = new PluginRequest().WithOnlyFilterOnAssembly(assemblyId);
            return await Registration.FindRecordsNotInList(crm, getPluginRequest, list);
        }

        public async static Task DeleteAssemblyPluginsNotInList(Crm crm, List<PluginType> plugins)
        {
            var unwantedPlugins = await FindUnwantedPlugins(crm, plugins);
            await Registration.DeleteRecords(crm, unwantedPlugins);
        }

        public async static Task DeleteAssemblyPluginsNotInList(Crm crm, List<IPluginSetup> plugins)
        {
            var pluginList = new List<PluginType>();
            foreach (var pluginSetup in plugins)
                pluginList.Add(pluginSetup.plugin);
            await DeleteAssemblyPluginsNotInList(crm, pluginList);
        }

        public async static Task<RecordResponse> DeletePlugin(Crm crm, PluginType plugin)
        {
            var steps = await crm.GetList<SdkMessageProcessingStep>(new StepRequest().WithPluginId(plugin.Id));
            await Registration.DeleteRecords(crm, steps);
            var response = await crm.Delete(new PluginRequest(plugin));
            return new RecordResponse(response, plugin.GetType());
        }

        public async static Task<RecordResponse> UpsertPlugin(Crm crm, PluginType plugin)
        {
            var request = new PluginRequest(plugin);
            return await Registration.Upsert<PluginType>(crm, request);
        }
    }
}
