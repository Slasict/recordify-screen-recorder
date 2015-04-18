using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RecordifyAppWin.RecManagerWindowView.Commands
{
    public class ListViewSizeChanged : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            ListView listView = parameter as ListView;
            GridView gView = listView.View as GridView;

            double workingWidth = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth;
            var columnWidths = new[] {0.29, 0.10, 0.18, 0.11, 0.32};

            for (var i = 0; i < gView.Columns.Count; i++)
            {
                gView.Columns[i].Width = workingWidth * columnWidths[i];
            }
        }
    }
}
