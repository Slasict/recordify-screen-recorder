using System;
using System.Windows;
using System.Windows.Input;

namespace RecordifyAppWin.MainWindowView.Commands
{
    public class MouseMoveAction : ICommand
    {
        private MainWindowModel model;
        private MainWindowViewModel viewModel;

        public MouseMoveAction(MainWindowViewModel viewModel)
        {
            this.viewModel = viewModel;
            model = this.viewModel.MainWindowModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                model.IsSelected = false;
                Point currentMousePos = Mouse.GetPosition(Application.Current.MainWindow);
                model.Width = Math.Abs(model.StartPos.X - currentMousePos.X);
                model.Height = Math.Abs(model.StartPos.Y - currentMousePos.Y);
                model.Width = (model.Width % 2 == 0) ? model.Width : model.Width + 1;
                model.Height = (model.Height % 2 == 0) ? model.Height : model.Height + 1;
                model.Leftoffset = Math.Min(model.StartPos.X, currentMousePos.X);
                model.Topoffset = Math.Min(model.StartPos.Y, currentMousePos.Y);
                model.RecordButton.Position = viewModel.GetPossibleBtnLoc();
                model.RecordButton.IsVisible = true;
            }
        }
    }
}