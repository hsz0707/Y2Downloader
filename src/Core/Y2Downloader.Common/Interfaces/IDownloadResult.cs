namespace Y2Downloader.Common.Interfaces;

public interface IDownloadResult
{
    int DownloadedFileCount { get; }

    ISet<string> FailedLinks { get; }

    bool IsSuccessful { get; }
}