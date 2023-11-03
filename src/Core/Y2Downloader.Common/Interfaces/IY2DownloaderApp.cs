namespace Y2Downloader.Common.Interfaces;

public interface IY2DownloaderApp
{
    void Init();

    Task RunAsync();
}