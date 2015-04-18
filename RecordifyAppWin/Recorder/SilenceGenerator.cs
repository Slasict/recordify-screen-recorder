using System;
using NAudio.Wave;

namespace RecordifyAppWin.Recorder
{
    public class SilenceGenerator : IWaveProvider
    {
        private readonly WaveFormat waveFormat = new WaveFormat(44100, 16, 2);

        public int Read(byte[] buffer, int offset, int count)
        {
            Array.Clear(buffer, offset, count);
            return count;
        }

        public WaveFormat WaveFormat
        {
            get { return waveFormat; }
        }

        public long Position
        {
            get { return -1; }
            set
            {
                throw new InvalidOperationException();
            }
        }

        public long Length
        {
            get { return -1; }
        }

        public void Dispose()
        {
            //do nothing
        }
    }
}
