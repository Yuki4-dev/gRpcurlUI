using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace gRpcurlUI.Core.Procces
{
    public class ProcessExecuter : IProcessExecuter
    {
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        public event Action<string>? StanderdOutputRecieve;

        public event Action<string>? StanderdErrorRecieve;

        public async Task ExecuteAysnc(IProccesCommand command, CancellationToken token)
        {
            var info = new ProcessStartInfo(command.AppPath, command.Arguments)
            {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                StandardErrorEncoding = Encoding,
                StandardOutputEncoding = Encoding
            };

            var process = new Process
            {
                StartInfo = info,
                EnableRaisingEvents = true
            };

            process.OutputDataReceived += (s, e) => StanderdOutputRecieve?.Invoke(e.Data ?? string.Empty);
            process.ErrorDataReceived += (s, e) => StanderdErrorRecieve?.Invoke(e.Data ?? string.Empty);

            var pTask = Task.Run(() =>
            {
                try
                {
                    StanderdOutputRecieve?.Invoke(command.AppPath + " " + command.Arguments);
                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    process.WaitForExit();
                }
                finally
                {
                    process.Close();
                }
            });

            await Task.Run(() =>
            {
                while (!token.IsCancellationRequested && !pTask.IsCompleted)
                {
                    Thread.Sleep(1);
                    Thread.Sleep(0);
                    Thread.Yield();
                }

                if (!pTask.IsCompleted)
                {
                    try
                    {
                        StanderdOutputRecieve?.Invoke("Cancel.");
                        process.CancelOutputRead();
                        process.CancelErrorRead();
                        process.Kill();
                    }
                    finally
                    {
                        process.Close();
                    }
                }
            });

        }
    }
}
