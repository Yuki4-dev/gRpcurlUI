namespace gRpcurlUI.Model.TabContent
{
    public class ClearTextBoxMessage
    {
        public ClearTextBoxType ClearTextType { get; }

        public ClearTextBoxMessage(ClearTextBoxType clearTextType)
        {
            ClearTextType = clearTextType;
        }
    }

    public enum ClearTextBoxType
    {
        Request,
        Response,
        Error,
    }
}
