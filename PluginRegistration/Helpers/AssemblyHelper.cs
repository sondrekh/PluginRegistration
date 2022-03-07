using Dynamics.Basic;
using PluginRegistration.Models;
using PluginRegistration.Requests;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PluginRegistration.Helpers
{
    public static class AssemblyHelper
    {
        public static async Task<RecordResponse> RegisterAssembly(Crm crm, string path)
        {
            var assembly = new PluginAssembly(path);
            var request = new AssemblyRequest(assembly);
            var response = await crm.Post(request);
            return new RecordResponse(response, typeof(PluginAssembly));
        }

        public async static Task<RecordResponse> UpsertAssemblyWithPath(Crm crm, string path)
        {
            var assembly = new PluginAssembly(path);
            return await UpsertAssembly(crm, assembly);
        }

        public static async Task<RecordResponse> UpsertAssembly(Crm crm, PluginAssembly assembly)
        {
            var request = new AssemblyRequest(assembly);
            var list = await crm.GetList<PluginAssembly>(request);
            HttpResponseMessage response = null;

            if (list.Count == 0)
                response = await crm.Post(request);

            else if (list.Count == 1)
            {
                var assemblyId = list.First().Id;
                response = await crm.Patch(request.WithId(assemblyId));
            }
            Console.WriteLine($"Assembly: {assembly.Name}");
            return new RecordResponse(response, typeof(PluginAssembly));
        }

        public static async Task<RecordResponse> DeleteAssembly(Crm crm, string assemblyId)
        {
            var response = await crm.Delete(new AssemblyRequest().WithId(assemblyId));
            return new RecordResponse(response, typeof(PluginAssembly)); 
        }
    }
}
