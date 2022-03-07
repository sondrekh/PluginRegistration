using Dynamics.Basic;
using PluginRegistration.Models;
using PluginRegistration.Requests;
using System.Threading.Tasks;

namespace PluginRegistration.Helpers
{
    public class MessageHelper
    {
        public static async Task<string> GetByName(Crm crm, string name)
        {
            var getMessage = new SdkMessage().WithName(name);
            var request = new MessageRequest(getMessage);
            var message = await crm.GetFirst<SdkMessage>(request);
            return message.Id;
        }
    }
}
