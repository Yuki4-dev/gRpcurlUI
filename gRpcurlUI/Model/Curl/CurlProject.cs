﻿using CommunityToolkit.Mvvm.ComponentModel;
using gRpcurlUI.Core.Model;
using gRpcurlUI.Core.Process;
using Newtonsoft.Json;
using System;
using System.Text;
using static gRpcurlUI.Language;

namespace gRpcurlUI.Model.Curl
{
    public partial class CurlProject : ObservableObject, IProject
    {
        public Type JsonType => typeof(CurlProjectJson);

        public CurlProjectLanguage Texts => Language.Default.CurlProject;

        [ObservableProperty]
        private string projectName = string.Empty;

        [ObservableProperty]
        private string endPoint = string.Empty;

        [ObservableProperty]
        private bool isSelected = false;

        [ObservableProperty]
        private string option = string.Empty;

        [ObservableProperty]
        private bool isJsonContent = false;

        [ObservableProperty]
        private string sendContent = string.Empty;

        public CurlProject() { }

        public bool PrepareProject(out string message)
        {
            var sb = new StringBuilder();
            if (string.IsNullOrWhiteSpace(EndPoint))
            {
                _ = sb.AppendLine(Language.Default.CurlProject.EndPointBlank);
            }

            if (!string.IsNullOrWhiteSpace(SendContent) && IsJsonContent)
            {
                try
                {
                    SendContent = FormatJson(SendContent, Formatting.Indented);
                }
                catch (Exception ex)
                {
                    _ = sb.AppendLine(ex.Message);
                }
            }

            message = sb.ToString();
            return sb.Length == 0;
        }

        public IProcessCommand CreateCommand()
        {
            string jsonContent = SendContent;
            if (!string.IsNullOrWhiteSpace(jsonContent) && IsJsonContent)
            {
                try
                {
                    jsonContent = FormatJson(jsonContent);
                }
                catch { }
            }
            return new CurlCommand(Option, EndPoint, jsonContent);
        }

        public object ToJsonObject()
        {
            return new CurlProjectJson()
            {
                ProjectName = ProjectName,
                EndPoint = EndPoint,
                Option = Option,
                IsJsonContent = IsJsonContent,
                IsSelected = IsSelected,
                SendContent = SendContent,
            };
        }

        public void LoadJsonObject(object jsonObject)
        {
            if (jsonObject is not CurlProjectJson curlProject)
            {
                throw new Exception(Language.Default.CurlProject.NonJsonType);
            }

            ProjectName = curlProject.ProjectName;
            EndPoint = curlProject.EndPoint;
            Option = curlProject.Option;
            IsJsonContent = curlProject.IsJsonContent;
            IsSelected = curlProject.IsSelected;
            SendContent = curlProject.SendContent;
        }

        private static string FormatJson(string json, Formatting format = Formatting.None)
        {
            var parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson, format);
        }

        internal class CurlProjectJson
        {
            public string ProjectName { get; set; } = string.Empty;
            public string EndPoint { get; set; } = string.Empty;
            public bool IsSelected { get; set; } = false;
            public string Option { get; set; } = string.Empty;
            public bool IsJsonContent { get; set; } = false;
            public string SendContent { get; set; } = string.Empty;
        }
    }
}

