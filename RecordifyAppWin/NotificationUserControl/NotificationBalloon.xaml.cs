using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RecordifyAppWin.NotificationUserControl
{
    /// <summary>
    /// Interaction logic for NotificationBalloon.xaml
    /// </summary>
    public partial class NotificationBalloon : UserControl
    {
        public NotificationBalloon()
        {
            InitializeComponent();
            DataContext = new NotificationViewModel();
        }
    }
}
