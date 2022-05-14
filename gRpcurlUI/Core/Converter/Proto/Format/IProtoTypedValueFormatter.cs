using gRpcurlUI.Core.Converter.Proto.Model;

namespace gRpcurlUI.Core.Converter.Proto.Format
{
    public interface IProtoTypedValueFormatter
    {
        bool IsTarget(ProtoMessageMemberInfomation memberInfomation);

        string GetValueFormat(ProtoMessageMemberInfomation memberInfomation);
    }
}
