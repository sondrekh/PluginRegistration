using Dynamics.Basic;
using PluginRegistration.Models;
using System;

namespace PluginRegistration.Requests
{
    public class AssemblyRequest : IRequest
    {
        public string body { get; set; }
        public string entityName { get; set; }
        public string expand { get; set; }
        public string fieldName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string filter { get; set; }
        public string recordId { get; set; }
        public string select { get; set; }

        public AssemblyRequest(PluginAssembly assembly)
        {
            recordId = assembly.Id;
            body = assembly.Serialize();
            SetEntityName();
            SetSelect();
            filter = $"&$filter=name eq '{assembly.Name}'";
        }

        private void SetSelect()
        {
            select = "$select=pluginassemblyid";
        }

        private void SetEntityName()
        {
            entityName = "pluginassemblies";
        }

        public AssemblyRequest()
        {
            SetSelect();
            SetEntityName();
        }

        public IRequest WithId(string id)
        {
            recordId = id;
            return this;
        }
    }
}
