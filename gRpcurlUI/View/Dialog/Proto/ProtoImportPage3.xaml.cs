using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace gRpcurlUI.View.Dialog.Proto
{
    /// <summary>
    /// ProtoImportPage3.xaml の相互作用ロジック
    /// </summary>
    public partial class ProtoImportPage3 : Page
    {
        public ProtoImportPage3()
        {
            InitializeComponent();
        }

        private void ListBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("s");
        }
    }
}
