
namespace gRpcurlUI.Model.Grpcurl
{
    public class GrpcurlProject : Observable, IProject
    {
        private string _ProjectName = "";
        public string ProjectName
        {
            get => _ProjectName;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    OnPropertyChanged(ref _ProjectName, value);
                }
            }
        }

        private string _EndPoint = "";
        public string EndPoint
        {
            get => _EndPoint;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    OnPropertyChanged(ref _EndPoint, value);
                }
            }
        }

        private string _Option = "";
        public string Option
        {
            get => _Option;
            set => OnPropertyChanged(ref _Option, value?.Trim());
        }

        private string _Service = "";
        public string Service
        {
            get => _Service;
            set => OnPropertyChanged(ref _Service, value?.Trim());
        }

        private string _SendContent = "";
        public string SendContent
        {
            get => _SendContent;
            set => OnPropertyChanged(ref _SendContent, value);
        }

        public GrpcurlProject() { }

        public object Clone()
        {
            return new GrpcurlProject()
            {
                ProjectName = _ProjectName,
                EndPoint = _EndPoint,
                Option = _Option,
                Service = _Service
            };
        }
    }
}
