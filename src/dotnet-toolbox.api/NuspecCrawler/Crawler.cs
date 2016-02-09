using dotnet_toolbox.api.BackgroundWorker;
using dotnet_toolbox.api.Query;

namespace dotnet_toolbox.api.NuspecCrawler
{
    public class Crawler : ICrawler
    {
        private IGetSetQuerier<PackageDetails> setQuery;
        INuspecDownloader downloader;
        private IJobQueue downloadStatsJobQueue;

        public Crawler(IGetSetQuerier<PackageDetails> setQuery, INuspecDownloader downloader, IJobQueue downloadStatsJobQueue)
        {
            this.setQuery = setQuery;
            this.downloader = downloader;
            this.downloadStatsJobQueue = downloadStatsJobQueue;
            
        }
        public void CrawlProject(string name)
        {
            this.downloader.Download(name)
                .Pipe(new NuspecParser().Parse)
                .DoTo(r => this.setQuery.Set(name, r), r => downloadStatsJobQueue.EnqueueJob(name));
        }
    }
    
    public interface ICrawler
    {
        void CrawlProject(string name);
    }
}
