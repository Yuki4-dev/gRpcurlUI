using gRpcurlUI.Core.Converter.Proto.Model;
using System;
using System.Collections.Generic;

namespace gRpcurlUI.Core.Converter.Proto.Format
{
    public class ProtoFormatEntry
    {
        public ProtoFormatResult Format(ProtoServiceMethodInformation methodInformation, ProtoMessageInformation[] messageInformation, ProtoFormatOption formatOption)
        {
            var requestFormat = Format(methodInformation.Request, messageInformation, formatOption);
            var responseFormat = Format(methodInformation.Response, messageInformation, formatOption);
            return new ProtoFormatResult(methodInformation, requestFormat, responseFormat);
        }

        private IDictionary<string, object> Format(ProtoMessageInformation messageInformation, ProtoMessageInformation[] messageInformations, ProtoFormatOption formatOption)
        {
            var enumFormatter = new EnumProtoValueFormatter();
            var primitiveFormatters = new List<PrimitiveProtoValueFormatter>();

            var formatValue = new Dictionary<string, object>();
            foreach (var member in messageInformation.MemberInformation)
            {

                object value;
                if (member.ModuleType == ProtoModuleType.TypeEnum)
                {
                    value = enumFormatter.GetValueFormat(member);
                }
                else if (member.ModuleType == ProtoModuleType.TypePrimitive)
                {
                    IProtoTypedValueFormatter? format = primitiveFormatters.Find(f => f.IsTarget(member));
                    if (format != null)
                    {
                        value = format.GetValueFormat(member);
                    }
                    else
                    {
                        value = UnKnownProtoValueFormatter.Default.GetValueFormat(member);
                    }
                }
                else if (member.ModuleType == ProtoModuleType.TypeMessage)
                {
                    var message = Array.Find(messageInformations, m => m.MessageName == member.TypeName);
                    if (message != null)
                    {
                        value = Format(message, messageInformations, formatOption);
                    }
                    else
                    {
                        value = UnKnownProtoValueFormatter.Default.GetValueFormat(member);
                    }
                }
                else
                {
                    value = UnKnownProtoValueFormatter.Default.GetValueFormat(member);
                }

                if (member.IsRepeated)
                {
                    formatValue.Add(member.PropertyName, new object[] { value });
                }
                else
                {
                    formatValue.Add(member.PropertyName, value);
                }
            }

            return formatValue;
        }
    }

    public class ProtoFormatResult
    {
        public ProtoServiceMethodInformation MethodInformation { get; }

        public IDictionary<string, object> RequestFormat { get; }

        public IDictionary<string, object> ResponseFormat { get; }

        public ProtoFormatResult(ProtoServiceMethodInformation methodInformation, IDictionary<string, object> request, IDictionary<string, object> response)
        {
            MethodInformation = methodInformation;
            RequestFormat = request;
            ResponseFormat = response;
        }
    }

    public class ProtoFormatOption
    {
    }
}
