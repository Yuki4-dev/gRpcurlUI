using System.Windows;
using System.Windows.Controls;

namespace gRpcurlUI.Controls
{
    public partial class HeaderContent : UserControl
    {
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public FrameworkElement Header
        {
            get => (FrameworkElement)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public HeaderContent()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(HeaderContent), new PropertyMetadata(""));

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(FrameworkElement), typeof(HeaderContent), new PropertyMetadata());
    }
}
