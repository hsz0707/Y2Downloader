using System;
using System.Threading.Tasks;
using Unity;
using Y2Downloader.Common.Interfaces;
using Y2Downloader.Logger.Disc;
using Y2Downloader.Services.Y2Mate;

namespace Y2Downloader
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            try
            {
                var container = new UnityContainer();
                LoggerInitializer.Init(container);
                Y2DownloaderInitializer.Init(container);

                var logger = container.Resolve<ILogger>();
                logger.Init();

                var downloader = container.Resolve<IY2DownloadService>();
                downloader.Init();

                var result = await downloader.DownloadFromLinksAsync(new[]
                    {"https://www.youtube.com/watch?v=8UvZHoypU0s"});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}