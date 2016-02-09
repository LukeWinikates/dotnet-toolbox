using System;
using dotnet_toolbox.api.Env;
using dotnet_toolbox.api.NuspecCrawler;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace dotnet_toolbox.api.BackgroundWorker
{
    public class PackageCrawlerJobListener
    {
        ILogger logger = LoggingConfiguration.CreateLogger<PackageCrawlerJobListener>();
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
                    logger.LogInformation("Crawling Package: {0}", nextPackage);
                    crawler.CrawlProject(nextPackage);
                    redisDatabase.ListRemove(Constants.Redis.StartedPackageCrawlerJobQueueName, nextPackage);
                    logger.LogInformation("Package Loaded: {0}", nextPackage);
                }
                catch (Exception e)
                {
                    logger.LogError("Crawling Failed for package: {0} due to error: {1}", nextPackage, e);
                }
                nextPackage = redisDatabase.ListRightPopLeftPush(Constants.Redis.PackageCrawlerJobQueueName, Constants.Redis.StartedPackageCrawlerJobQueueName);
            }
        }
    }
}