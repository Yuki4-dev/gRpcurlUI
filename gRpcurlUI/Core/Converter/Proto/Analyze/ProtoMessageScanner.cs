using gRpcurlUI.Core.Converter.Proto.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace gRpcurlUI.Core.Converter.Proto.Analyze
{
    public class ProtoMessageScanner
    {
        private const string MESSAGE = "message";

        private const string REPEARTED = "repeated";

        private ProtoMessageInformation? current;

        private readonly IList<ProtoMessageInformation> protoMessageInformation = new List<ProtoMessageInformation>();

        private readonly ProtoNameInformation protoNameInformation;

        public ProtoMessageScanner(ProtoNameInformation protoNameInformation)
        {
            this.protoNameInformation = protoNameInformation;
        }

        public bool ReadLine(string line, out string errorMessage)
        {
            var trimLine = line.Trim();
            if (string.IsNullOrEmpty(trimLine))
            {
                errorMessage = "空白行です。";
                return false;
            }

            var splitLine = trimLine.Split(' ');
            if (splitLine.Length > 1)
            {
                var messageType = splitLine[0];
                var messageName = splitLine[1];
                if (messageType == MESSAGE)
                {
                    current = new ProtoMessageInformation(messageName);
                    protoMessageInformation.Add(current);
                    errorMessage = string.Empty;
                    return true;
                }
                else if (current != null)
                {
                    if (AddMember(splitLine))
                    {
                        errorMessage = string.Empty;
                        return true;
                    }
                }
            }

            errorMessage = $"無効な行です。 : {line}";
            return false;
        }

        public ProtoMessageInformation[] GetProtoMessageInformation()
        {
            return protoMessageInformation.ToArray();
        }

        private bool AddMember(string[] splitLine)
        {
            if (current is null)
            {
                throw new InvalidOperationException();
            }

            if (splitLine.Length < 4)
            {
                return false;
            }

            var isRepeated = false;
            if (splitLine[0] == REPEARTED)
            {
                if (splitLine.Length < 5)
                {
                    return false;
                }

                isRepeated = true;
            }

            string typeName;
            string propertyName;
            if (isRepeated)
            {
                typeName = splitLine[1];
                propertyName = splitLine[2];
            }
            else
            {
                typeName = splitLine[0];
                propertyName = splitLine[1];
            }

            if (protoNameInformation.EnumNames.Contains(typeName))
            {
                current.AddMemberInformation(ProtoMessageMemberInformation.OfEnum(propertyName, typeName, isRepeated));
            }
            else if (protoNameInformation.MessageNames.Contains(typeName))
            {
                current.AddMemberInformation(ProtoMessageMemberInformation.OfMessage(propertyName, typeName, isRepeated));
            }
            else
            {
                current.AddMemberInformation(ProtoMessageMemberInformation.OfUnKnown(propertyName, typeName, isRepeated));
            }

            return true;
        }
    }
}
