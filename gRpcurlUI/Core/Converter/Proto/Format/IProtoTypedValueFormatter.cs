using gRpcurlUI.Core.Converter.Proto.Model;

namespace gRpcurlUI.Core.Converter.Proto.Format
{
    public interface IProtoTypedValueFormatter
    {
        bool IsTarget(ProtoMessageMemberInformation memberInformation);

        string GetValueFormat(ProtoMessageMemberInformation memberInformation);
    }
}
