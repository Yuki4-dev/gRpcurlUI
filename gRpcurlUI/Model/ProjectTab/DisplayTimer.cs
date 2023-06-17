using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace gRpcurlUI.Model.ProjectTab
{
    [INotifyPropertyChanged]
    public partial class DisplayTimer 
    {
        [ObservableProperty]
        private bool isShow = true;

        [ObservableProperty]
        private string displayTime = "0";

        private readonly Stopwatch stopwatch = new ();

        public DisplayTimer()
        {
        }

        public void Start()
        {
            DisplayTime = TimeSpan.Zero.ToString();
            stopwatch.Restart();
            stopwatch.Start();
        }

        public void Stop()
        {
            stopwatch.Stop();
            var time = TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds);
            DisplayTime = time.ToString();   
        }
    }
}
