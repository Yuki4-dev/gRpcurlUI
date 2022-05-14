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

        public bool IsTarget(ProtoMessageMemberInfomation memberInfomation)
        {
            return memberInfomation.ModuleType == ProtoModuleType.TypePrimitive
                && memberInfomation.TypeName == typeName;
        }

        public string GetValueFormat(ProtoMessageMemberInfomation memberInfomation)
        {
            if (!IsTarget(memberInfomation))
            {
                throw new InvalidOperationException($"{memberInfomation}");
            }

            return defaultValue;
        }
    }
}
