using gRpcurlUI.Core.Procces;
using gRpcurlUI.Model.Curl;
using gRpcurlUI.Model.Grpcurl;
using gRpcurlUI.Service;
using gRpcurlUI.ViewModel;
using System.Windows;

namespace gRpcurlUI.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IWindowService windowService;

        private readonly SettingPage settingPage = new();

        private readonly TabContentPage curlPage = new();

        private readonly TabContentPage gRpcCurlPage = new();

        public MainWindow()
        {
            windowService = DI.Get<IWindowService>();
            windowService.SetBaseWindow(this);

            var pe = DI.Get<IProcessExecuter>();
            var pd = DI.Get<IProjectDataService>();

            var curlViewModel = new TabContentPageViewModel(windowService, pe, pd)
            {
                ProjectContext = new CurlProjectContext()
            };
            curlPage.DataContext = curlViewModel;

            var grpcurlViewModel = new TabContentPageViewModel(windowService, pe, pd)
            {
                ProjectContext = new GrpcurlProjectContext()
            };
            gRpcCurlPage.DataContext = grpcurlViewModel;

            InitializeComponent();

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
