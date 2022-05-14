using gRpcurlUI.Core.Converter.Proto.Model;
using System;
using System.Collections.Generic;

namespace gRpcurlUI.Core.Converter.Proto.Format
{
    public class ProtoFormatEntry
    {
        public ProtoFormatResut Format(ProtoServiceMethodInfomation methodInfomation, ProtoMessageInfomation[] messageInfomations, ProtoFormatOption formatOption)
        {
            var requestFormat = Format(methodInfomation.Request, messageInfomations, formatOption);
            var responseFormat = Format(methodInfomation.Response, messageInfomations, formatOption);
            return new ProtoFormatResut(methodInfomation, requestFormat, responseFormat);
        }

        private IDictionary<string, object> Format(ProtoMessageInfomation messageInfomation, ProtoMessageInfomation[] messageInfomations, ProtoFormatOption formatOption)
        {
            var enumFormatter = new EnumProtoValueFormatter();
            var primitiveFormatters = new List<PrimitiveProtoValueFormatter>();

            var formatValue = new Dictionary<string, object>();
            foreach (var member in messageInfomation.MessageInfomations)
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
                    var message = Array.Find(messageInfomations, m => m.MessageName == member.TypeName);
                    if (message != null)
                    {
                        value = Format(message, messageInfomations, formatOption);
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

    public class ProtoFormatResut
    {
        public ProtoServiceMethodInfomation MethodInfomation { get; }

        public IDictionary<string, object> RequestFormat { get; }

        public IDictionary<string, object> ResponseFormat { get; }

        public ProtoFormatResut(ProtoServiceMethodInfomation methodInfomation, IDictionary<string, object> request, IDictionary<string, object> response)
        {
            MethodInfomation = methodInfomation;
            RequestFormat = request;
            ResponseFormat = response;
        }
    }

    public class ProtoFormatOption
    {
    }
}
