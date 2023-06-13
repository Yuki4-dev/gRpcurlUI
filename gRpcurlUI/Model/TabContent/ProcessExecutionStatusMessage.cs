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
        Done,
        Cancel,
        Error,
    }
}
