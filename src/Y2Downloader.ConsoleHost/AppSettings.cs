namespace Y2Downloader.ConsoleHost;

using Common.Interfaces;

using Microsoft.Extensions.Configuration;

internal class AppSettings : IAppSettings
{
    private const string AppSettingsFileName = "AppSettings.json";

    public int SourceLocationProcessDelay { get; private set; }

    public void Init()
    {
        ReadSettingsFromConfigFile();
    }

    private void ReadSettingsFromConfigFile()
    {
        var builder = new ConfigurationBuilder().AddJsonFile(AppSettingsFileName, false);
        var config = builder.Build();

        SourceLocationProcessDelay = config.GetInt(nameof(SourceLocationProcessDelay));
    }
}