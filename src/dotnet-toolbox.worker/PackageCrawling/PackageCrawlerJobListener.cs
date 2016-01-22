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
            string val = redisDatabase.ListLeftPop("PackageCrawlerJob");
            while(val != null) {
                crawler.CrawlProject(val);
                Console.WriteLine("Crawling Package: {0}", val);
                val = redisDatabase.ListLeftPop("PackageCrawlerJob");
            }
        }
    }
}