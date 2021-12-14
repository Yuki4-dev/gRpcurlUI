using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace gRpcurlUI.Model
{
    public class Observable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void OnPropertyChanged<T>(ref T prop, T value, [CallerMemberName] string name = null)
        {
            if (SetProperty(ref prop, value))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        public bool SetProperty<T>(ref T prop, T value)
        {
            if (Equals(prop, value))
            {
                return false;
            }

            prop = value;
            return true;
        }
    }
}
