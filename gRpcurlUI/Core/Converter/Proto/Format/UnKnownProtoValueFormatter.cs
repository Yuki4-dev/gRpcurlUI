using gRpcurlUI.Core.Converter.Proto.Model;
using System;

namespace gRpcurlUI.Core.Converter.Proto.Format
{
    public class UnKnownProtoValueFormatter : IProtoTypedValueFormatter
    {
        public static IProtoTypedValueFormatter Default { get; } = new UnKnownProtoValueFormatter();

        public bool IsTarget(ProtoMessageMemberInfomation memberInfomation)
        {
            return memberInfomation.ModuleType == ProtoModuleType.TypeUnknown;
        }

        public string GetValueFormat(ProtoMessageMemberInfomation memberInfomation)
        {
            if (!IsTarget(memberInfomation))
            {
                throw new InvalidOperationException($"{memberInfomation}");
            }

            return string.Empty;
        }
    }
}
