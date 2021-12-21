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
        private readonly IWindowOwner windowOwner;

        public SettingPage()
        {
            InitializeComponent();
        }

        public SettingPage(IWindowOwner owner) : this()
        {
            windowOwner = owner;
        }

        private void Page_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is ViewModelBase oldVm)
            {
                windowOwner?.RemoveViewModel(oldVm);
            }

            if (DataContext is ViewModelBase vm)
            {
                windowOwner?.AddViewModel(vm);
            }
        }

    }
}
