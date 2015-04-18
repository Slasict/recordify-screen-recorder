using System;
using System.Windows;
using System.Windows.Input;
using RecordifyAppWin.Recorder.Model;

namespace RecordifyAppWin.RecManagerWindowView.Commands
{
    public class DeleteRecording : ICommand
    {
        private RecManagerViewModel viewModel;

        public DeleteRecording(RecManagerViewModel viewModel)
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
            RecordingInfo selectedRecordingInfo = parameter as RecordingInfo;
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "Delete Confirmation",
                MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                viewModel.RecManagerService.DeleteRecording(selectedRecordingInfo);
            }
        }
    }
}