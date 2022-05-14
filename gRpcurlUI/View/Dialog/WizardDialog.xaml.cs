﻿using gRpcurlUI.Service;
using gRpcurlUI.ViewModel.Dialog;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace gRpcurlUI.View.Dialog
{
    /// <summary>
    /// WizardDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class WizardDialog : Window
    {
        private int currentIndex = 0;

        private readonly IWizardDialogViewModel[] wizardDialogViewModels;

        private readonly List<Page> pageChashes = new List<Page>();

        private WizardDialog(IWizardDialogViewModel[] wizardDialogViewModels)
        {
            this.wizardDialogViewModels = wizardDialogViewModels;

            BorderBrush = DI.Get<IWindowService>().AccentBrush;
            InitializeComponent();

            Navigate(wizardDialogViewModels[currentIndex]);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var viewModel in wizardDialogViewModels)
            {
                viewModel.Close();
            }
            Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentIndex < 1)
            {
                throw new InvalidOperationException(nameof(currentIndex));
            }

            var cuurentViewModel = wizardDialogViewModels[currentIndex];
            if (cuurentViewModel.CanBack())
            {
                currentIndex--;
                if (currentIndex == 0)
                {
                    BackButton.IsEnabled = false;
                }

                Navigate(wizardDialogViewModels[currentIndex]);
                NextButton.Content = "Next";
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            var cuurentViewModel = wizardDialogViewModels[currentIndex];
            if (currentIndex < wizardDialogViewModels.Length - 1)
            {
                if (cuurentViewModel.CanNext())
                {
                    currentIndex++;
                    Navigate(wizardDialogViewModels[currentIndex]);
                    BackButton.IsEnabled = true;
                    if (currentIndex == wizardDialogViewModels.Length - 1)
                    {
                        NextButton.Content = "Success";
                    }
                }
            }
            else
            {
                cuurentViewModel.Success();
                Close();
            }
        }

        private void Navigate(IWizardDialogViewModel viewModel)
        {
            var pageType = viewModel.PageType;
            var cashe = pageChashes.Find(p => p.GetType() == pageType);
            if (cashe != null)
            {
                MainFrame.Navigate(cashe);
            }
            else
            {
                var page = (Page?)Activator.CreateInstance(pageType) ?? throw new InvalidOperationException(nameof(viewModel));
                page.DataContext = viewModel;
                pageChashes.Add(page);
                MainFrame.Navigate(page);
            }

            viewModel.Navigate();
            if (MainFrame.CanGoBack)
            {
                MainFrame.RemoveBackEntry();
            }
        }

        public static void ShowWizard(IWizardDialogViewModel[] wizardDialogViewModels)
        {
            var wizard = new WizardDialog(wizardDialogViewModels)
            {
                Width = 700,
                Height = 700
            };
            wizard.ShowDialog();
        }
    }
}