namespace Core.Facilities
{
    public interface ILogger
    {
        void LogData(string Message, int FileList);

        void Dispose();
    }
}

public enum LoggerFileList
{
    Error,
    Event,
    Data
};
