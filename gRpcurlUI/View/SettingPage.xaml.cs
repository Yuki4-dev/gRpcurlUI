using gRpcurlUI.ViewModel;
using System.Windows.Controls;

namespace gRpcurlUI.View
{
    /// <summary>
    /// SettingPage.xaml の相互作用ロジック
    /// </summary>
    public partial class SettingPage : Page
    {
        public SettingPage()
        {
            DataContext = DI.Get<SettingPageViewModel>();
            InitializeComponent();
        }
    }
}
