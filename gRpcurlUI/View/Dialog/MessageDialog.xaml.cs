using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace gRpcurlUI.View.Dialog
{
    /// <summary>
    /// MessageDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class MessageDialog : Window
    {

        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(nameof(Message), typeof(string), typeof(MessageDialog), new PropertyMetadata(""));

        public MessageDialog()
        {
            InitializeComponent();
        }

        private MessageBoxResult result = MessageBoxResult.None;

        private void SetButton(MessageBoxButton button)
        {
            Action<Button, string, MessageBoxResult> setB = (button, content, result) =>
             {
                 button.Visibility = Visibility.Visible;
                 button.Content = content;
                 button.Tag = result;
             };

            if (button == MessageBoxButton.OK)
            {
                setB(FirstButton, "OK", MessageBoxResult.OK);
            }
            else if (button == MessageBoxButton.OKCancel)
            {
                setB(FirstButton, "OK", MessageBoxResult.OK);
                setB(SecandButton, "Cancel", MessageBoxResult.Cancel);
            }
            else if (button == MessageBoxButton.YesNo)
            {
                setB(FirstButton, "YES", MessageBoxResult.Yes);
                setB(SecandButton, "No", MessageBoxResult.No);
            }
            else if (button == MessageBoxButton.YesNoCancel)
            {
                setB(FirstButton, "YES", MessageBoxResult.Yes);
                setB(SecandButton, "No", MessageBoxResult.No);
                setB(ThirdButton, "Cancel", MessageBoxResult.Cancel);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            result = (MessageBoxResult)((Button)sender).Tag;
            Close();
        }

        public static MessageBoxResult Show(string Title, string Message, MessageBoxButton button = MessageBoxButton.OK)
        {
            var dialog = new MessageDialog()
            {
                Title = Title,
                Message = Message,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            dialog.SetButton(button);
            dialog.ShowDialog();
            return dialog.result;
        }

        private void Root_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C &&
               (Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) == KeyStates.Down
               || (Keyboard.GetKeyStates(Key.RightCtrl) & KeyStates.Down) == KeyStates.Down)
            {
                Clipboard.SetText(Message);
            }
        }
    }
}
