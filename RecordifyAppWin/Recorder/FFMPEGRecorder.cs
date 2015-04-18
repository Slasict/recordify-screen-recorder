using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace RecordifyAppWin.Recorder
{
    class FFMPEGRecorder : IRecorderInterface
    {

        private ProcessStartInfo processInfo;
        private Process process;
        private AudioRecorder audioRecorder;
        private const string ffmpegDshowOption = "-f dshow -i video=\"screen-capture-recorder\" -f dshow -i audio=\"virtual-audio-capturer\" -c:v libx264 -pix_fmt yuv420p -preset ultrafast -vprofile baseline -level 3.0 -crf 16 -c:a libmp3lame -b:v 3000k -minrate 3000k -maxrate 3000k -bufsize 4000k -vf crop={0}:{1}:{2}:{3} -y {4}";
        // recording mp4 and webm at same time causes lag in video. we should first record mp4 and then convert finished mp4 to webm
        private const string ffmpegGdigrabOption = "-f gdigrab -video_size {0}x{1} -offset_x {2} -offset_y {3} -i desktop -pix_fmt yuv420p -preset ultrafast -profile:v baseline -level 3.0 -crf 35 -c:v libx264 {4}.mp4";
        private Regex ffmpegProgressRegex = new Regex("^frame=\\s+(\\d+)\\s+fps=\\s+(\\d+).*size=\\s+(\\d+).*time=([0-9:.]+)");
        private string timeString;

        public FFMPEGRecorder()
        {
            processInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                FileName = "RecorderLib/Ffmpeg/ffmpeg.exe",
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                RedirectStandardError = true
            };
            audioRecorder = new AudioRecorder();
        }

        public double Duration { get; set; }
        public bool RecorderRunning { get; set; }

        public void Start(double offsetTop, double offsetLeft, double width, double height, string location, string name)
        {
            processInfo.Arguments = String.Format(ffmpegGdigrabOption, width, height, offsetLeft, offsetTop, location + "\\" + name);
            process = new Process();
            process.StartInfo = processInfo;
            process.OutputDataReceived += (sender, args) => Console.WriteLine(@"Output data: " + args.Data);
            process.ErrorDataReceived += (sender, args) => ProcessOutput(args.Data);
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            RecorderRunning = true;
            audioRecorder.Init(location + "\\" + name);
            audioRecorder.Play();
        }

        public void Stop()
        {
            audioRecorder.Stop();
            process.StandardInput.WriteLine("q");
            process.WaitForExit();
            RecorderRunning = false;
            DateTime dateTime = Convert.ToDateTime(timeString);
            Duration = int.Parse(dateTime.ToString("ss"));
        }

        private void ProcessOutput(string output)
        {
            if (output == null) return;
            Match match = ffmpegProgressRegex.Match(output);
            if (match.Success)
            {
                timeString = match.Groups[4].Value;
            }
        }
    }
}
