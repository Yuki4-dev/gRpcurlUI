using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gRpcurlUI.Model.TabContent
{
    public class ProcessExecutionStatusMessage
    {
        public ProcessExecutionStatus ExecutionStatus { get; }

        public ProcessExecutionStatusMessage(ProcessExecutionStatus executionStatus)
        {
            ExecutionStatus = executionStatus;
        }
    }

    public enum ProcessExecutionStatus
    {
        Init,
        PreProcess,
        PostProcess,
        Error,
    }
}
