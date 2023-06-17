namespace gRpcurlUI.Core.Process
{
    public interface IProcessCommand
    {
        string AppName { get; }

        string Arguments { get; }
    }
}
