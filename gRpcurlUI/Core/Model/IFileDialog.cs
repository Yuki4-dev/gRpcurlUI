namespace gRpcurlUI.Core.Model
{
    public interface IFileDialog
    {
        string Title { get; set; }

        string Filter { get; set; }

        string FileName { get; set; }
    }
}
