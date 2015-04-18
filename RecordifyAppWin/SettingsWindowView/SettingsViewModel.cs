using System.Collections.Generic;
using System.Windows.Input;
using RecordifyAppWin.NotificationUserControl;
using RecordifyAppWin.SettingsWindowView.Commands;

namespace RecordifyAppWin.SettingsWindowView
{
    public class SettingsViewModel
    {
        public SettingsViewModel()
        {
            Settings = new SettingsModel();
            Settings.RecorderList = new List<SettingsRecorderListModel>
            {
                new SettingsRecorderListModel("VLC (broken)", "VLCRecorder", false),
                new SettingsRecorderListModel("FFMPEG (default, faster)", "FFMPEGRecorder")
            };
            Settings.HotkeyList = new List<AvailableHotkeyModel>
            {
                new AvailableHotkeyModel("NONE"),
                new AvailableHotkeyModel("CTRL"),
                new AvailableHotkeyModel("ALT"),
                new AvailableHotkeyModel("SHIFT"),
                new AvailableHotkeyModel("WIN")
            };
            BrowseSaveLocationCommand = new BrowseSaveLocation(this);
            SaveSettingsCommand = new SaveSettings(this);
        }

        public SettingsModel Settings { get; set; }

        public ICommand BrowseSaveLocationCommand { get; private set; }

        public ICommand SaveSettingsCommand { get; private set; }

        public void SaveChanges()
        {
            Properties.Settings.Default.Save();
            Notification.Instance.ShowNotificationBalloon("Settings changed.", "Settings has been successfully saved.");
        }
    }
}