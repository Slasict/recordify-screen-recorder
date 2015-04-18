using System;
using System.Windows.Input;
using Ookii.Dialogs.Wpf;

namespace RecordifyAppWin.SettingsWindowView.Commands
{
    public class BrowseSaveLocation : ICommand
    {
        private SettingsViewModel viewModel;

        public BrowseSaveLocation(SettingsViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog
            {
                Description = "Please select a folder.",
                UseDescriptionForTitle = true
            };
            if (dialog.ShowDialog() == true)
            {
                viewModel.Settings.SaveLocation = dialog.SelectedPath;
            }
        }
    }
}
