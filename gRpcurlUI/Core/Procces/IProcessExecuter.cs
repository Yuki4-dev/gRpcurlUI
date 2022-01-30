using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace gRpcurlUI.Core.Procces
{
    public interface IProcessExecuter
    {
        Encoding Encoding { get; set; }

        event Action<string> StanderdOutputRecieve;

        event Action<string> StanderdErrorRecieve;

        Task ExecuteAysnc(IProccesCommand command, CancellationToken token);
    }
}
