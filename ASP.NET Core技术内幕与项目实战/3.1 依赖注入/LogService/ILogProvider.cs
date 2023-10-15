namespace LogService
{
    public interface ILogProvider
    {
        void LogErr(string content);

        void LogInfo(string content);
    }
}
