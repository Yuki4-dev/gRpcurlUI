using gRpcurlUI.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace gRpcurlUI.View
{
    /// <summary>
    /// TabContentPage.xaml の相互作用ロジック
    /// </summary>
    public partial class TabContentPage : Page
    {
        private readonly ExecutePage excutePage;

        private readonly IWindowOwner windowOwner;

        public TabContentPage()
        {
            excutePage = new ExecutePage();
            InitializeComponent();
        }

        public TabContentPage(IWindowOwner owner)
        {
            windowOwner = owner;
            windowOwner.WindowSizeChenged += Owner_WindowSizeChenged;
            excutePage = new ExecutePage(windowOwner);
            InitializeComponent();
        }

        private void Owner_WindowSizeChenged(double height, double width)
        {
            LeftColumn.Width = new GridLength(width * 0.2);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(excutePage);
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

            if (DataContext is TabContentPageViewModel tabPageVm)
            {
                excutePage.DataContext = tabPageVm.ExecutePageViewModel;
            }
        }
    }
}
