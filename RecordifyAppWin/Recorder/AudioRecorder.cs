using System;
using System.Diagnostics;
using NAudio.Wave;
using NAudio.Lame;

namespace RecordifyAppWin.Recorder
{
    public class AudioRecorder
    {
        private Stopwatch durationStopwatch;
        private IWaveIn waveIn;
        private WaveOut waveOut;
        private LameMP3FileWriter wri;

        public void Init(string path)
        {
            durationStopwatch = new Stopwatch();
            waveIn = new WasapiLoopbackCapture();
            wri = new LameMP3FileWriter(@path + ".mp3", waveIn.WaveFormat, 32);
            waveOut = new WaveOut();
            waveOut.Init(new SilenceGenerator());
        }

        public void Play()
        {
            // NAudio only captures when there is audio playing.
            // So If there is no audio playing, play silence instead.
            waveIn.DataAvailable += (s, e) =>
            {
                if (BitConverter.ToInt16(e.Buffer, 0) == 0)
                {
                    if (waveOut.PlaybackState == PlaybackState.Paused)
                    {
                        waveOut.Resume();
                    }
                    else if (waveOut.PlaybackState == PlaybackState.Stopped)
                    {
                        waveOut.Play();
                    }
                }
                else
                {
                    if (waveOut.PlaybackState == PlaybackState.Playing)
                    {
                        waveOut.Pause();
                    }
                }
                wri.Write(e.Buffer, 0, e.BytesRecorded);
            };
            waveIn.StartRecording();
            durationStopwatch.Start();
        }

        public void Stop()
        {
            waveIn.StopRecording();
            durationStopwatch.Stop();
            wri.Flush();
            waveIn.Dispose();
            wri.Dispose();
        }

        public long Duration()
        {
            return durationStopwatch.ElapsedMilliseconds;
        }
    }
}