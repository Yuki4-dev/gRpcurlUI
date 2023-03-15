using System.Collections.Generic;

namespace gRpcurlUI.Core.Converter.Proto.Model
{
    public class ProtoMessageInformation
    {
        public string MessageName { get; }

        private readonly IList<ProtoMessageMemberInformation> _MemberInformation = new List<ProtoMessageMemberInformation>();
        public IEnumerable<ProtoMessageMemberInformation> MemberInformation => _MemberInformation;

        public ProtoMessageInformation(string messageName)
        {
            MessageName = messageName;
        }

        public void AddMemberInformation(ProtoMessageMemberInformation Information)
        {
            _MemberInformation.Add(Information);
        }

        public override string ToString()
        {
            return $"Name : {MessageName} ({string.Join(",", MemberInformation)})";
        }
    }

    public enum ProtoModuleType
    {
        TypeUnknown, TypePrimitive, TypeEnum, TypeMessage
    }

    public class ProtoMessageMemberInformation
    {
        public string PropertyName { get; }

        public string TypeName { get; }

        public bool IsRepeated { get; }

        public ProtoModuleType ModuleType { get; }

        protected ProtoMessageMemberInformation(ProtoModuleType moduleType, string propertyName, string typeName, bool isRepeated)
        {
            ModuleType = moduleType;
            PropertyName = propertyName;
            TypeName = typeName;
            IsRepeated = isRepeated;
        }

        public override string ToString()
        {
            return $"PropertyName : {PropertyName} TypeName : {TypeName}  ( ModuleType : {ModuleType} )";
        }

        public static ProtoMessageMemberInformation OfEnum(string propertyName, string typeName, bool isRepeated)
        {
            return new ProtoMessageMemberInformation(ProtoModuleType.TypeEnum, propertyName, typeName, isRepeated);
        }

        public static ProtoMessageMemberInformation OfPrimitive(string propertyName, string typeName, bool isRepeated)
        {
            return new ProtoMessageMemberInformation(ProtoModuleType.TypeEnum, propertyName, typeName, isRepeated);
        }

        public static ProtoMessageMemberInformation OfMessage(string propertyName, string typeName, bool isRepeated)
        {
            return new ProtoMessageMemberInformation(ProtoModuleType.TypeMessage, propertyName, typeName, isRepeated);
        }

        public static ProtoMessageMemberInformation OfUnKnown(string propertyName, string typeName, bool isRepeated)
        {
            return new ProtoMessageMemberInformation(ProtoModuleType.TypeUnknown, propertyName, typeName, isRepeated);
        }
    }
}
