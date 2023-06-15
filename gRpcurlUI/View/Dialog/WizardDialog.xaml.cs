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

        private readonly List<Page> pageCashes = new();

        private WizardDialog(IWizardDialogViewModel[] wizardDialogViewModels)
        {
            this.wizardDialogViewModels = wizardDialogViewModels;
            InitializeComponent();

            Closed += WizardDialog_Closed;
            Navigate(wizardDialogViewModels[currentIndex]);
        }

        private void WizardDialog_Closed(object? sender, EventArgs e)
        {
            foreach (var viewModel in wizardDialogViewModels)
            {
                viewModel.Close();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentIndex < 1)
            {
                throw new InvalidOperationException(nameof(currentIndex));
            }

            var currentViewModel = wizardDialogViewModels[currentIndex];
            if (currentViewModel.CanBack())
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
            var currentViewModel = wizardDialogViewModels[currentIndex];
            if (currentIndex < wizardDialogViewModels.Length - 1)
            {
                if (currentViewModel.CanNext())
                {
                    Navigate(wizardDialogViewModels[++currentIndex]);
                    BackButton.IsEnabled = true;
                    if (currentIndex == wizardDialogViewModels.Length - 1)
                    {
                        NextButton.Content = "Success";
                    }
                }
            }
            else
            {
                currentViewModel.Success();
                Close();
            }
        }

        private void Navigate(IWizardDialogViewModel viewModel)
        {
            var pageType = viewModel.PageType;
            var cashed = pageCashes.Find(p => p.GetType() == pageType);
            if (cashed != null)
            {
                _ = MainFrame.Navigate(cashed);
            }
            else
            {
                var page = (Page?)Activator.CreateInstance(pageType) ?? throw new InvalidOperationException(nameof(viewModel));
                page.DataContext = viewModel;
                pageCashes.Add(page);
                _ = MainFrame.Navigate(page);
            }

            viewModel.Navigate();
            if (MainFrame.CanGoBack)
            {
                _ = MainFrame.RemoveBackEntry();
            }
        }

        public static void ShowWizard(IWizardDialogViewModel[] wizardDialogViewModels)
        {
            var wizard = new WizardDialog(wizardDialogViewModels)
            {
                Width = 700,
                Height = 700
            };
            _ = wizard.ShowDialog();
        }
    }
}
