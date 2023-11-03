namespace Y2Downloader.Mp3Downloader.Experimental;

using System.Diagnostics;
using System.Reflection;

using Common;
using Common.Helpers;
using Common.Interfaces;

using DotNetTools.SharpGrabber;
using DotNetTools.SharpGrabber.Grabbed;
using DotNetTools.SharpGrabber.YouTube;

public class Y2Downloader : IY2Downloader
{
    private const string AudioFileDownloadFolderName = "Download Result";

    private const string AudioFileExtension = "mp3";

    private const string RootFolderName = "Downloads";

    private readonly IClientLogger _clientLogger;

    private readonly ILogger _logger;

    private string? _downloadPath;

    private YouTubeGrabber? _grabber;

    public Y2Downloader(ILogger logger, IClientLogger clientLogger)
    {
        _logger = logger;
        _clientLogger = clientLogger;
    }

    public async Task<IDownloadResult> DownloadFromLinksAsync(ISet<string> links)
    {
        var result = new DownloadResult { IsSuccessful = true };
        var downloadPath = ExceptionBuilder.GetNotEmptyStringOrThrowException(_downloadPath);
        var fileNameSet = new HashSet<string>();
        var linkNumber = 1;

        foreach (var link in links)
        {
            try
            {
                if (!Directory.Exists(downloadPath))
                {
                    Directory.CreateDirectory(downloadPath);
                }

                var mediaResult = await ProcessLinkAsync(link);
                var fileNameWithNoSpecSymbols = RegexPool.SpecSymbol().Replace(mediaResult.Title, "");

                if (fileNameSet.Contains(fileNameWithNoSpecSymbols))
                {
                    _clientLogger.LogInfo($"File duplicate was found for link: {linkNumber} - [{link}].");
                }
                else
                {
                    var saveFilePath = Path.Combine(downloadPath, $"{fileNameWithNoSpecSymbols}.{AudioFileExtension}");
                    await File.WriteAllBytesAsync(saveFilePath, mediaResult.Bytes);

                    fileNameSet.Add(fileNameWithNoSpecSymbols);

                    _clientLogger.LogInfo($"Link downloaded: {linkNumber} - [{link}].");
                }
            }
            catch (Exception e)
            {
                result.IsSuccessful = false;
                result.FailedLinks.Add(link);

                _clientLogger.LogError(link, e);
                await _logger.LogErrorAsync(link, e);
            }

            linkNumber++;
        }

        result.DownloadedFileCount = fileNameSet.Count;

        return result;
    }

    public void Init()
    {
        SetPath();

        _grabber = new YouTubeGrabber(GrabberServices.Default);
    }

    private async Task<MediaLinkProcessingResult> ProcessLinkAsync(string link)
    {
        var grabber = ExceptionBuilder.GetNotNullOrThrowException(_grabber);
        var url = new Uri(link);
        var options = new GrabOptions(GrabOptionFlags.All);

        var grabResult = await grabber.GrabAsync(url, CancellationToken.None, options);
        var media = grabResult.Resources<GrabbedMedia>()
            .LastOrDefault(x => x.Channels == MediaChannels.Audio);

        media = ExceptionBuilder.GetNotNullOrThrowException(media);

        using var client = new HttpClient();
        using var response = await client.GetAsync(media.ResourceUri);

        await using var originalStream = await response.Content.ReadAsStreamAsync();
        await using var stream = await grabResult.WrapStreamAsync(originalStream);
        var bytes = new byte[int.MaxValue / 10];
        var total = await stream.ReadAsync(bytes, 0, bytes.Length);

        var result = new MediaLinkProcessingResult(grabResult.Title, bytes.Take(total).ToArray());

        return result;
    }

    private void SetPath()
    {
        var rootPath = Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.Personal)) ??
            throw ExceptionBuilder.SourceReturnedNoResult(nameof(Path.GetDirectoryName));

        var assemblyName = Assembly.GetEntryAssembly()?.GetName().Name ??
            throw ExceptionBuilder.SourceReturnedNoResult(nameof(Assembly.GetEntryAssembly));

        _downloadPath = Path.Combine(rootPath, RootFolderName, assemblyName, AudioFileDownloadFolderName);
    }

    [Obsolete("It was used in older implementation.")]
    private static string GetVideoId(string link)
    {
        var match = RegexPool.VideoId().Match(link);
        string? videoId = null;

        if (match is { Success: true, Groups.Count: 4 })
        {
            videoId = match.Groups[2].Value;

            if (string.IsNullOrEmpty(videoId))
            {
                videoId = match.Groups[3].Value;
            }
        }

        if (string.IsNullOrWhiteSpace(videoId))
        {
            throw new Exception($"Video ID was not found.\r\nSource link: [{link}].");
        }

        return videoId;
    }
}