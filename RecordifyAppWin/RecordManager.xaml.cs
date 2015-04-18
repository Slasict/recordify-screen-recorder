using System;
using System.Windows;
using System.Windows.Controls;
using RecordifyAppWin.RecManagerWindowView;

namespace RecordifyAppWin
{
    public partial class RecordManager : Window
    {
        public RecordManager()
        {
            InitializeComponent();
            DataContext = new RecManagerViewModel();
        }

        private void linkButton_Click(object sender, RoutedEventArgs e)
        {
            string content = (sender as Button).Content.ToString();
            Console.WriteLine(content);
            if (content != "" && content != "Not uploaded")
            {
                Clipboard.SetDataObject(content);
            }
        }
    }
}
