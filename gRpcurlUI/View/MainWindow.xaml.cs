using gRpcurlUI.Model.Curl;
using gRpcurlUI.Model.Grpcurl;
using gRpcurlUI.Model.Setting;
using gRpcurlUI.View;
using gRpcurlUI.ViewModel;
using System.Windows;

namespace gRpcurlUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SettingPage settingPage;

        private readonly TabContentPage curlPage;

        private readonly TabContentPage gRpcCurlPage;

        public MainWindow()
        {
            WindowOwner.Current = new WindowOwner(this);
            settingPage = new SettingPage(WindowOwner.Current);
            settingPage.DataContext = new SettingPageViewModel(new FontSetting(App.Current.Resources), new BrushSetting(App.Current.Resources));
            curlPage = new TabContentPage(WindowOwner.Current);
            gRpcCurlPage = new TabContentPage(WindowOwner.Current);

            var curlViewModel = new TabContentPageViewModel();
            curlViewModel.ProjectContext = new CurlProjectContext();
            curlPage.DataContext = curlViewModel;

            var grpcurlViewModel = new TabContentPageViewModel();
            grpcurlViewModel.ProjectContext = new GrpcurlProjectContext();
            gRpcCurlPage.DataContext = grpcurlViewModel;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = curlPage;
        }

        private void CurlButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = curlPage;
        }

        private void GrpcurlButton_Click_1(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = gRpcCurlPage;
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContentFrame.Content == settingPage && ContentFrame.CanGoBack)
            {
                ContentFrame.GoBack();
            }
            else
            {
                ContentFrame.Content = settingPage;
            }
        }
    }
}
