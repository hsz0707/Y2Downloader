namespace Y2Downloader.Common.Interfaces;

public interface ILinkReader
{
    Task<ISet<string>> GetLinksAsync();

    void Init();

    Task SaveFailedLinksAsync(ISet<string> links);
}