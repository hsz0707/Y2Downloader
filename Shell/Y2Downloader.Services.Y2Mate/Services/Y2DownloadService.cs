using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Y2Downloader.Common.Interfaces;
using Y2Downloader.Common.Items;
using Y2Sharp.Youtube;

namespace Y2Downloader.Services.Y2Mate.Services
{
    internal class Y2DownloadService : IY2DownloadService
    {
        private static string _downloadPath;
        private readonly Regex _fileIdRegex = new Regex(@"^.*watch\?v=((.*)\&.*|(.*))$");
        private readonly ILogger _logger;

        public Y2DownloadService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<Y2DownloadResult> DownloadFromLinksAsync(IEnumerable<string> links)
        {
            var result = new Y2DownloadResult();

            try
            {
                foreach (var link in links)
                {
                    try
                    {
                        var match = _fileIdRegex.Match(link);
                        string videoId = null;

                        if (match.Success)
                        {
                            if (match.Groups.Count == 4)
                            {
                                videoId = match.Groups[2].Value;

                                if (string.IsNullOrEmpty(videoId))
                                {
                                    videoId = match.Groups[3].Value;
                                }
                            }
                        }

                        if (string.IsNullOrWhiteSpace(videoId))
                        {
                            await _logger.LogErrorAsync(
                                new Exception($"Video ID was not found.\r\nSource link: [{link}]."));

                            throw new NotImplementedException();
                        }

                        await Video.GetInfo(videoId);
                        var video = new Video();

                        if (!Directory.Exists(_downloadPath))
                        {
                            Directory.CreateDirectory(_downloadPath);
                        }

                        await video.DownloadAsync(Path.Combine(_downloadPath, $"{video.Title}.mp3"), "mp3", "320");
                    }
                    catch (Exception e)
                    {
                        await _logger.LogErrorAsync(e);
                    }
                }
            }
            catch (Exception e)
            {
                await _logger.LogErrorAsync(e);
            }

            return result;
        }

        public void Init()
        {
            SetDownloadPath();
        }

        private static void SetDownloadPath()
        {
            var rootPath = Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
            var assemblyName = Assembly.GetEntryAssembly().GetName().Name;

            _downloadPath = Path.Combine(rootPath, "Downloads", assemblyName);
        }
    }
}