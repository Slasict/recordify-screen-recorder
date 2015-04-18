using System;
using System.Windows;
using System.Windows.Input;

namespace RecordifyAppWin
{
    public partial class BaseWindow : ResourceDictionary
    {

        public BaseWindow()
        {
            InitializeComponent();
        }

        private void titleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;
            window.DragMove();
        }
        
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;
            window.Close();
        }

        private void minimizeButton_Click(object sender, RoutedEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;
            window.WindowState = WindowState.Minimized;
        }
    }
}