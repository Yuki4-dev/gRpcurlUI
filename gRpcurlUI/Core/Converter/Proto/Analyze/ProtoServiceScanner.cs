using gRpcurlUI.Core.Converter.Proto.Model;
using System;
using System.Collections.Generic;

namespace gRpcurlUI.Core.Converter.Proto.Analyze
{
    public class ProtoServiceScanner
    {
        private const string SERVICE = "service";

        private const string RPC = "rpc";

        private string serviceName = string.Empty;

        private readonly IList<ProtoServiceMethodInfomation> protoServiceMethods = new List<ProtoServiceMethodInfomation>();

        private readonly ProtoMessageInfomation[] protoMessageInfomations;

        public ProtoServiceScanner(ProtoMessageInfomation[] protoMessageInfomations)
        {
            this.protoMessageInfomations = protoMessageInfomations;
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
                if (messageType == SERVICE)
                {
                    serviceName = messageName;
                }
                else if (messageType == RPC)
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

        public ProtoServiceInfomation GetProtoServiceInfomation()
        {
            return new ProtoServiceInfomation(serviceName, protoServiceMethods);
        }

        private bool AddMember(string[] splitLine)
        {
            if (splitLine.Length < 5)
            {
                return false;
            }

            var requestName = GetMessageName(splitLine[2]);
            var requestModule = Array.Find(protoMessageInfomations, m => m.MessageName == requestName);
            if (requestModule is null)
            {
                return false;
            }

            var responseName = GetMessageName(splitLine[4]);
            var responseModule = Array.Find(protoMessageInfomations, m => m.MessageName == responseName);
            if (responseModule is null)
            {
                return false;
            }

            var methodName = splitLine[1];
            protoServiceMethods.Add(new ProtoServiceMethodInfomation(methodName, requestModule, responseModule));
            return true;
        }

        private string GetMessageName(string message)
        {
            return message[1..^1];
        }
    }
}
