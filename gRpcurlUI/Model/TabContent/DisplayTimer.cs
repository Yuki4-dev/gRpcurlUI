using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace gRpcurlUI.Model.TabContent
{
    [INotifyPropertyChanged]
    public partial class DisplayTimer 
    {
        [ObservableProperty]
        private bool isShow = true;

        [ObservableProperty]
        private string displayTime = "0";

        private Stopwatch stopwatch = new ();

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
