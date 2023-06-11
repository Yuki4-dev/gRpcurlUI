﻿using gRpcurlUI.Core.API;
using System.Windows;
using System.Windows.Controls;

namespace gRpcurlUI.View
{
    /// <summary>
    /// TabContentPage.xaml の相互作用ロジック
    /// </summary>
    public partial class TabContentPage : UserControl
    {
        private readonly IWindowService windowService;

        public TabContentPage()
        {
            windowService = DI.Get<IWindowService>();
            windowService.WindowSizeChanged += WindowService_WindowSizeChenged;

            InitializeComponent();
        }

        private void WindowService_WindowSizeChenged(double height, double width)
        {
            LeftColumn.Width = new GridLength(width * 0.2);
            MidColumn.Width = new GridLength(width * 0.25);
            RightRow.Height = new GridLength(height * 0.6);
        }
    }
}
