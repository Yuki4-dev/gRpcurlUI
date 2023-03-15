using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace gRpcurlUI.Core.Process
{
    public interface IProcessExecuter
    {
        Encoding Encoding { get; set; }

        event Action<string>? StandardOutputReceive;

        event Action<string>? StandardErrorReceive;

        Task ExecuteAsync(IProcessCommand command, CancellationToken token);
    }
}
