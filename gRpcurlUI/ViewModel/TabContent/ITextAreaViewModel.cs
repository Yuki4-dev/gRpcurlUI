using CommunityToolkit.Mvvm.Messaging;
using gRpcurlUI.Model.TabContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gRpcurlUI.ViewModel.TabContent
{
    public interface ITextAreaViewModel: IRecipient<ClearTextBoxMessage>
    {
        public void TextBoxClear();

        public string GetText();
    }
}
