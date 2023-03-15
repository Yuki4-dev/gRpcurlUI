using gRpcurlUI.Core.Converter.Proto.Model;
using System;

namespace gRpcurlUI.Core.Converter.Proto.Format
{
    public class EnumProtoValueFormatter : IProtoTypedValueFormatter
    {
        private readonly int defaultIndex = 0;

        public EnumProtoValueFormatter() : this(0) { }

        public EnumProtoValueFormatter(int index)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            defaultIndex = index;
        }

        public bool IsTarget(ProtoMessageMemberInformation memberInformation)
        {
            return memberInformation.ModuleType == ProtoModuleType.TypeEnum;
        }

        public string GetValueFormat(ProtoMessageMemberInformation memberInformation)
        {
            if (!IsTarget(memberInformation))
            {
                throw new InvalidOperationException($"{memberInformation}");
            }

            return defaultIndex.ToString();
        }
    }
}
