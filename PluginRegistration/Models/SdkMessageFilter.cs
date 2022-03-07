using Newtonsoft.Json;

namespace PluginRegistration.Models
{
    public class SdkMessageFilter
    {
        [JsonProperty("sdkmessagefilterid")]
        public string Id { get; set; }
        [JsonProperty("primaryobjecttypecode")]
        public string PrimaryObjectTypeCode { get; set; }
        [JsonProperty("_sdkmessageid_value")]
        public string SdkMessageId { get; set; }

        public SdkMessageFilter TargetingEntity(string entityName)
        {
            PrimaryObjectTypeCode = entityName;
            return this;
        }

        public SdkMessageFilter WithMessageId(string messageId)
        {
            SdkMessageId = messageId;
            return this;
        }
    }
}
