using dotnet_toolbox.api.Env;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace dotnet_toolbox.api.PackageCrawling
{
    public class Crawler : ICrawler
    {
        private IDatabase redisDatabase;
        private INuspecDownload download;

        public Crawler(IDatabase redisDatabase, INuspecDownload download)
        {
            this.redisDatabase = redisDatabase;
            this.download = download;
        }

        public void CrawlProject(string name)
        {
            var details = this.download.Download(name);
            this.redisDatabase.StringSet(Constants.Redis.PackageKeyForName(name), JsonConvert.SerializeObject(details));
        }
    }
}
