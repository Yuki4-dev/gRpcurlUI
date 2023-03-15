using CommunityToolkit.Mvvm.ComponentModel;
using gRpcurlUI.Core.Converter.Proto.Analyze;
using gRpcurlUI.View.Proto;
using gRpcurlUI.ViewModel.Dialog;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace gRpcurlUI.ViewModel.Proto
{
    [ObservableObject]
    public partial class ProtoImportPage2ViewModel : IWizardDialogViewModel
    {
        public Type PageType => typeof(ProtoImportPage2);

        private bool _IsAnalyze = false;
        public bool IsAnalyze
        {
            get => _IsAnalyze;
            set
            {
                if (SetProperty(ref _IsAnalyze, value))
                {
                    OnPropertyChanged(nameof(ProgressVisible));
                }
            }
        }
        public Visibility ProgressVisible => IsAnalyze ? Visibility.Visible : Visibility.Hidden;

        [ObservableProperty]
        private string protoInformation = string.Empty;

        [ObservableProperty]
        private string protoErrorMessage = string.Empty;

        private readonly bool canBack = true;

        private bool canNext = true;

        private readonly ProtoImportPageShareSetting protoImportPageShareSetting;

        private readonly ProtoAnalyzeEntry protoAnalyzeEntry;

        public ProtoImportPage2ViewModel(ProtoImportPageShareSetting protoImportPageShareSetting, ProtoAnalyzeEntry protoAnalyzeEntry)
        {
            this.protoImportPageShareSetting = protoImportPageShareSetting;
            this.protoAnalyzeEntry = protoAnalyzeEntry;
        }

        public bool CanBack()
        {
            return canBack;
        }

        public bool CanNext()
        {
            return canNext;
        }

        public bool CanSuccess()
        {
            throw new NotImplementedException();
        }

        public void Success()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            //
        }

        public async void Navigate()
        {
            IsAnalyze = true;
            try
            {
                var text = await File.ReadAllTextAsync(protoImportPageShareSetting.FilePath);
                protoImportPageShareSetting.ProtoAnalyzeEntryResult = await protoAnalyzeEntry.AnalyzeAllLineAsync(GetTextAllLine(text));
                SetProtoInformation();
                canNext = true;
            }
            catch (Exception ex)
            {
                ProtoErrorMessage = $"Analyze Failed.{Environment.NewLine}{ex.Message}";
                canNext = false;
            }
            finally
            {
                IsAnalyze = false;
            }
        }

        private void SetProtoInformation()
        {
            if (protoImportPageShareSetting.ProtoAnalyzeEntryResult != null)
            {
                var analyzeResult = protoImportPageShareSetting.ProtoAnalyzeEntryResult;

                var sb = new StringBuilder();
                _ = sb.AppendLine($"Package Name :  {analyzeResult.ProtoNameInformation.PackageNames[0]} .");
                _ = sb.AppendLine($"Service Name : {analyzeResult.ProtoNameInformation.ServiceNames[0]} .");

                var indent = "    ";
                var service = analyzeResult.ProtoServiceInformation;
                _ = sb.AppendLine($"Found {service.ProtoServiceMethods.Count()} Method.");
                foreach (var method in service.ProtoServiceMethods)
                {
                    _ = sb.AppendLine($"{indent}- {method.MethodName} ( Request : {method.Request.MessageName} / Response : {method.Response.MessageName} )");
                }

                ProtoInformation = sb.ToString();

                var err = protoImportPageShareSetting.ProtoAnalyzeEntryResult.ErrorInformation;
                ProtoErrorMessage = string.Join(Environment.NewLine, err.Select(e => $"{e.Message} ( Line : {e.Line} )"));
            }
        }

        private string[] GetTextAllLine(string text)
        {
            var replaced = text.Replace("\r\n", Environment.NewLine)
                .Replace("\n", Environment.NewLine);

            return replaced.Split(Environment.NewLine);
        }
    }
}
