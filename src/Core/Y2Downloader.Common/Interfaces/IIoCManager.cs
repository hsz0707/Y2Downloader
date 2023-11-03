namespace Y2Downloader.Common.Interfaces;

public interface IIoCManager
{
    IY2DownloaderApp GetApp();

    void RegisterTypes<TSettings, TLogger>()
        where TLogger : class, IClientLogger
        where TSettings : class, IAppSettings;
}