using System.Linq;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using dotnet_toolbox.api.Controllers;
using dotnet_toolbox.api.Env;
using dotnet_toolbox.api.Nuget;
using dotnet_toolbox.api.Query;
using dotnet_toolbox.api.NuspecCrawler;

namespace dotnet_toolbox.api.BackgroundWorker
{
    public class PackageCrawlerWorker
    {
        ILogger logger = LoggingConfiguration.CreateLogger<PackageCrawlerWorker>();
        private ConnectionMultiplexer muxer;
        private RealTimerProvider timerProvider;
        IPackageCrawlerJobQueue jobQueue;

        public PackageCrawlerWorker(ConnectionMultiplexer muxer, RealTimerProvider timerProvider, IPackageCrawlerJobQueue jobQueue)
        {
            this.muxer = muxer;
            this.timerProvider = timerProvider;
            this.jobQueue = jobQueue;
        }

        public static void Start()
        {
            var muxer = ConnectionMultiplexer.Connect(EnvironmentReader.FromEnvironment().RedisConnectionString);
            var timerProvider = new RealTimerProvider();
            var worker = new PackageCrawlerWorker(muxer, timerProvider, new PackageCrawlerJobQueue(muxer.GetDatabase(Constants.Redis.PACKAGES_DB)));
            worker.Run();
        }

        public void Run()
        {
            logger.LogInformation("Starting background jobs");
            IGetSetQuerier<PackageDetails> querier = new RedisGetSetQuery<PackageDetails>(CreatePackagesDbConnection(), Constants.Redis.PackageKeyForName);
            this.jobQueue.DoTo(q => CategoriesController.KeyPackageNames.Select(n => { q.EnqueueJob(n); return true; }).ToArray());
            new BackgroundJobListener(timerProvider, CreatePackagesDbConnection(), Constants.Redis.PackageCrawlerJobQueueName)
                .ListenWith(new Crawler(querier, new NuspecDownloader()).CrawlProject);
        }

        private IDatabase CreatePackagesDbConnection()
        {
            return muxer.GetDatabase(Constants.Redis.PACKAGES_DB);
        }
    }

}
