using System;
using StackExchange.Redis;

namespace dotnet_toolbox.worker.PackageCrawler
{
    public class PackageCrawlerJobListener {
        ICrawler crawler;
        ITimerProvider timerProvider;
        IDatabase redisDatabase;

        public PackageCrawlerJobListener(ITimerProvider timerProvider, IDatabase redisDatabase, ICrawler crawler) {
            this.crawler = crawler;
            this.timerProvider = timerProvider;
            this.redisDatabase = redisDatabase;
        }

        public void Listen()
        {
            timerProvider.StartWithCallback(DrainQueue);
        }

        private void DrainQueue()
        {
            string nextPackage = redisDatabase.ListLeftPop("PackageCrawlerJob");
            while(nextPackage != null) {
                Console.WriteLine("Crawling Package: {0}", nextPackage);
                crawler.CrawlProject(nextPackage);
                Console.WriteLine("Package Loaded: {0}", nextPackage);
                nextPackage = redisDatabase.ListLeftPop("PackageCrawlerJob");
            }
        }
    }
}