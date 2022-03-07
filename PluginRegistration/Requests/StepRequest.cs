using Dynamics.Basic;
using PluginRegistration.Models;

namespace PluginRegistration.Requests
{
    public class StepRequest : IRequest
    {
        public string body { get; set; }
        public string entityName { get; set; }
        public string expand { get; set; }
        public string fieldName { get; set; }
        public string filter { get; set; }
        public string recordId { get; set; }
        public string select { get; set; }

        public StepRequest WithPluginId(string pluginId)
        {
            filter = $"&$filter=_plugintypeid_value eq {pluginId}";
            return this;
        }

        public StepRequest(SdkMessageProcessingStep step)
        {
            recordId = step.Id;
            body = step.Serialize();
            SetEntityName();
            SetSelect();
            filter = $"&$filter=" +
                $"_plugintypeid_value eq {step.PluginTypeId} and " +
                $"_sdkmessageid_value eq {step.SdkMessageId} and " +
                $"_sdkmessagefilterid_value eq {step.SdkMessageFilterId}";
        }

        private void SetSelect()
        {
            select = "$select=sdkmessageprocessingstepid,_eventhandler_value,_sdkmessageid_value,_sdkmessagefilterid_value";
        }

        private void SetEntityName()
        {
            entityName = "sdkmessageprocessingsteps";
        }

        public IRequest WithId(string id)
        {
            recordId = id;
            return this;
        }

        public StepRequest()
        {
            SetSelect();
            SetEntityName();
        }
    }
}
