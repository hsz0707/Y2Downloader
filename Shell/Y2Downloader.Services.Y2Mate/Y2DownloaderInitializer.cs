using Unity;
using Y2Downloader.Common.Interfaces;
using Y2Downloader.Services.Y2Mate.Services;

namespace Y2Downloader.Services.Y2Mate
{
    public static class Y2DownloaderInitializer
    {
        public static void Init(IUnityContainer container)
        {
            container
               .RegisterSingleton<IY2DownloadService, Y2DownloadService>()
                ;
        }
    }
}