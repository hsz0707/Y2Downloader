using System.Collections.Generic;
using System.Threading.Tasks;
using Y2Downloader.Common.Items;

namespace Y2Downloader.Common.Interfaces
{
    public interface IY2DownloadService
    {
        Task<Y2DownloadResult> DownloadFromLinksAsync(IEnumerable<string> links);

        void Init();
    }
}