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
        public MainWindow()
        {
            DataContext = DI.Get<MainWindowViewModel>();
            InitializeComponent();

            DI.Get<IWindowService>().SetBaseWindow(this);
        }
    }
}
