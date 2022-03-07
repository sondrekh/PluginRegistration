using Dynamics.Basic;
using PluginRegistration.Models;
using System;

namespace PluginRegistration.Interfaces
{
    public class ImageRequest : IRequest
    {
        public string body { get; set; }
        public string entityName { get; set; }
        public string expand { get; set; }
        public string fieldName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string filter { get; set; }
        public string recordId { get; set; }
        public string select { get; set; }

        public ImageRequest WithStepId(string stepId)
        {
            filter = $"&$filter=_sdkmessageprocessingstepid_value eq {stepId}";
            return this;
        }

        public ImageRequest(SdkMessageProcessingStepImage image)
        {
            body = image.Serialize();
            SetEntityName();
            SetSelect();
            filter = $"&$filter=" +
                $"_sdkmessageprocessingstepid_value eq {image.SdkMessageProcessingStepId} and " +
                $"name eq '{image.Name}'";
            recordId = image.Id;
        }

        public ImageRequest()
        {
            SetSelect();
            SetEntityName(); 
        }

        private void SetSelect()
        {
            select = "$select=_sdkmessageprocessingstepid_value,name";
        }

        private void SetEntityName()
        {
            entityName = "sdkmessageprocessingstepimages";
        }

        public IRequest WithId(string id)
        {
            recordId = id;
            return this;
        }
    }
}
