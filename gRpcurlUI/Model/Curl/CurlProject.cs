
namespace gRpcurlUI.Model.Curl
{
    public class CurlProject : Observable, IProject
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

        private bool _IsJsonContent = false;
        public bool IsJsonContent
        {
            get => _IsJsonContent;
            set => OnPropertyChanged(ref _IsJsonContent, value);
        }

        private string _SendContent = "";
        public string SendContent
        {
            get => _SendContent;
            set => OnPropertyChanged(ref _SendContent, value);
        }

        public CurlProject() { }

        public object Clone()
        {
            return new CurlProject()
            {
                ProjectName = _ProjectName,
                EndPoint = _EndPoint,
                Option = _Option,
            };
        }
    }
}
