using System;
using System.Collections.Generic;
using System.Windows;

namespace RecordifyAppWin.Recorder.Model
{
    public class RecordingInfo
    {
        public RecordingInfo()
        {
            Formats = new List<string> {"mp4"};
        }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Path { get { return Location + "\\" + Name; } }
        public double Duration { get; set; }
        public DateTime Date { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public Size Dimension { get { return new Size(Width, Height); } }
        public string Url { get; set; }
        public string ActionKey { get; set; }
        public string Recorder { get; set; }
        public List<string> Formats { get; private set; }
    }
}
