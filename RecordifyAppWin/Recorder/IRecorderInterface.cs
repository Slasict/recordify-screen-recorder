namespace RecordifyAppWin.Recorder
{
    interface IRecorderInterface
    {
        void Start(double offsetTop, double offsetLeft, double width, double height, string location, string name);
        void Stop();
    }
}
