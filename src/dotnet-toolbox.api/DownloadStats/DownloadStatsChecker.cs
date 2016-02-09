using dotnet_toolbox.api.Models;
using dotnet_toolbox.api.Query;

namespace dotnet_toolbox.api.DownloadStats
{
    public class DownloadStatsChecker
    {
        private IDownloadStatsCheck downloadStatsCheck;
        private IGetSetQuerier<Stats> statsQuerier;
        private IGetSetQuerier<Package> packagesQuerier;

        public DownloadStatsChecker(IGetSetQuerier<Package> packageQuerier, IGetSetQuerier<Stats> statsQuerier, IDownloadStatsCheck downloadStatsCheck)
        {
            this.downloadStatsCheck = downloadStatsCheck;
            this.packagesQuerier = packageQuerier;
            this.statsQuerier = statsQuerier;
        }

        public void LoadStats(string packageName)
        {
            var version = packagesQuerier.Get(packageName).Version;
            downloadStatsCheck.Download(packageName, version)
                .DoTo(s => statsQuerier.Set(packageName, s));
        }
    }
}