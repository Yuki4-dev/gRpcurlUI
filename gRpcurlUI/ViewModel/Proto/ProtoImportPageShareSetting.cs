using gRpcurlUI.Core.Converter.Proto.Analyze;

namespace gRpcurlUI.ViewModel.Proto
{
    public class ProtoImportPageShareSetting
    {
        public string FilePath { get; set; } = string.Empty;

        public ProtoAnalyzeEntryResult? ProtoAnalyzeEntryResult { get; set; }
    }
}
