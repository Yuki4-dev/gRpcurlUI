using System.Collections.Generic;

namespace gRpcurlUI.Core.Converter.Proto.Model
{
    public class ProtoMessageInfomation
    {
        public string MessageName { get; }

        private readonly IList<ProtoMessageMemberInfomation> _MemberInfomations = new List<ProtoMessageMemberInfomation>();
        public IEnumerable<ProtoMessageMemberInfomation> MemberInfomations => _MemberInfomations;

        public ProtoMessageInfomation(string messageName)
        {
            MessageName = messageName;
        }

        public void AddMemberInfomation(ProtoMessageMemberInfomation infomation)
        {
            _MemberInfomations.Add(infomation);
        }

        public override string ToString()
        {
            return $"Name : {MessageName} ({string.Join(",", MemberInfomations)})";
        }
    }

    public enum ProtoModuleType
    {
        TypeUnknown, TypePrimitive, TypeEnum, TypeMessage
    }

    public class ProtoMessageMemberInfomation
    {
        public string PropertyName { get; }

        public string TypeName { get; }

        public bool IsRepeated { get; }

        public ProtoModuleType ModuleType { get; }

        protected ProtoMessageMemberInfomation(ProtoModuleType moduleType, string propertyName, string typeName, bool isRepeated)
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

        public static ProtoMessageMemberInfomation OfEnum(string propertyName, string typeName, bool isRepeated)
        {
            return new ProtoMessageMemberInfomation(ProtoModuleType.TypeEnum, propertyName, typeName, isRepeated);
        }

        public static ProtoMessageMemberInfomation OfPrimitive(string propertyName, string typeName, bool isRepeated)
        {
            return new ProtoMessageMemberInfomation(ProtoModuleType.TypeEnum, propertyName, typeName, isRepeated);
        }

        public static ProtoMessageMemberInfomation OfMessage(string propertyName, string typeName, bool isRepeated)
        {
            return new ProtoMessageMemberInfomation(ProtoModuleType.TypeMessage, propertyName, typeName, isRepeated);
        }

        public static ProtoMessageMemberInfomation OfUnKnown(string propertyName, string typeName, bool isRepeated)
        {
            return new ProtoMessageMemberInfomation(ProtoModuleType.TypeUnknown, propertyName, typeName, isRepeated);
        }
    }
}
