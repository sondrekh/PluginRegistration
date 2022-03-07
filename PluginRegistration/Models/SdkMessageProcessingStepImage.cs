using Dynamics.Basic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PluginRegistration.Models
{
    public class SdkMessageProcessingStepImage
    {
        [JsonProperty("sdkmessageprocessingstepimageid")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("_sdkmessageprocessingstepid_value")]
        public string SdkMessageProcessingStepId { get; set; }

        public string Attributes { get; set; } // Ops! On "Update"-steps, image-attributes are required 
        public string MessagePropertyName { get; set; }
        public string EntityAlias { get; set; }
        public int ImageType { get; set; }
        public bool IsManaged = false;

        public string Serialize()
        {
            var json = new JObject();

            json.AddIfProvided("name", Name);
            json.AddIfProvided("ismanaged", IsManaged);
            json.AddIfProvided("imagetype", ImageType);
            json.AddIfProvided("attributes", Attributes);
            json.AddIfProvided("entityalias", EntityAlias);
            json.AddIfProvided("messagepropertyname", MessagePropertyName);
            json.AddIfProvided("sdkmessageprocessingstepid@odata.bind", $"/sdkmessageprocessingsteps({SdkMessageProcessingStepId})");

            return json.ToString();
        }

        public override bool Equals(object obj)
        {
            if (this.GetType() != obj.GetType())
                return false;

            var image = (SdkMessageProcessingStepImage)obj;
            return (this.Name == image.Name &&
                    this.SdkMessageProcessingStepId == image.SdkMessageProcessingStepId);
        }
    }

    public enum ImageType
    {
        PreImage = 0,
        PostImage = 1,
        Both = 2
    }
}
