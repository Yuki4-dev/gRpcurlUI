﻿using gRpcurlUI.ViewModel;
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

        public ExecutePage()
        {
            InitializeComponent();
        }

        public ExecutePage(IWindowOwner owner) : this()
        {
            windowOwner = owner;
            windowOwner.WindowSizeChenged += Owner_WindowSizeChenged;
        }

        private void Owner_WindowSizeChenged(double height, double width)
        {
            LeftColumn.Width = new GridLength(width * 0.25);
            RightRow.Height = new GridLength(height * 0.6);
        }

        private void ExecutePage_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is ViewModelBase vm)
            {
                windowOwner?.SetViewModel(vm);
            }
        }
    }
}