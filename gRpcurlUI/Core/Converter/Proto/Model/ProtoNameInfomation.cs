namespace gRpcurlUI.Core.Converter.Proto.Model
{
    public class ProtoNameInfomatin
    {
        public string[] PackageNames { get; set; }

        public string[] MessageNames { get; }

        public string[] EnumNames { get; }

        public string[] MethodNames { get; }

        public string[] ServiceNames { get; }

        public ProtoNameInfomatin(string[] packageNames, string[] messageNames, string[] enumNames, string[] methodNames, string[] serviceNames)
        {
            PackageNames = packageNames;
            MessageNames = messageNames;
            EnumNames = enumNames;
            MethodNames = methodNames;
            ServiceNames = serviceNames;
        }
    }
}
