using Dynamics.Basic;
using PluginRegistration.Models;

namespace PluginRegistration.Requests
{
    public class PluginRequest : IRequest
    {
        public string body { get; set; }
        public string entityName { get; set; }
        public string expand { get; set; }
        public string fieldName { get; set; }
        public string filter { get; set; }
        public string recordId { get; set; }
        public string select { get; set; }

        public PluginRequest(PluginType plugin)
        {
            recordId = plugin.Id;
            body = plugin.Serialize();
            SetEntityName();
            SetSelect();
            filter = $"&$filter=" +
                $"typename eq '{plugin.TypeName}' and " +
                $"_pluginassemblyid_value eq {plugin.PluginAssemblyId}";
        }

        public PluginRequest WithOnlyFilterOnAssembly(string assemblyId)
        {
            filter = $"&$filter=_pluginassemblyid_value eq {assemblyId}";
            return this;
        }

        private void SetSelect()
        {
            select = "$select=plugintypeid,typename,_pluginassemblyid_value";
        }

        private void SetEntityName()
        {
            entityName = "plugintypes";
        }

        public PluginRequest()
        {
            SetEntityName();
            SetSelect();
        }

        public IRequest WithId(string id)
        {
            recordId = id;
            return this; 
        }
    }
}
