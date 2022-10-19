using Unity;
using Y2Downloader.Common.Interfaces;
using Y2Downloader.Logger.Disc.Services;

namespace Y2Downloader.Logger.Disc
{
    public static class LoggerInitializer
    {
        public static void Init(IUnityContainer container)
        {
            container
               .RegisterSingleton<ILogger, DiscLogger>()
                ;
        }
    }
}