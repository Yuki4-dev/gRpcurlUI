using gRpcurlUI.Core.Converter.Proto.Model;
using System;

namespace gRpcurlUI.Core.Converter.Proto.Format
{
    public class UnKnownProtoValueFormatter : IProtoTypedValueFormatter
    {
        public static IProtoTypedValueFormatter Default { get; } = new UnKnownProtoValueFormatter();

        public bool IsTarget(ProtoMessageMemberInformation memberInformation)
        {
            return memberInformation.ModuleType == ProtoModuleType.TypeUnknown;
        }

        public string GetValueFormat(ProtoMessageMemberInformation memberInformation)
        {
            if (!IsTarget(memberInformation))
            {
                throw new InvalidOperationException($"{memberInformation}");
            }

            return string.Empty;
        }
    }
}
