using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using Hardcodet.Wpf.TaskbarNotification;
using Hardcodet.Wpf.TaskbarNotification.Interop;
using Point = Hardcodet.Wpf.TaskbarNotification.Interop.Point;

namespace RecordifyAppWin.NotificationUserControl
{
    internal class BalloonDisposable
    {
        private NotificationBalloon balloon;
        private Action<BalloonDisposable> callback;
        private int timeout;
        private Timer timer;

        public BalloonDisposable(NotificationBalloon balloon, int timeout, Action<BalloonDisposable> callback)
        {
            this.balloon = balloon;
            this.timeout = timeout;
            this.callback = callback;
            timer = new Timer(TimerCallback);
            timer.Change(timeout, Timeout.Infinite);
        }

        public NotificationBalloon NotificationBalloon
        {
            get { return balloon; }
        }

        private void TimerCallback(object state)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var anim = new DoubleAnimation(0, TimeSpan.FromSeconds(0.6));
                anim.Completed += (s, e) =>
                {
                    NotificationBalloon.Visibility = Visibility.Hidden;
                    callback(this);
                };
                NotificationBalloon.BeginAnimation(UIElement.OpacityProperty, anim);
            });
        }
    }

    public class Notification
    {
        private static Lazy<Notification> Lazy = new Lazy<Notification>(() => new Notification());
        private List<BalloonDisposable> notifictaionBalloonList = new List<BalloonDisposable>();
        private Thickness defaultMargin = new Thickness(0, 0, 10, 10);
        private TaskbarIcon tbIcon;

        private Notification()
        {
            Application.Current.Dispatcher.Invoke(
                () =>
                {
                    tbIcon =
                        (Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as
                            MainWindow).myTaskbarIcon;
                });
        }

        public static Notification Instance
        {
            get { return Lazy.Value; }
        }

        public void ShowBalloonyTip(string title, string description)
        {
            tbIcon.ShowBalloonTip(title, description, tbIcon.Icon);
        }

        public NotificationBalloon ShowNotificationBalloon(string title, string description, int timeout = 5000)
        {
            Point trayPosition = TrayInfo.GetTrayLocation();
            var notificationBalloon = new NotificationBalloon {Margin = defaultMargin};
            var notificationViewModel = notificationBalloon.DataContext as NotificationViewModel;
            notificationViewModel.Model.BalloonTitle = title;
            notificationViewModel.Model.BalloonContent = description;

            int balloons = notifictaionBalloonList.Count;
            if (balloons > 0)
            {
                Thickness newMargin = defaultMargin;
                newMargin.Bottom = (defaultMargin.Bottom * balloons) + notificationBalloon.Height * balloons +
                                   (SystemParameters.PrimaryScreenHeight - trayPosition.Y);
                notificationBalloon.Margin = newMargin;
            }

            tbIcon.ShowCustomBalloon(notificationBalloon, PopupAnimation.Fade, 10000000);
            notifictaionBalloonList.Add(new BalloonDisposable(notificationBalloon, timeout, FancyBalloonCloseCallback));
            return notificationBalloon;
        }

        private void FancyBalloonCloseCallback(BalloonDisposable balloonDisposable)
        {
            notifictaionBalloonList.Remove(balloonDisposable);
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (BalloonDisposable t in notifictaionBalloonList)
                {
                    Thickness newMargin = defaultMargin;
                    newMargin.Bottom = t.NotificationBalloon.Margin.Bottom - t.NotificationBalloon.Height -
                                       defaultMargin.Bottom;
                    t.NotificationBalloon.Margin = newMargin;
                }
            });
        }

        public void ChangeIcon(Icon icon)
        {
            tbIcon.Icon = icon;
        }
    }
}