using CommunityToolkit.Mvvm.Messaging;
using gRpcurlUI.Model.ProjectTab;

namespace gRpcurlUI.ViewModel.Pages.ProjectTab
{
    public interface ITextAreaViewModel: IRecipient<ClearTextBoxMessage>
    {
        public void TextBoxClear();

        public string GetText();
    }
}
