using gRpcurlUI.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace gRpcurlUI.View
{
    /// <summary>
    /// SettingPage.xaml の相互作用ロジック
    /// </summary>
    public partial class SettingPage : Page
    {
        private readonly IWindowOwner _Owner;

        public SettingPage()
        {
            InitializeComponent();
        }

        public SettingPage(IWindowOwner owner) : this()
        {
            _Owner = owner;
        }

        private void Page_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is ViewModelBase vm)
            {
                _Owner?.SetViewModel(vm);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}
