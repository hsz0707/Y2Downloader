namespace Y2Downloader.Common.Interfaces;

public interface IY2Downloader
{
    Task<IDownloadResult> DownloadFromLinksAsync(ISet<string> links);

    void Init();
}