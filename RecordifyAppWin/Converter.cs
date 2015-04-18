using System;
using System.Data.Odbc;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace RecordifyAppWin
{
    class TimeOfProgress : EventArgs
    {
        public double Second { get; set; }
        public double DonePercentage { get; set; }
    }

    class Converter
    {
        public string Path { get; set; }
        public bool FinishedGood = false;
        public double FileSize;
        public EventHandler<TimeOfProgress> Progress;
        public EventHandler Exit;
        private Process process;
        private ProcessStartInfo processInfo;
        private string webmArgument = "-i {0} -c:v libvpx -qmax 30 -c:a libvorbis -cpu-used -4 -deadline realtime {1}";
        private string gifArgument = "-i {0} -r 15 -pix_fmt rgb24 {1}";
        private string combineArgument = "-i {0} -i {1} -c:v copy -c:a copy -shortest {2}";
        private Regex ffmpegProgressRegex = new Regex("frame=\\s*(\\d+)\\s*fps=\\s*(\\d+).*size=\\s*(\\d+).*time=([0-9:.]+)");
        private Regex ffmpegDurationRegex = new Regex("Duration:\\s*([0-9:.]+)");
        private Regex ffmpegSuccessfullyDoneRegex = new Regex("video:.*?audio:.*?subtitle:.*?other.*?global.*?muxing.*");
        private double inputDurationTotalSeconds = -1;

        public Converter(string path)
        {
            Path = path;
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
        }

        public void ToWebm()
        {
            FinishedGood = false;
            processInfo.Arguments = String.Format(webmArgument, Path + ".mp4", Path + ".webm");
            process = new Process { StartInfo = processInfo };
            process.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
            process.ErrorDataReceived += (sender, args) => ProcessOutput(args.Data);
            //process.Exited += (sender, args) => OnExit();
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
        }

        public void ToGif()
        {
            FinishedGood = false;
            processInfo.Arguments = String.Format(gifArgument, Path + ".mp4", Path + ".gif");
            process = new Process { StartInfo = processInfo };
            process.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
            process.ErrorDataReceived += (sender, args) => ProcessOutput(args.Data);
            //process.Exited += (sender, args) => OnExit();
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
        }

        // Combine/Merge audio and video file
        public void Combine()
        {
            FinishedGood = false;
            processInfo.Arguments = String.Format(combineArgument, Path + ".mp4", Path + ".mp3", Path + "_combined.mp4");
            process = new Process { StartInfo = processInfo };
            process.Start();
            process.WaitForExit();
            if (File.Exists(Path + "_combined.mp4"))
            {
                File.Delete(Path + ".mp4");
                File.Delete(Path + ".mp3");
                File.Move(Path + "_combined.mp4", Path + ".mp4");
            }
        }

        public void Stop()
        {
            process.Kill();
        }

        private void ProcessOutput(string output)
        {
            if (output == null) return;
            if (inputDurationTotalSeconds < 0)
            {
                Match durationMatch = ffmpegDurationRegex.Match(output);
                if (durationMatch.Success)
                {
                    inputDurationTotalSeconds = Convert.ToDateTime(durationMatch.Groups[1].Value).TimeOfDay.TotalSeconds;
                }
            }
            else
            {
                Match progressMatch = ffmpegProgressRegex.Match(output);
                if (progressMatch.Success)
                {
                    DateTime dateTime = Convert.ToDateTime(progressMatch.Groups[4].Value);
                    TimeOfProgress TOP = new TimeOfProgress
                    {
                        Second = dateTime.TimeOfDay.TotalSeconds,
                        DonePercentage = 100 / inputDurationTotalSeconds * dateTime.TimeOfDay.TotalSeconds
                    };
                    OnProgress(TOP);
                }
                Match finishedMatch = ffmpegSuccessfullyDoneRegex.Match(output);
                if (finishedMatch.Success)
                {
                    FinishedGood = true;
                }
            }
        }

        public void OnProgress(TimeOfProgress args)
        {
            EventHandler<TimeOfProgress> handler = Progress;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public void OnExit()
        {
            EventHandler handler = Exit;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
