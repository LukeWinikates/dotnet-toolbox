using dotnet_toolbox.api.Query;

namespace dotnet_toolbox.api.NuspecCrawler
{
    public class Crawler : ICrawler
    {
        private IGetSetQuerier<PackageDetails> setQuery;
        INuspecDownloader downloader;

        public Crawler(IGetSetQuerier<PackageDetails> setQuery, INuspecDownloader downloader)
        {
            this.setQuery = setQuery;
            this.downloader = downloader;
        }

        public void CrawlProject(string name)
        {
            this.downloader.Download(name)
                .Pipe(new NuspecParser().Parse)
                .DoTo(r => this.setQuery.Set(name, r));
        }
    }
    
    public interface ICrawler
    {
        void CrawlProject(string name);
    }
}
