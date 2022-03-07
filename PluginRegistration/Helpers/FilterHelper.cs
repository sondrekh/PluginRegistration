using Dynamics.Basic;
using PluginRegistration.Models;
using PluginRegistration.Requests;
using System.Threading.Tasks;

namespace PluginRegistration.Helpers
{
    public static class FilterHelper
    {
        public static async Task<string> GetFilterId(Crm crm, string messageId, string entityName)
        {
            var getFilter = new SdkMessageFilter()
            {
                SdkMessageId = messageId,
                PrimaryObjectTypeCode = entityName
            };

            var request = new FilterRequest(getFilter);
            var filter = await crm.GetFirst<SdkMessageFilter>(request);
            //Console.WriteLine($"Filter: {entityName}");
            return filter.Id;
        }
    }
}
