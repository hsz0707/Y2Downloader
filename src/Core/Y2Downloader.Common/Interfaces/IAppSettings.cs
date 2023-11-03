namespace Y2Downloader.Common.Interfaces;

public interface IAppSettings
{
    int SourceLocationProcessDelay { get; }

    void Init();
}