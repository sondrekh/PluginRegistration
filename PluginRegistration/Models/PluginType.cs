using Dynamics.Basic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace PluginRegistration.Models
{
    public class PluginType
    {
        [JsonProperty("plugintypeid")]
        public string Id { get; set; }
        [JsonProperty("_pluginassemblyid_value")]
        public string PluginAssemblyId { get; set; } // Required
        [JsonProperty("typename")]
        public string TypeName { get; set; } // Required
        public string FriendlyName { get; set; } // Required (?) 
        public string Name { get; set; } // Required
        public string Description { get; set; } // Optional
        public List<SdkMessageProcessingStep> Steps { get; set; }

        public PluginType()
        {
            Steps = new List<SdkMessageProcessingStep>();
        }

        public string Serialize()
        {
            var json = new JObject();

            json.AddIfProvided("typename", TypeName);
            json.AddIfProvided("friendlyname", TypeName);
            json.AddIfProvided("name", TypeName);
            json.AddIfProvided("description", Description);
            json.AddIfProvided("pluginassemblyid@odata.bind", $"/pluginassemblies({PluginAssemblyId})");

            return json.ToString();
        }

        public void AddPluginIdToSteps(string pluginId)
        {
            foreach (var step in Steps)
                step.PluginTypeId = pluginId;
        }

        public PluginType WithAssemblyId(string assemblyId)
        {
            PluginAssemblyId = assemblyId;
            return this;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType())
                return false;

            var plugin = (PluginType)obj;

            var equalTypename = this.TypeName == plugin.TypeName;
            var equalAssemblyId = this.PluginAssemblyId == plugin.PluginAssemblyId;
            var areEqual = equalTypename && equalAssemblyId;

            return areEqual;
        }
    }
}
