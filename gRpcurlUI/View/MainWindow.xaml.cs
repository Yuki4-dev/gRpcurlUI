using gRpcurlUI.Core;
using gRpcurlUI.Model;
using gRpcurlUI.View;
using gRpcurlUI.ViewModel;
using gRpcurlUI.ViewModel.Curl;
using gRpcurlUI.ViewModel.Grpcurl;
using System.Windows;

namespace gRpcurlUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly WindowOwner windowOwner;

        private readonly SettingPage settingPage;

        private readonly TabContentPage curlPage;

        private readonly TabContentPage gRpcCurlPage;

        private readonly AppSetting appSetting;

        private readonly ILoadModel loadModel = new LoadModel();

        public MainWindow()
        {
            appSetting = new AppSetting(App.Current.Resources);
            windowOwner = new WindowOwner(this);
            settingPage = new SettingPage(windowOwner);
            settingPage.DataContext = new SettingPageViewModel(appSetting);
            curlPage = new TabContentPage(windowOwner);
            gRpcCurlPage = new TabContentPage(windowOwner);
            curlPage.DataContext = new TabContentPageViewModel(loadModel, new CurlExecutePageViewModel(new ProcessExecuter()));
            gRpcCurlPage.DataContext = new TabContentPageViewModel(loadModel, new GrpcurlExecutePageViewModel(new ProcessExecuter(), appSetting));

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
