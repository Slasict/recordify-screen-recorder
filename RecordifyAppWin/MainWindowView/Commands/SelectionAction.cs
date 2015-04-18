using System;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using RecordifyAppWin.Hotkey;
using RecordifyAppWin.NotificationUserControl;
using RecordifyAppWin.Recorder.Model;
using RecordifyAppWin.SettingsWindowView;
using RecordifyAppWin.VideoProcessWindowView;

namespace RecordifyAppWin.MainWindowView.Commands
{
    public class SelectionAction : ICommand
    {

        private MainWindowViewModel viewModel;
        private RecordingInfo recordingInfo;
        private int counter, counterCurrent;
        private NotificationBalloon notificationBalloon;
        private NotificationModel notificationModel;
        private dynamic recorder;
        private HotKey recorderHotKey;
        private Timer trayIconTimer, countdownTimer;
        private bool trayIconSwitch;

        public SelectionAction(MainWindowViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            // record button clicked
            if (parameter != null)
            {
                recordingInfo = new RecordingInfo
                {
                    Name = System.IO.Path.GetRandomFileName().Replace(".", string.Empty),
                    Location = @Properties.Settings.Default.SaveLocation,
                    Width = viewModel.MainWindowModel.Width,
                    Height = viewModel.MainWindowModel.Height
                };
                // begin the countdown
                counter = new SettingsModel().CountdownSecond;
                counterCurrent = counter;
                notificationBalloon = Notification.Instance.ShowNotificationBalloon("Press " + new SettingsModel().HotKeyAsString + " stop process.", "Process starting...", counter * 1000 + 2000);
                notificationModel = (notificationBalloon.DataContext as NotificationViewModel).Model;
                countdownTimer = new Timer
                {
                    Interval = 1000
                };
                countdownTimer.Elapsed += CountdownTimerEvent;
                countdownTimer.Start();

                // Making selection click-through
                viewModel.MainWindowModel.Foreground = "#00000000";
                viewModel.MainWindowModel.Background = "#00000000";
                viewModel.MainWindowModel.RecordButton.IsVisible = false;
            }
            // contextmenu item dorecord clicked
            else
            {
                if (viewModel.MainWindowModel.IsRecording)
                {
                    Stop();
                    return;
                }

                if (viewModel.MainWindowModel.SelectionVisibility == Visibility.Hidden)
                {
                    viewModel.ShowSelection();
                }
                else
                {
                    viewModel.HideSelection();
                }
            }
        }

        private void Record()
        {
            try
            {
                recordingInfo.Recorder = Properties.Settings.Default.DefaultRecorder;
                var recorderType = Type.GetType("RecordifyAppWin.Recorder." + recordingInfo.Recorder);
                if (recorderType == null)
                {
                    Notification.Instance.ShowNotificationBalloon("Oh snap!", "Error initializing recorder.");
                    viewModel.HideSelection();
                }

                recorder = Activator.CreateInstance(recorderType);
                viewModel.MainWindowModel.IsRecording = true;
                viewModel.ContextMenuItems[0].Text = "Stop";
                viewModel.MainWindowModel.Stroke = "Red";
                recorder.Start(viewModel.MainWindowModel.Topoffset, viewModel.MainWindowModel.Leftoffset, viewModel.MainWindowModel.Width, viewModel.MainWindowModel.Height, recordingInfo.Location, recordingInfo.Name);
                RegisterStopHotkey();
                trayIconTimer = new Timer
                {
                    Interval = 1000
                };
                trayIconTimer.Elapsed += TrayIconBlinkTimerEvent;
                trayIconTimer.Start();
            }
            catch (Exception ex)
            {
                Stop(ex);
            }
        }

        private void Stop(Exception ex = null)
        {
            viewModel.HideSelection();
            viewModel.MainWindowModel.IsRecording = false;
            viewModel.ContextMenuItems[0].Text = "Record";
            Notification.Instance.ChangeIcon(Properties.Resources.icon_grey);
            UnregisterStopHotkey();

            if (trayIconTimer != null)
            {
                trayIconTimer.Stop();
            }

            if (ex != null || recorder.RecorderRunning == false) return;

            recorder.Stop();
            recordingInfo.Duration = recorder.Duration;
            recordingInfo.Date = DateTime.Now;

            VideoProcessWindow videoProcessor = new VideoProcessWindow();
            videoProcessor.Visibility = Visibility.Visible;
            videoProcessor.Activate();
            var videoProcessViewModel = videoProcessor.DataContext as VideoProcessViewModel;
            if (videoProcessViewModel != null)
            {
                videoProcessViewModel.Model.RecordingInfo = recordingInfo;
                videoProcessViewModel.StartProcess();
            }
            else
            {
                Notification.Instance.ShowNotificationBalloon("Oh snap!", "Error opening video-processor.");
            }
        }

        private void RegisterStopHotkey()
        {
            // Global Stop Hotkey
            HotKey hk = new SettingsModel().HotKey;
            recorderHotKey = hk;
            recorderHotKey.HotKeyPressed += k => Stop();
        }

        private void UnregisterStopHotkey()
        {
            if (recorderHotKey != null) recorderHotKey.UnregisterHotKey();
        }

        private void CountdownTimerEvent(object source, ElapsedEventArgs e)
        {
            // Countdown timer before starting a record
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (counterCurrent == 0)
                {
                    notificationModel.BalloonContent = "Time's up. Live!";
                    countdownTimer.Stop();
                    counterCurrent = counter; // reset
                    Record();
                }
                else
                {
                    notificationModel.BalloonContent = counterCurrent.ToString();
                    counterCurrent--;
                }
            });

        }

        private void TrayIconBlinkTimerEvent(object source, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Notification.Instance.ChangeIcon(trayIconSwitch ? Properties.Resources.icon_red : Properties.Resources.icon_grey);
                trayIconSwitch = !trayIconSwitch;
            });
        }

    }
}
