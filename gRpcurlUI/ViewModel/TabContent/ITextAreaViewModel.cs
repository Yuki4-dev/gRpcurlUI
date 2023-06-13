using CommunityToolkit.Mvvm.Messaging;
using gRpcurlUI.Model.TabContent;

namespace gRpcurlUI.ViewModel.TabContent
{
    public interface ITextAreaViewModel: IRecipient<ClearTextBoxMessage>
    {
        public void TextBoxClear();

        public string GetText();
    }
}
