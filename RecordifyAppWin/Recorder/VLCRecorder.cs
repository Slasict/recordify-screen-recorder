using System;
using System.Diagnostics;
using Vlc.DotNet.Core;
using Vlc.DotNet.Core.Medias;
using Vlc.DotNet.Wpf;

namespace RecordifyAppWin.Recorder
{
    class VLCRecorder : IRecorderInterface
    {
        private LocationMedia vlcLocationMedia;
        private VlcControl vlcControl;
        private Stopwatch durationWatch;
        private AudioRecorder audioRecorder;
        private string vlcMediaOption = ":sout=#transcode{{vcodec=h264,acodec=none}}:file{{dst={0}.mp4}}";

        public VLCRecorder()
        {
            VlcContext.LibVlcDllsPath = @"RecorderLib/Vlc/";
            VlcContext.LibVlcPluginsPath = @"RecorderLib/Vlc/plugins/";
            VlcContext.StartupOptions.IgnoreConfig = true;
            VlcContext.StartupOptions.ShowLoggerConsole = false;
            VlcContext.StartupOptions.AddOption("--screen-fps=25");
            VlcContext.StartupOptions.AddOption("--no-screen-follow-mouse");
            VlcContext.StartupOptions.AddOption("--screen-mouse-image=cursor.png");
            audioRecorder = new AudioRecorder();
            durationWatch = new Stopwatch();
        }

        public double Duration { get; set; }
        public string[] RecorderFormats
        {
            get { return new string[] { "mp4" }; }
        }

        public void Start(double offsetTop, double offsetLeft, double width, double height, string location, string name)
        {
            VlcContext.StartupOptions.AddOption("--screen-top=" + offsetTop);
            VlcContext.StartupOptions.AddOption("--screen-left=" + offsetLeft);
            VlcContext.StartupOptions.AddOption("--screen-width=" + width);
            VlcContext.StartupOptions.AddOption("--screen-height=" + height);
            VlcContext.Initialize();
            vlcLocationMedia = new LocationMedia("screen://");
            vlcLocationMedia.AddOption(String.Format(vlcMediaOption, location + "\\" + name));
            audioRecorder.Init(location + "\\" + name);

            durationWatch.Reset();

            vlcControl = new VlcControl();
            vlcControl.Media = vlcLocationMedia;
            vlcControl.Playing += (sender, args) =>
            {
                durationWatch.Start();
                audioRecorder.Play();
            };
            vlcControl.Stopped += (sender, args) =>
            {
                durationWatch.Stop();
                audioRecorder.Stop();
            };
        }

        public void Stop()
        {
            vlcControl.Stop();
            vlcControl.Dispose();
            VlcContext.CloseAll();
            Duration = durationWatch.Elapsed.Seconds;
        }
    }
}