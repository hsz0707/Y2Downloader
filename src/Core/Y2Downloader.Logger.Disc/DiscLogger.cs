namespace Y2Downloader.Logger.Disc;

using System.Globalization;
using System.Reflection;
using System.Text;

using Common.Helpers;
using Common.Interfaces;

public class DiscLogger : ILogger
{
    private const string ErrorLogFileName = "err.log";

    private const string RootFolderName = "Downloads";

    private string? _logDirectory;

    private string? _logFullPath;

    public void Init()
    {
        SetPath();
    }

    public async Task LogErrorAsync(string title, Exception e)
    {
        var logFullPath = ExceptionBuilder.GetNotEmptyStringOrThrowException(_logFullPath);
        var directory = Path.GetDirectoryName(logFullPath) ??
            throw ExceptionBuilder.SourceReturnedNoResult(nameof(Path.GetDirectoryName));

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await using var streamWriter = File.AppendText(logFullPath);

        var splitter = new string('-', 100);
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine(DateTime.Now.ToString(CultureInfo.InvariantCulture));
        stringBuilder.AppendLine(title);
        stringBuilder.AppendLine(splitter);
        stringBuilder.AppendLine(e.ToString());
        stringBuilder.AppendLine(splitter);
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();

        await streamWriter.WriteAsync(stringBuilder.ToString());
    }

    private void SetPath()
    {
        var rootPath = Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.Personal)) ??
            throw ExceptionBuilder.SourceReturnedNoResult(nameof(Path.GetDirectoryName));

        var assemblyName = Assembly.GetEntryAssembly()?.GetName().Name ??
            throw ExceptionBuilder.SourceReturnedNoResult(nameof(Assembly.GetEntryAssembly));

        _logDirectory = Path.Combine(rootPath, RootFolderName, assemblyName);
        _logFullPath = Path.Combine(_logDirectory, ErrorLogFileName);
    }
}