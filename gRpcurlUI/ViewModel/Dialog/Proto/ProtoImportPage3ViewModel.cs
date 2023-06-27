using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using gRpcurlUI.Core.Converter.Proto.Format;
using gRpcurlUI.Core.Model;
using gRpcurlUI.Model.Grpcurl;
using gRpcurlUI.View.Dialog.Proto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static gRpcurlUI.Language;

namespace gRpcurlUI.ViewModel.Dialog.Proto
{
    [ObservableObject]
    public partial class ProtoImportPage3ViewModel : IWizardDialogViewModel
    {
        public Type PageType => typeof(ProtoImportPage3);

        public ProtoDialogLanguage Texts => Language.Default.ProtoDialog;

        private readonly ICollection<GrpcurlProject> projects = new ObservableCollection<GrpcurlProject>();
        public IEnumerable<IProject> Projects => projects;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        [ObservableProperty]
        private IProject? selectedProject = null;

        private readonly ProtoImportPageShareSetting protoImportPageShareSetting;

        private readonly ProtoFormatEntry protoFormatEntry;

        private bool canSuccess = false;

        public ProtoImportPage3ViewModel(ProtoImportPageShareSetting protoImportPageShareSetting, ProtoFormatEntry protoFormatEntry)
        {
            this.protoImportPageShareSetting = protoImportPageShareSetting;
            this.protoFormatEntry = protoFormatEntry;
        }

        public bool CanBack()
        {
            return true;
        }

        public bool CanNext()
        {
            return false;
        }

        public bool CanSuccess()
        {
            return canSuccess;
        }

        public void Success()
        {
            foreach (var p in projects)
            {
                p.IsSelected = false;
                p.IsReadProtoButtonEnable = true;
            }

            _ = WeakReferenceMessenger.Default.Send(new AddGrpcProjectMessage(projects.ToArray()));
        }

        public void Close()
        {
            //
        }

        public void Navigate()
        {
            if (protoImportPageShareSetting.ProtoAnalyzeEntryResult is null)
            {
                throw new InvalidOperationException("ProtoAnalyzeEntryResult is null.");
            }

            projects.Clear();
            try
            {
                AddProjects();
                canSuccess = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"AppProject Failed.{Environment.NewLine}{ex.Message}";
                canSuccess = false;
            }
        }

        private void AddProjects()
        {
            var analyzeResult = protoImportPageShareSetting.ProtoAnalyzeEntryResult;
            var packageName = analyzeResult.ProtoNameInformation.PackageNames[0];
            var serviceName = analyzeResult.ProtoNameInformation.ServiceNames[0];
            foreach (var protoModuleInfo in analyzeResult.ProtoServiceInformation.ProtoServiceMethods)
            {
                var formatResult = protoFormatEntry.Format(protoModuleInfo, analyzeResult.ProtoMessageInformation.ToArray(), new ProtoFormatOption());
                var project = new GrpcurlProject
                {
                    ProjectName = formatResult.MethodInformation.MethodName,
                    Service = packageName + "." + serviceName + "/" + formatResult.MethodInformation.MethodName,
                    SendContent = ToJson(formatResult.RequestFormat),
                    IsReadProtoButtonEnable = false
                };

                projects.Add(project);
            }
        }

        private string ToJson(object value)
        {
            try
            {
                return JsonConvert.SerializeObject(value, Formatting.Indented);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
