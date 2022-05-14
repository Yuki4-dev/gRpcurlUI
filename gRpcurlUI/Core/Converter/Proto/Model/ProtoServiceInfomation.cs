using System.Collections.Generic;

namespace gRpcurlUI.Core.Converter.Proto.Model
{
    public class ProtoServiceInfomation
    {
        public string ServiceName { get; }

        public IEnumerable<ProtoServiceMethodInfomation> ProtoServiceMethods { get; }

        public ProtoServiceInfomation(string serviceName, IEnumerable<ProtoServiceMethodInfomation> protoServiceMethods)
        {
            ServiceName = serviceName;
            ProtoServiceMethods = protoServiceMethods;
        }
    }

    public class ProtoServiceMethodInfomation
    {
        public string MethodName { get; }

        public ProtoMessageInfomation Request { get; }

        public ProtoMessageInfomation Response { get; }

        public ProtoServiceMethodInfomation(string methodName, ProtoMessageInfomation request, ProtoMessageInfomation response)
        {
            MethodName = methodName;
            Request = request;
            Response = response;
        }
    }
}
