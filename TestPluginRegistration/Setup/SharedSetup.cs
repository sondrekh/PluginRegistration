using Dynamics.Basic;
using PluginRegistration;
using PluginRegistration.Models;
using PluginRegistration.Requests;
using System.Threading.Tasks;

namespace TestPluginRegistration.Setup
{
    public class SharedSetup
    {
        public static async Task<IRequest> Assembly(Crm crm, string assemblyPath)
        {
            var assembly = new PluginAssembly(assemblyPath);
            var request = new AssemblyRequest(assembly);
            var response = await crm.Post(request);
            request.recordId = response.GetCreatedId();
            new RecordResponse(response, assembly.GetType());
            return request;
        }

        public static async Task<IRequest> Plugin(Crm crm, string assemblyId)
        {
            var plugin = ObjectExamples.Plugin(assemblyId);
            var request = new PluginRequest(plugin);
            var response = await crm.Post(request);
            request.recordId = response.GetCreatedId();
            new RecordResponse(response, plugin.GetType());
            return request;
        }

        public static async Task<SdkMessage> Message(Crm crm)
        {
            var message = new SdkMessage().WithName("create");
            var request = new MessageRequest(message);
            return await crm.GetFirst<SdkMessage>(request);
        }

        public static async Task<IRequest> Step(Crm crm, string pluginId)
        {
            var step = ObjectExamples.Step(pluginId);
            await step.ResolveIds(crm);
            var request = new StepRequest(step);
            var response = await crm.Post(request);
            request.recordId = response.GetCreatedId();
            new RecordResponse(response, step.GetType());
            return request;
        }
    }
}
