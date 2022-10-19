using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Y2Downloader.Common.Interfaces;

namespace Y2Downloader.Logger.Disc.Services
{
    internal class DiscLogger : ILogger
    {
        private string _logDirectory;
        private string _logFullPath;

        public void Init()
        {
            SetLogPath();
        }

        public async Task LogErrorAsync(Exception e)
        {
            var directory = Path.GetDirectoryName(_logFullPath);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using var streamWriter = File.AppendText(_logFullPath);

            var splitter = new string('-', 100);
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(DateTime.Now.ToString());
            stringBuilder.AppendLine(splitter);
            stringBuilder.AppendLine(e.ToString());
            stringBuilder.AppendLine(splitter);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();

            await streamWriter.WriteAsync(stringBuilder.ToString());
        }

        private void SetLogPath()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var assemblyName = Assembly.GetEntryAssembly().GetName().Name;
            var fileName = "err.log";

            _logDirectory = Path.Combine(documentsPath, assemblyName);
            _logFullPath = Path.Combine(_logDirectory, fileName);
        }
    }
}