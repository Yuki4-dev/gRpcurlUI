using gRpcurlUI.ViewModel;
using gRpcurlUI.ViewModel.Pages;
using System.Windows;
using System.Windows.Controls;

namespace gRpcurlUI.View.Pages
{
    /// <summary>
    /// ProjectTabPage.xaml の相互作用ロジック
    /// </summary>
    public partial class ProjectTabPage : Page
    {
        public ProjectTabPage(ProjectTabPageViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
