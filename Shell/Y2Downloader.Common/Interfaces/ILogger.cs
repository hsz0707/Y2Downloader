using System;
using System.Threading.Tasks;

namespace Y2Downloader.Common.Interfaces
{
    public interface ILogger
    {
        void Init();
        Task LogErrorAsync(Exception e);
    }
}