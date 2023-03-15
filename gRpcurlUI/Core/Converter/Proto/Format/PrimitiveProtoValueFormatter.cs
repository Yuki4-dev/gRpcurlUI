using gRpcurlUI.Core.Converter.Proto.Model;
using System;

namespace gRpcurlUI.Core.Converter.Proto.Format
{
    public class PrimitiveProtoValueFormatter : IProtoTypedValueFormatter
    {
        private readonly string typeName;

        public readonly string defaultValue;

        public PrimitiveProtoValueFormatter(string typeName, string defaultValue)
        {
            this.typeName = typeName;
            this.defaultValue = defaultValue;
        }

        public bool IsTarget(ProtoMessageMemberInformation memberInformation)
        {
            return memberInformation.ModuleType == ProtoModuleType.TypePrimitive
                && memberInformation.TypeName == typeName;
        }

        public string GetValueFormat(ProtoMessageMemberInformation memberInformation)
        {
            if (!IsTarget(memberInformation))
            {
                throw new InvalidOperationException($"{memberInformation}");
            }

            return defaultValue;
        }
    }
}
