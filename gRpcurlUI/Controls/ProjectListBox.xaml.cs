using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace gRpcurlUI.Controls
{
    /// <summary>
    /// ProjectListBox.xaml の相互作用ロジック
    /// </summary>
    public partial class ProjectListBox : UserControl
    {
        public ICommand SelectedCommand
        {
            get => (ICommand)GetValue(SelectedCommandProperty);
            set => SetValue(SelectedCommandProperty, value);
        }

        public object ItemsSource
        {
            get => GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public bool IsMultiSelect
        {
            get => (bool)GetValue(IsMultiSelectProperty);
            set => SetValue(IsMultiSelectProperty, value);

        }

        public bool IsAllSelected
        {
            get => (bool)GetValue(IsAllSelectedProperty);
            set => SetValue(IsAllSelectedProperty, value);

        }

        public ProjectListBox()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(object), typeof(ProjectListBox), new PropertyMetadata());

        public static readonly DependencyProperty SelectedCommandProperty =
            DependencyProperty.Register(nameof(SelectedCommand), typeof(ICommand), typeof(ProjectListBox), new PropertyMetadata());

        public static readonly DependencyProperty IsMultiSelectProperty =
            DependencyProperty.Register(nameof(IsMultiSelect), typeof(bool), typeof(ProjectListBox), new PropertyMetadata(false));

        public static readonly DependencyProperty IsAllSelectedProperty =
           DependencyProperty.Register(nameof(IsAllSelected), typeof(bool), typeof(ProjectListBox), new PropertyMetadata(false));

        private void CheckToggleButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SelectRudioButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
