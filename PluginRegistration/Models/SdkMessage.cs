using Newtonsoft.Json;

namespace PluginRegistration.Models
{
    public class SdkMessage
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("sdkmessageid")]
        public string Id { get; set; }

        public SdkMessage WithName(string name)
        {
            Name = name;
            return this;
        }
    }
}
