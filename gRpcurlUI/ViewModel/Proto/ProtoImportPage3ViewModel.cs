using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using gRpcurlUI.Core.Converter.Proto.Format;
using gRpcurlUI.Model;
using gRpcurlUI.Model.Grpcurl;
using gRpcurlUI.View.Proto;
using gRpcurlUI.ViewModel.Dialog;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace gRpcurlUI.ViewModel.Proto
{
    [ObservableObject]
    public partial class ProtoImportPage3ViewModel : IWizardDialogViewModel
    {
        public Type PageType => typeof(ProtoImportPage3);

        private readonly ICollection<GrpcurlProject> _Projects = new ObservableCollection<GrpcurlProject>();
        public IEnumerable<IProject> Projects => _Projects;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        [ObservableProperty]
        private IProject? selectedProject = null;

        private readonly ProtoImportPageShareSetting protoImportPageShareSetting;

        private readonly ProtoFormatEntry protoFormatEntry;

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
            return true;
        }

        public void Success()
        {
            foreach (var p in _Projects)
            {
                p.IsSelected = false;
                p.IsReadProtoButtonEnable = true;
            }

            WeakReferenceMessenger.Default.Send(new AddGrpcProjectMessage(_Projects));
        }

        public void Close()
        {
            //
        }

        public void Navigate()
        {
            if (protoImportPageShareSetting.ProtoAnalyzeEntryResult is null)
            {
                throw new InvalidOperationException("");
            }

            _Projects.Clear();
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

                _Projects.Add(project);
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
