using System;
using dotnet_toolbox.api.Env;
using StackExchange.Redis;

namespace dotnet_toolbox.api.PackageCrawling
{
    public class PackageCrawlerJobListener {
        ICrawler crawler;
        ITimerProvider timerProvider;
        IDatabase redisDatabase;
        System.Threading.Timer timer;

        public PackageCrawlerJobListener(ITimerProvider timerProvider, IDatabase redisDatabase, ICrawler crawler) {
            this.crawler = crawler;
            this.timerProvider = timerProvider;
            this.redisDatabase = redisDatabase;
        }

        public void Listen()
        {
            this.timer = timerProvider.StartWithCallback(DrainQueue);
        }

        private void DrainQueue()
        {
            string nextPackage = redisDatabase.ListLeftPop(Constants.Redis.PackageCrawlerJobQueueName);
            while(nextPackage != null) {
                Console.WriteLine("Crawling Package: {0}", nextPackage);
                crawler.CrawlProject(nextPackage);
                Console.WriteLine("Package Loaded: {0}", nextPackage);
                nextPackage = redisDatabase.ListLeftPop(Constants.Redis.PackageCrawlerJobQueueName);
            }
        }
    }
}