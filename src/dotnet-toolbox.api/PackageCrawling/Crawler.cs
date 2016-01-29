using dotnet_toolbox.api.Models;
using dotnet_toolbox.api.Query;

namespace dotnet_toolbox.api.PackageCrawling
{
    public class Crawler : ICrawler
    {
        private IGetSetQuerier<INuspecPackageInfo> setQuery;
        private INuspecDownload download;

        public Crawler(IGetSetQuerier<INuspecPackageInfo> setQuery, INuspecDownload download)
        {
            this.setQuery = setQuery;
            this.download = download;
        }

        public void CrawlProject(string name)
        {
            var details = this.download.Download(name);
            this.setQuery.Set(name, details);
        }
    }
}
