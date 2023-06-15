using gRpcurlUI.ViewModel.Pages;
using System.Windows.Controls;

namespace gRpcurlUI.View.Pages
{
    /// <summary>
    /// SettingPage.xaml の相互作用ロジック
    /// </summary>
    public partial class SettingPage : Page
    {
        public SettingPage(SettingPageViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
