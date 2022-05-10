namespace gRpcurlUI.Core.Reader
{
    public interface IObjectTextReader
    {
        bool IsReading { get; }

        bool IsComplete { get; }

        bool ReadLine(string line, out string message);

        object? GetObject(bool clear);
    }
}
