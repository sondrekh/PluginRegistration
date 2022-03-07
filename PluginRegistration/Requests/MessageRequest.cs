using Dynamics.Basic;
using PluginRegistration.Models;
using System;

namespace PluginRegistration.Requests
{
    public class MessageRequest : IRequest
    {
        public string body { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string entityName { get; set; }
        public string expand { get; set; }
        public string fieldName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string filter { get; set; }
        public string recordId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string select { get; set; }
        public SdkMessage message;

        public MessageRequest(SdkMessage message)
        {
            entityName = "sdkmessages";
            select = "$select=name";
            filter = $"&$filter=name eq '{message.Name}'";
        }
    }
}
