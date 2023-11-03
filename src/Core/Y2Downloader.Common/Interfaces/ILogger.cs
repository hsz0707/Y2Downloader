namespace Y2Downloader.Common.Interfaces;

public interface ILogger
{
    void Init();

    Task LogErrorAsync(string title, Exception e);
}