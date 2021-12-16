using gRpcurlUI.View;
using gRpcurlUI.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace gRpcurlUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SettingPage settingPage;

        private readonly ExecutePage excutePage;

        private readonly WindowOwner _Owner;

        public MainWindow()
        {
            InitializeComponent();
            _Owner = new WindowOwner(this);
            excutePage = new ExecutePage(_Owner);
            settingPage = new SettingPage(_Owner);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is TabContentPageViewModel vm)
            {
                var bindExecute = new Binding(nameof(vm.ExecutePageViewModel))
                {
                    Source = vm,
                    Mode = BindingMode.OneWay
                };
                excutePage.SetBinding(Page.DataContextProperty, bindExecute);

                var bindSetting = new Binding(nameof(vm.SettingPageViewModel))
                {
                    Source = vm,
                    Mode = BindingMode.OneWay
                };
                settingPage.SetBinding(Page.DataContextProperty, bindSetting);

                _Owner.SetViewModel(vm);
            }

            MainFrame.Navigate(excutePage);
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(settingPage);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                return;
            }

            LeftColumn.Width = new GridLength(Width * 0.2);
            _Owner.OnWindowSizeChenged(Height, Width);
        }

    }
}
