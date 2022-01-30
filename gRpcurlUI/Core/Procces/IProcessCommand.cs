namespace gRpcurlUI.Core.Procces
{
    public interface IProccesCommand
    {
        string AppPath { get; }

        string Arguments { get; }
    }
}
