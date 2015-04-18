using System.Windows;
using RecordifyAppWin.VideoProcessWindowView;

namespace RecordifyAppWin
{
    /// <summary>
    /// Interaction logic for VideoProcess.xaml
    /// </summary>
    public partial class VideoProcessWindow : Window
    {
        public VideoProcessWindow()
        {
            InitializeComponent();
            DataContext = new VideoProcessViewModel();
        }
    }
}
