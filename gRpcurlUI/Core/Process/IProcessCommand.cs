namespace gRpcurlUI.Core.Process
{
    public interface IProcessCommand
    {
        string AppPath { get; }

        string Arguments { get; }
    }
}
