using Dynamics.Basic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PluginRegistration.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PluginRegistration.Models
{
    public class SdkMessageProcessingStep
    {
        public string Configuration { get; set; }
        public int Mode { get; set; }
        public int Rank = 1; // 1 by default? 
        public int Stage { get; set; }
        public string FilteringAttributes { get; set; }
        [JsonProperty("_eventhandler_value")]
        public string PluginTypeId { get; set; }
        [JsonProperty("_sdkmessageid_value")]
        public string SdkMessageId { get; set; }
        [JsonProperty("_sdkmessagefilterid_value")]
        public string SdkMessageFilterId { get; set; }
        [JsonProperty("sdkmessageprocessingstepid")]
        public string Id { get; set; }
        public string Message { get; set; }
        public string Entity { get; set; }
        public int SupportedDeployment = 0;
        public int InvocationSource { get; set; }
        public List<SdkMessageProcessingStepImage> Images { get; set; }

        public SdkMessageProcessingStep()
        {
            Images = new List<SdkMessageProcessingStepImage>();
        }

        public string Serialize()
        {
            var json = new JObject();

            json.AddIfProvided("rank", Rank);
            json.AddIfProvided("mode", Mode);
            json.AddIfProvided("stage", Stage);
            json.AddIfProvided("configuration", Configuration);
            json.AddIfProvided("name", $"{Message}.{Entity}");
            json.AddIfProvided("invocationsource", InvocationSource);
            json.AddIfProvided("supporteddeployment", SupportedDeployment);
            json.AddIfProvided("filteringattributes", FilteringAttributes);
            json.AddIfProvided("plugintypeid@odata.bind", $"/plugintypes({PluginTypeId})");
            json.AddIfProvided("sdkmessageid@odata.bind", $"/sdkmessages({SdkMessageId})");
            json.AddIfProvided("sdkmessagefilterid@odata.bind", $"/sdkmessagefilters({SdkMessageFilterId})");

            return json.ToString();
        }

        public void AddStepIdToImages(string stepId)
        {
            foreach (var image in Images)
                image.SdkMessageProcessingStepId = stepId;
        }

        public override bool Equals(object obj)
        {
            if (this.GetType() != obj.GetType())
                return false;

            var step = (SdkMessageProcessingStep)obj;
            var samePlugin = this.PluginTypeId == step.PluginTypeId;
            var sameMessage = this.SdkMessageId == step.SdkMessageId;
            var sameFilter = this.SdkMessageFilterId == step.SdkMessageFilterId;
            return (samePlugin && sameMessage && sameFilter);
        }

        public async Task ResolveIds(Crm crm)
        {
            SdkMessageId = await MessageHelper.GetByName(crm, Message);
            SdkMessageFilterId = await FilterHelper.GetFilterId(crm, SdkMessageId, Entity);
            return;
        }
    }
    public enum Mode
    {
        Sync = 0,
        Async = 1
    }

    public enum Stage
    {
        preValidate = 10,
        preOperation = 20,
        postOperation = 40
    }

    public enum InvocationSource
    {
        Parent = 0,
        Child = 1
    }
}