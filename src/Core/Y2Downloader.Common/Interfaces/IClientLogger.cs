namespace Y2Downloader.Common.Interfaces;

public interface IClientLogger
{
    void LogError(string title, Exception e);

    void LogInfo(string message);
}