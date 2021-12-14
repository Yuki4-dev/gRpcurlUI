using System.Windows;
using System.Windows.Controls;

namespace gRpcurlUI.Controls
{
    /// <summary>
    /// OperationInputArea.xaml の相互作用ロジック
    /// </summary>
    public partial class OperationInputArea : UserControl
    {
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public OperationInputArea()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TitleProperty =
          DependencyProperty.Register(nameof(Title), typeof(string), typeof(OperationInputArea), new PropertyMetadata(""));
    }
}
