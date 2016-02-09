using System.Linq;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using dotnet_toolbox.api.Controllers;
using dotnet_toolbox.api.Env;
using dotnet_toolbox.api.Query;
using dotnet_toolbox.api.NuspecCrawler;
using dotnet_toolbox.api.DownloadStats;
using dotnet_toolbox.api.Models;

namespace dotnet_toolbox.api.BackgroundWorker
{
    public class BackgroundWorker
    {
        ILogger logger = LoggingConfiguration.CreateLogger<BackgroundWorker>();
        private ConnectionMultiplexer muxer;
        private RealTimerProvider timerProvider;
        IJobQueue jobQueue;

        public BackgroundWorker(ConnectionMultiplexer muxer, RealTimerProvider timerProvider, IJobQueue jobQueue)
        {
            this.muxer = muxer;
            this.timerProvider = timerProvider;
            this.jobQueue = jobQueue;
        }

        public static void Start()
        {
            var muxer = ConnectionMultiplexer.Connect(EnvironmentReader.FromEnvironment().RedisConnectionString);
            var timerProvider = new RealTimerProvider();
            var worker = new BackgroundWorker(muxer, timerProvider, 
                new JobQueueFactory(muxer.GetDatabase(Constants.Redis.PACKAGES_DB))
                .ForQueueName(Constants.Redis.PackageCrawlerJobQueueName));
            worker.Run();
        }

        public void Run()
        {
            logger.LogInformation("Starting background jobs");
            IGetSetQuerier<PackageDetails> querier = new RedisGetSetQuery<PackageDetails>(CreatePackagesDbConnection(), Constants.Redis.PackageKeyForName);
            this.jobQueue.DoTo(q => CategoriesController.KeyPackageNames.Select(n => { q.EnqueueJob(n); return true; }).ToArray());
            new BackgroundJobListener(timerProvider, CreatePackagesDbConnection(), Constants.Redis.PackageCrawlerJobQueueName)
                .ListenWith(new Crawler(querier, new NuspecDownloader()).CrawlProject);
            new BackgroundJobListener(timerProvider, CreatePackagesDbConnection(), Constants.Redis.DownloadStatsCheckerQueue)
                .ListenWith(new DownloadStatsChecker(
                    new RedisGetSetQuery<Package>(CreatePackagesDbConnection(), Constants.Redis.PackageKeyForName), 
                    new RedisGetSetQuery<Stats>(CreatePackagesDbConnection(), Constants.Redis.PackageKeyForName), 
                    new DownloadStatsCheck()).LoadStats);
        }

        private IDatabase CreatePackagesDbConnection()
        {
            return muxer.GetDatabase(Constants.Redis.PACKAGES_DB);
        }
    }

}
