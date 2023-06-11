using gRpcurlUI.ViewModel;
using gRpcurlUI.ViewModel.Setting;
using System.Windows.Controls;

namespace gRpcurlUI.View
{
    /// <summary>
    /// SettingPage.xaml の相互作用ロジック
    /// </summary>
    public partial class SettingPage : UserControl
    {
        public SettingPage()
        {
            DataContext = DI.Get<SettingPageViewModel>();
            InitializeComponent();
        }
    }
}
