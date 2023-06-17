using gRpcurlUI.Core.API;
using gRpcurlUI.View.Pages;
using gRpcurlUI.ViewModel;
using System.Windows;

namespace gRpcurlUI.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INavigationWindow
    {
        private readonly IWindowService windowService;

        private readonly ProjectTabPage projectTabPage;

        private readonly SettingPage settingPage;

        public MainWindow()
        {
            windowService = DI.Get<IWindowService>();
            projectTabPage = DI.Get<ProjectTabPage>();
            settingPage = DI.Get<SettingPage>();

            DataContext = DI.Get<MainWindowViewModel>();
            InitializeComponent();

            windowService.SetBaseWindow(this);
        }

        public bool Navigate(NavigatePageType navigatePageType)
        {
            if(navigatePageType == NavigatePageType.Back)
            {
                MainFrame.GoBack();
                return true;
            }
            else if(navigatePageType == NavigatePageType.ProjectTab)
            {
                return MainFrame.Navigate(projectTabPage);
            }
            else if(navigatePageType == NavigatePageType.Setting)
            {
                return MainFrame.Navigate(settingPage);
            }

            throw new System.Exception();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _ = Navigate(NavigatePageType.ProjectTab);
        }
    }
}
