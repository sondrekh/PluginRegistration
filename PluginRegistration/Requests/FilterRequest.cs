using Dynamics.Basic;
using PluginRegistration.Models;
using System;

namespace PluginRegistration.Requests
{
    public class FilterRequest : IRequest
    {
        public string body { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string entityName { get; set; }
        public string expand { get; set; }
        public string fieldName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string filter { get; set; }
        public string recordId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string select { get; set; }

        public FilterRequest(SdkMessageFilter messageFilter)
        {
            entityName = "sdkmessagefilters";
            select = "$select=sdkmessagefilterid,_sdkmessageid_value";
            filter = $"&$filter=" +
                $"primaryobjecttypecode eq '{messageFilter.PrimaryObjectTypeCode}' and " +
                $"_sdkmessageid_value eq {messageFilter.SdkMessageId}";
        }

        public IRequest WithId(string id)
        {
            throw new NotImplementedException();
        }
    }
}
