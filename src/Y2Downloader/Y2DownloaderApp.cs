namespace Y2Downloader;

using Common.Interfaces;

public class Y2DownloaderApp : IY2DownloaderApp
{
    private readonly IClientLogger _clientLogger;

    private readonly IY2Downloader _downloader;

    private readonly ILinkReader _linkReader;

    private readonly ILogger _logger;

    private readonly IAppSettings _settings;

    public Y2DownloaderApp(
        ILogger logger,
        IClientLogger clientLogger,
        ILinkReader linkReader,
        IY2Downloader downloader,
        IAppSettings settings)
    {
        _logger = logger;
        _clientLogger = clientLogger;
        _linkReader = linkReader;
        _downloader = downloader;
        _settings = settings;
    }

    public void Init()
    {
        _settings.Init();
        _logger.Init();
        _linkReader.Init();
        _downloader.Init();
    }

    public async Task RunAsync()
    {
        while (true)
        {
            try
            {
                var links = await _linkReader.GetLinksAsync();

                if (links.Count > 0)
                {
                    _clientLogger.LogInfo("Cycle started.");
                    _clientLogger.LogInfo($"Found link count: {links.Count}.");
                }

                var downloadResult = await _downloader.DownloadFromLinksAsync(links);

                if (!downloadResult.IsSuccessful)
                {
                    _clientLogger.LogInfo($"Failed link count: {downloadResult.FailedLinks.Count}.");

                    await _linkReader.SaveFailedLinksAsync(downloadResult.FailedLinks);
                }

                if (links.Count > 0)
                {
                    _clientLogger.LogInfo($"Downloaded file count: {downloadResult.DownloadedFileCount}.");
                    _clientLogger.LogInfo("Cycle ended.");
                }
            }
            catch (Exception e)
            {
                const string ErrorTitle = nameof(RunAsync);
                _clientLogger.LogError(ErrorTitle, e);
                await _logger.LogErrorAsync(ErrorTitle, e);
            }

            await Task.Delay(_settings.SourceLocationProcessDelay);
        }

        // ReSharper disable once FunctionNeverReturns
    }
}