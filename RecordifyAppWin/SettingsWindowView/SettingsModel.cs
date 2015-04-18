using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using RecordifyAppWin.Hotkey;
using RecordifyAppWin.Properties;

namespace RecordifyAppWin.SettingsWindowView
{
    public class SettingsRecorderListModel
    {
        public SettingsRecorderListModel(string name, string className, bool enabled = true)
        {
            Name = name;
            ClassName = className;
            Enabled = enabled;
        }

        public string Name { get; set; }
        public string ClassName { get; set; }
        public bool Enabled { get; set; }
    }

    public class AvailableHotkeyModel : INotifyPropertyChanged
    {
        private bool enabled;

        public AvailableHotkeyModel(string name)
        {
            Name = name;
            Enabled = true;
        }

        public string Name { get; set; }

        public bool Enabled
        {
            get { return enabled; }
            set
            {
                enabled = value;
                OnPropertyChanged("Enabled");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class SettingsModel : INotifyPropertyChanged
    {
        public SettingsModel()
        {
            Settings.Default.Reload();
        }

        public string SaveLocation
        {
            get { return Settings.Default.SaveLocation; }
            set
            {
                Settings.Default.SaveLocation = value;
                OnPropertyChanged("SaveLocation");
            }
        }

        public bool KeepVideo
        {
            get { return Settings.Default.KeepVideo; }
            set
            {
                Settings.Default.KeepVideo = value;
                Settings.Default.KeepVideo = value;
                OnPropertyChanged("KeepVideo");
            }
        }

        public HotKey HotKey
        {
            get
            {
                string[] sp = Settings.Default.StartStopHotkey.Split(new[] {"_+_"}, StringSplitOptions.None);
                ModifierKeys mKey1 = StringToModifierKey(sp[0]);
                ModifierKeys mKey2 = StringToModifierKey(sp[1]);
                Keys key;
                Enum.TryParse(sp[2], out key);
                return new HotKey(mKey1 | mKey2, key, Application.Current.MainWindow);
            }
        }

        public string HotKeyAsString
        {
            get
            {
                string[] sp = Settings.Default.StartStopHotkey.Split(new[] {"_+_"}, StringSplitOptions.None);
                return sp[0] + "+" + sp[1] + "+" + sp[2];
            }
        }

        public String ModifierKey1
        {
            get { return Settings.Default.StartStopHotkey.Split(new[] {"_+_"}, StringSplitOptions.None)[0]; }
            set
            {
                string[] sp = Settings.Default.StartStopHotkey.Split(new[] {"_+_"}, StringSplitOptions.None);
                Settings.Default.StartStopHotkey = value + "_+_" + sp[1] + "_+_" + sp[2];
                OnPropertyChanged("SelectedMod1");
            }
        }

        public String ModifierKey2
        {
            get { return Settings.Default.StartStopHotkey.Split(new[] {"_+_"}, StringSplitOptions.None)[1]; }
            set
            {
                string[] sp = Settings.Default.StartStopHotkey.Split(new[] {"_+_"}, StringSplitOptions.None);
                Settings.Default.StartStopHotkey = sp[0] + "_+_" + value + "_+_" + sp[2];
                OnPropertyChanged("SelectedMod2");
            }
        }

        public String Key
        {
            get { return Settings.Default.StartStopHotkey.Split(new[] {"_+_"}, StringSplitOptions.None)[2]; }
            set
            {
                string[] sp = Settings.Default.StartStopHotkey.Split(new[] {"_+_"}, StringSplitOptions.None);
                Settings.Default.StartStopHotkey = sp[0] + "_+_" + sp[1] + "_+_" + value;
                OnPropertyChanged("Key");
            }
        }

        public string Recorder
        {
            get { return Settings.Default.DefaultRecorder; }
            set
            {
                Settings.Default.DefaultRecorder = value;
                OnPropertyChanged("Recorder");
            }
        }

        public int CountdownSecond
        {
            get { return Settings.Default.CountdownSecond; }
            set
            {
                if (value > 10 || value < 0)
                {
                    value = Settings.Default.CountdownSecond;
                }
                Settings.Default.CountdownSecond = value;
                OnPropertyChanged("CountdownSecond");
            }
        }

        public List<SettingsRecorderListModel> RecorderList { get; set; }

        public List<AvailableHotkeyModel> HotkeyList { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private ModifierKeys StringToModifierKey(string str)
        {
            str = str.ToLower();
            ModifierKeys key;
            if (str == "alt")
            {
                key = ModifierKeys.Alt;
            }
            else if (str == "ctrl")
            {
                key = ModifierKeys.Control;
            }
            else if (str == "win")
            {
                key = ModifierKeys.Windows;
            }
            else if (str == "shift")
            {
                key = ModifierKeys.Shift;
            }
            else
            {
                key = ModifierKeys.None;
            }
            return key;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}