using System.Collections.Generic;

namespace gRpcurlUI.Core.Converter.Proto.Model
{
    public class ProtoAnalyzeEntryResult
    {
        public ProtoServiceInformation ProtoServiceInformation { get; }

        public ProtoNameInformation ProtoNameInformation { get; }

        public IEnumerable<ProtoMessageInformation> ProtoMessageInformation { get; }

        public IEnumerable<ErrorInformation> ErrorInformation { get; }

        public ProtoAnalyzeEntryResult(ProtoServiceInformation protoServiceInformation, ProtoNameInformation protoNameInformation, IEnumerable<ProtoMessageInformation> protoMessageInformation, IEnumerable<ErrorInformation> errorInformation)
        {
            ProtoServiceInformation = protoServiceInformation;
            ProtoNameInformation = protoNameInformation;
            ProtoMessageInformation = protoMessageInformation;
            ErrorInformation = errorInformation;
        }
    }
}
