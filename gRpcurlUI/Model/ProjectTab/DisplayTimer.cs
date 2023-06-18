using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace gRpcurlUI.Model.ProjectTab
{
    [INotifyPropertyChanged]
    public partial class DisplayTimer 
    {
        private const string FORMAT = @"mm\.ss\.fff";

        [ObservableProperty]
        private bool isShow = true;

        [ObservableProperty]
        private string displayTime = "0";

        private readonly Stopwatch stopwatch = new ();

        public DisplayTimer()
        {
            TimeInit();
        }

        public void Start()
        {
            TimeInit();
            stopwatch.Restart();
            stopwatch.Start();
        }

        public void Stop()
        {
            stopwatch.Stop();
            var time = TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds);
            DisplayTime = time.ToString(FORMAT);   
        }

        private void TimeInit()
        {
            DisplayTime = TimeSpan.Zero.ToString(FORMAT);
        }
    }
}
