using System.Collections.Generic;

namespace gRpcurlUI.Core.Converter.Proto.Model
{
    public class ProtoServiceInformation
    {
        public string ServiceName { get; }

        public IEnumerable<ProtoServiceMethodInformation> ProtoServiceMethods { get; }

        public ProtoServiceInformation(string serviceName, IEnumerable<ProtoServiceMethodInformation> protoServiceMethods)
        {
            ServiceName = serviceName;
            ProtoServiceMethods = protoServiceMethods;
        }
    }

    public class ProtoServiceMethodInformation
    {
        public string MethodName { get; }

        public ProtoMessageInformation Request { get; }

        public ProtoMessageInformation Response { get; }

        public ProtoServiceMethodInformation(string methodName, ProtoMessageInformation request, ProtoMessageInformation response)
        {
            MethodName = methodName;
            Request = request;
            Response = response;
        }
    }
}
