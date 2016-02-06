using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using dotnet_toolbox.api.Env;
using dotnet_toolbox.api.Query;
using dotnet_toolbox.api.Nuget;
using dotnet_toolbox.api.Controllers;
using System.Linq;

namespace dotnet_toolbox.api.PackageCrawling
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
            logger.LogInformation("Starting crawler");
            IGetSetQuerier<NuspecParser.PackageDetails> querier = new RedisGetSetQuery<NuspecParser.PackageDetails>(CreatePackagesDbConnection(), Constants.Redis.PackageKeyForName);
            this.jobQueue.DoTo(q => CategoriesController.KeyPackageNames.Select(n => { q.EnqueueJob(n); return true; }).ToArray());
            new PackageCrawlerJobListener(timerProvider, CreatePackagesDbConnection(), new Crawler(querier, new NuspecDownload())).Listen();
        }

        private IDatabase CreatePackagesDbConnection()
        {
            return muxer.GetDatabase(Constants.Redis.PACKAGES_DB);
        }
    }

    public class RealTimerProvider : ITimerProvider
    {
        ILogger logger = LoggingConfiguration.CreateLogger<RealTimerProvider>();
        public Timer StartWithCallback(Action action)
        {
            var timer = new Timer(_ =>
            {
                logger.LogDebug("Timer triggered");
                action();
                logger.LogDebug("Timer callback completed");
            }, null, 1000, 2000);
            return timer;
        }
    }
}
