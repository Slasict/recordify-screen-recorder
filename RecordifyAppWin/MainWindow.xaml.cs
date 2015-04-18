using System;
using System.Windows;
using System.Timers;
using System.Threading.Tasks;
using RecordifyAppWin.NotificationUserControl;
using RecordifyAppWin.Recorder;

namespace RecordifyAppWin
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Timer noticeMe = new Timer
            {
                Interval = 500,
                AutoReset = false,
            };
            noticeMe.Elapsed += (s, e) => Notification.Instance.ShowBalloonyTip("Recordify! is running.", "Double-click or right-click on me to start recording.");
            noticeMe.Start();
        }
    }
}
