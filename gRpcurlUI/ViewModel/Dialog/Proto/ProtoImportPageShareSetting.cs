using gRpcurlUI.Core.Converter.Proto.Model;

namespace gRpcurlUI.ViewModel.Dialog.Proto
{
    public class ProtoImportPageShareSetting
    {
        public string FilePath { get; set; } = string.Empty;

        public ProtoAnalyzeEntryResult? ProtoAnalyzeEntryResult { get; set; }
    }
}
