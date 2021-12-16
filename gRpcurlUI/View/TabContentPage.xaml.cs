using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace gRpcurlUI.View
{
    /// <summary>
    /// TabContentPage.xaml の相互作用ロジック
    /// </summary>
    public partial class TabContentPage : Page
    {
        public TabContentPage()
        {
            InitializeComponent();
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            //MainFrame.Navigate(settingPage);
        }
    }
}
