namespace Y2Downloader.LinkReader.Disc;

using System.Reflection;
using System.Text;

using Common.Helpers;
using Common.Interfaces;

public class DiscLinkReader : ILinkReader
{
    private const string FailedLinksFileName = "FailedLinks.txt";

    private const string PostProcessingFolderName = "SRC_POST";

    private const string RootFolderName = "Downloads";

    private const string SourceFileFilter = "*.txt";

    private const string SourceLocationFolderName = "SRC";

    private string? _failedLinksFile;

    private string? _postProcessingDirectory;

    private string? _sourceLocationDirectory;

    public Task<ISet<string>> GetLinksAsync()
    {
        var links = new HashSet<string>();
        var sourceLocationDirectory = ExceptionBuilder.GetNotEmptyStringOrThrowException(_sourceLocationDirectory);
        var postProcessingDirectory = ExceptionBuilder.GetNotEmptyStringOrThrowException(_postProcessingDirectory);

        if (Directory.Exists(_sourceLocationDirectory))
        {
            if (!Directory.Exists(_postProcessingDirectory))
            {
                Directory.CreateDirectory(postProcessingDirectory);
            }

            var files = Directory.GetFiles(sourceLocationDirectory, SourceFileFilter);

            foreach (var file in files)
            {
                var lines = File.ReadAllLines(file);

                foreach (var link in lines.Where(x => !string.IsNullOrWhiteSpace(x)))
                {
                    links.Add(link);
                }

                var fileName = Path.GetFileName(file);
                var movedFile = Path.Combine(postProcessingDirectory, fileName);
                File.Move(file, movedFile);
            }
        }
        else
        {
            Directory.CreateDirectory(sourceLocationDirectory);
        }

        return Task.FromResult<ISet<string>>(links);
    }

    public void Init()
    {
        SetPath();
    }

    public async Task SaveFailedLinksAsync(ISet<string> links)
    {
        var stringBuilder = new StringBuilder();
        var failedLinksFile = ExceptionBuilder.GetNotEmptyStringOrThrowException(_failedLinksFile);

        foreach (var link in links)
        {
            stringBuilder.AppendLine(link);
        }

        await using var streamWriter = File.AppendText(failedLinksFile);
        await streamWriter.WriteAsync(stringBuilder.ToString());
    }

    private void SetPath()
    {
        var baseRootPath = Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.Personal)) ??
            throw ExceptionBuilder.SourceReturnedNoResult(nameof(Path.GetDirectoryName));

        var rootPath = Path.Combine(baseRootPath, RootFolderName);

        var assemblyName = Assembly.GetEntryAssembly()?.GetName().Name ??
            throw ExceptionBuilder.SourceReturnedNoResult(nameof(Assembly.GetEntryAssembly));

        _sourceLocationDirectory = Path.Combine(rootPath, assemblyName, SourceLocationFolderName);
        _postProcessingDirectory = Path.Combine(rootPath, assemblyName, PostProcessingFolderName);
        _failedLinksFile = Path.Combine(rootPath, assemblyName, FailedLinksFileName);
    }
}