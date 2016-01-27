using System;
using dotnet_toolbox.api.Env;
using StackExchange.Redis;

namespace dotnet_toolbox.api.PackageCrawling
{
    public class PackageCrawlerJobListener
    {
        ICrawler crawler;
        ITimerProvider timerProvider;
        IDatabase redisDatabase;
        System.Threading.Timer timer;

        public PackageCrawlerJobListener(ITimerProvider timerProvider, IDatabase redisDatabase, ICrawler crawler)
        {
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
            string nextPackage = redisDatabase.ListRightPopLeftPush(Constants.Redis.PackageCrawlerJobQueueName, Constants.Redis.StartedPackageCrawlerJobQueueName);
            while (nextPackage != null)
            {
                try
                {
                    Console.WriteLine("Crawling Package: {0}", nextPackage);
                    crawler.CrawlProject(nextPackage);
                    redisDatabase.ListRemove(Constants.Redis.StartedPackageCrawlerJobQueueName, nextPackage);
                    Console.WriteLine("Package Loaded: {0}", nextPackage);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Crawling Failed for package: {0} due to error: {1}", nextPackage, e);
                }
                nextPackage = redisDatabase.ListRightPopLeftPush(Constants.Redis.PackageCrawlerJobQueueName, Constants.Redis.StartedPackageCrawlerJobQueueName);
            }
        }
    }
}