namespace Y2Downloader.Common.Helpers;

using System.Runtime.CompilerServices;

public static class ExceptionBuilder
{
    public static Exception ConfigSectionNotParsed(string configSectionName)
    {
        return new InvalidOperationException($"Config section was not parsed: [{configSectionName}].");
    }

    public static string GetNotEmptyStringOrThrowException(string? str, [CallerMemberName] string callerName = "")
    {
        return string.IsNullOrWhiteSpace(str)
            ? throw new InvalidOperationException($"Empty or whitespace string was provided in [{callerName}]")
            : str;
    }

    public static T GetNotNullOrThrowException<T>(T? obj, [CallerMemberName] string callerName = "")
        where T : notnull
    {
        return obj ?? throw new InvalidOperationException($"Null object was provided in [{callerName}]");
    }

    public static Exception SourceReturnedNoResult(string source)
    {
        return new InvalidOperationException($"[{source}] returned no data.");
    }
}