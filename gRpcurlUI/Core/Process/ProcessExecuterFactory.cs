using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace gRpcurlUI.Core.Process
{
    public class ProcessExecuterFactory
    {
        public IProcessExecuter Create()
        {
            return new InternalProcessExecuter();
        }

        private class InternalProcessExecuter : IProcessExecuter
        {
            public Encoding Encoding { get; set; } = Encoding.UTF8;

            public event Action<string>? StandardOutputReceive;

            public event Action<string>? StandardErrorReceive;

            public async Task ExecuteAsync(IProcessCommand command, CancellationToken token)
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

                var process = new System.Diagnostics.Process
                {
                    StartInfo = info,
                    EnableRaisingEvents = true
                };

                process.OutputDataReceived += (s, e) => StandardOutputReceive?.Invoke(e.Data ?? string.Empty);
                process.ErrorDataReceived += (s, e) => StandardErrorReceive?.Invoke(e.Data ?? string.Empty);

                var pTask = Task.Run(() =>
                {
                    try
                    {
                        StandardOutputReceive?.Invoke(command.AppPath + " " + command.Arguments);
                        _ = process.Start();
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
                        _ = Thread.Yield();
                    }

                    if (!pTask.IsCompleted)
                    {
                        try
                        {
                            StandardOutputReceive?.Invoke("Cancel.");
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
}
