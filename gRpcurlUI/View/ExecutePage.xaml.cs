using gRpcurlUI.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace gRpcurlUI.View
{
    /// <summary>
    /// GrpcurlExecute.xaml の相互作用ロジック
    /// </summary>
    public partial class ExecutePage : Page
    {
        private readonly IWindowOwner windowOwner;

        public ExecutePage() : this(WindowOwner.Current) { }

        public ExecutePage(IWindowOwner owner)
        {
            windowOwner = owner;
            if (windowOwner != null)
            {
                windowOwner.WindowSizeChenged += Owner_WindowSizeChenged;
            }
            InitializeComponent();
        }

        private void Owner_WindowSizeChenged(double height, double width)
        {
            LeftColumn.Width = new GridLength(width * 0.25);
            RightRow.Height = new GridLength(height * 0.6);
        }

        private void ExecutePage_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
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
