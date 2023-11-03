namespace Y2Downloader.IoC.MicrosoftDi;

using Common.Helpers;
using Common.Interfaces;

using LinkReader.Disc;

using Logger.Disc;

using Microsoft.Extensions.DependencyInjection;

using Mp3Downloader.Experimental;

public class MicrosoftDiManager : IIoCManager
{
    private ServiceProvider? _serviceProvider;

    public IY2DownloaderApp GetApp()
    {
        var app = _serviceProvider?.GetService<IY2DownloaderApp>();

        return ExceptionBuilder.GetNotNullOrThrowException(app);
    }

    public void RegisterTypes<TSettings, TLogger>()
        where TSettings : class, IAppSettings where TLogger : class, IClientLogger
    {
        var serviceCollection = new ServiceCollection();

        _serviceProvider = serviceCollection
            .AddSingleton<IAppSettings, TSettings>()
            .AddSingleton<IClientLogger, TLogger>()
            .AddSingleton<ILogger, DiscLogger>()
            .AddSingleton<ILinkReader, DiscLinkReader>()
            .AddSingleton<IY2Downloader, Y2MateDownloader>()
            .AddSingleton<IY2DownloaderApp, Y2DownloaderApp>()
            .BuildServiceProvider();
    }
}