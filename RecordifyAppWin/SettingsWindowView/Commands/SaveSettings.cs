using System;
using System.Windows.Input;

namespace RecordifyAppWin.SettingsWindowView.Commands
{
    public class SaveSettings : ICommand
    {
        private SettingsViewModel viewModel;

        public SaveSettings(SettingsViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            viewModel.SaveChanges();
        }
    }
}