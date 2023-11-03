namespace Y2Downloader.Common;

using System.Text.RegularExpressions;

public static partial class RegexPool
{
    [GeneratedRegex(@"^.*watch\?v=((.+?)\&.*|(.*))$")]
    public static partial Regex VideoId();

    [GeneratedRegex(@"[^a-zA-Z0-9_. \(\)\[\]-]+")]
    public static partial Regex SpecSymbol();
}