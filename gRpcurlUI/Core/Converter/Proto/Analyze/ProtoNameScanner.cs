using gRpcurlUI.Core.Converter.Proto.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace gRpcurlUI.Core.Converter.Proto.Analyze
{
    public class ProtoNameScanner
    {
        private const string ENUM = "enum";
        private const string MESSAGE = "message";
        private const string PACKAGE = "package";
        private const string SERVICE = "service";
        private const string METHOD = "rpc";

        private readonly string[] KEYS = new[] { ENUM, MESSAGE, PACKAGE, SERVICE, METHOD };

        private readonly IList<string> packageNames = new List<string>();

        private readonly IList<string> serviceNames = new List<string>();

        private readonly IList<string> messageNames = new List<string>();

        private readonly IList<string> enumNames = new List<string>();

        private readonly IList<string> methodNames = new List<string>();

        public bool ReadLine(string line, out string errorMessage)
        {
            var trimLine = line.Trim();
            if (string.IsNullOrEmpty(trimLine))
            {
                errorMessage = "空白行です。";
                return false;
            }

            var splitLine = trimLine.Split(' ');
            if (splitLine.Length < 2)
            {
                errorMessage = $"無効な行です。 : {line}";
                return false;
            }

            var messageType = splitLine[0].Trim();
            if (!KEYS.Contains(messageType))
            {
                errorMessage = $"無効な行です。 : {line}";
                return false;
            }

            var name = splitLine[1];
            if (name.EndsWith(";"))
            {
                name = name.Substring(0, name.Length - 1);
            }

            if (messageType == MESSAGE)
            {
                if (!messageNames.Contains(name))
                {
                    messageNames.Add(name);
                }
            }
            else if (messageType == ENUM)
            {
                if (!enumNames.Contains(name))
                {
                    enumNames.Add(name);
                }
            }
            else if (messageType == SERVICE)
            {
                if (!serviceNames.Contains(name))
                {
                    serviceNames.Add(name);
                }
            }
            else if (messageType == PACKAGE)
            {
                if (!packageNames.Contains(name))
                {
                    packageNames.Add(name);
                }
            }
            else if (messageType == METHOD)
            {
                if (!methodNames.Contains(name))
                {
                    methodNames.Add(name);
                }
            }

            errorMessage = string.Empty;
            return true;
        }

        public ProtoNameInformation GetProtoNameInformation()
        {
            return new ProtoNameInformation(packageNames.ToArray(), messageNames.ToArray(), enumNames.ToArray(), methodNames.ToArray(), serviceNames.ToArray());
        }
    }
}
