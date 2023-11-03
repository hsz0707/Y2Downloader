namespace Y2Downloader.ConsoleHost;

using Common.Helpers;

using Microsoft.Extensions.Configuration;

internal static class ConfigHelper
{
    public static int GetInt(this IConfigurationRoot root, string sectionName)
    {
        if (int.TryParse(root.GetSection(sectionName).Value, out var val))
        {
            return val;
        }

        throw ExceptionBuilder.ConfigSectionNotParsed(sectionName);
    }
}