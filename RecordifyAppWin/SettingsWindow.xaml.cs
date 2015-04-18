using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;
using RecordifyAppWin.SettingsWindowView;

namespace RecordifyAppWin
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>

    public partial class SettingsWindow : Window
    {
        private AvailableHotkeyModel previousSelection1, previousSelection2;

        public SettingsWindow()
        {
            InitializeComponent();
            DataContext = new SettingsViewModel();
            Modifier1.SelectionChanged += modifier1_SelectionChanged;
            Modifier2.SelectionChanged += modifier2_SelectionChanged;
        }

        private void modifier1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AvailableHotkeyModel this_selected = (AvailableHotkeyModel)(sender as ComboBox).SelectedItem;
            AvailableHotkeyModel modifier2_selected = (AvailableHotkeyModel)(Modifier2 as ComboBox).SelectedItem;

            if (modifier2_selected != null)
            {
                var Modifier2_Items = Modifier2.ItemsSource as List<AvailableHotkeyModel>;
                var findResult = Modifier2_Items.Find(x => x.Name == this_selected.Name);
                if (findResult != null)
                {
                    findResult.Enabled = false;
                    if (this_selected.Name == modifier2_selected.Name)
                    {
                        Modifier2.SelectedIndex = 0;
                    }
                    if (previousSelection2 != null)
                    {
                        previousSelection2.Enabled = true;
                    }
                    previousSelection2 = findResult;
                }
            }
        }

        private void modifier2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AvailableHotkeyModel this_selected = (AvailableHotkeyModel)(sender as ComboBox).SelectedItem;
            AvailableHotkeyModel modifier1_selected = (AvailableHotkeyModel)(Modifier1 as ComboBox).SelectedItem;

            var Modifier1_Items = Modifier1.ItemsSource as List<AvailableHotkeyModel>;
            var findResult = Modifier1_Items.Find(x => x.Name == this_selected.Name);
            if (findResult != null)
            {
                findResult.Enabled = false;
                if (this_selected.Name == modifier1_selected.Name)
                {
                    Modifier1.SelectedIndex = 0;
                }
                if (previousSelection1 != null)
                {
                    previousSelection1.Enabled = true;
                }
                previousSelection1 = findResult;
            }
        }

        private void hotkey_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            Hotkey.Text = e.Key.ToString();
        }
    }

}
