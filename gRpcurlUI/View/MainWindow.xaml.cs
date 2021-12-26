﻿using gRpcurlUI.Model;
using gRpcurlUI.Model.Curl;
using gRpcurlUI.Model.Grpcurl;
using gRpcurlUI.View;
using gRpcurlUI.ViewModel;
using System.Windows;

namespace gRpcurlUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SettingPage settingPage;

        private readonly TabContentPage curlPage;

        private readonly TabContentPage gRpcCurlPage;

        private readonly AppSetting appSetting;

        public MainWindow()
        {
            appSetting = new AppSetting(App.Current.Resources);
            WindowOwner.Current = new WindowOwner(this);
            settingPage = new SettingPage(WindowOwner.Current);
            settingPage.DataContext = new SettingPageViewModel(appSetting);
            curlPage = new TabContentPage(WindowOwner.Current);
            gRpcCurlPage = new TabContentPage(WindowOwner.Current);

            var curl = new CurlProjectContext();
            curl.SetSetting(appSetting);
            var curlViewModel = new TabContentPageViewModel();
            curlViewModel.ProjectContext = curl;
            curlPage.DataContext = curlViewModel;

            var grpcurl = new GrpcurlProjectContext();
            grpcurl.SetSetting(appSetting);
            var grpcurlViewModel = new TabContentPageViewModel();
            grpcurlViewModel.ProjectContext = grpcurl;
            gRpcCurlPage.DataContext = grpcurlViewModel;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = curlPage;
        }

        private void CurlButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = curlPage;
        }

        private void GrpcurlButton_Click_1(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = gRpcCurlPage;
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContentFrame.Content == settingPage && ContentFrame.CanGoBack)
            {
                ContentFrame.GoBack();
            }
            else
            {
                ContentFrame.Content = settingPage;
            }
        }
    }
}
