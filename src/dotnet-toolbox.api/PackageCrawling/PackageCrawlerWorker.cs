using System;
using System.Threading;
using StackExchange.Redis;
using dotnet_toolbox.api.Env;
using Microsoft.Extensions.Logging;
using dotnet_toolbox.api.Query;
using dotnet_toolbox.api.Models;

namespace dotnet_toolbox.api.PackageCrawling
{
    public class PackageCrawlerWorker
    {
        ILogger logger = LoggingConfiguration.CreateLogger<PackageCrawlerWorker>();
        private ConnectionMultiplexer muxer;
        private RealTimerProvider timerProvider;

        public PackageCrawlerWorker(ConnectionMultiplexer muxer, RealTimerProvider timerProvider)
        {
            this.muxer = muxer;
            this.timerProvider = timerProvider;
        }

        public static void Start()
        {
            var muxer = ConnectionMultiplexer.Connect(EnvironmentReader.FromEnvironment().RedisConnectionString);
            var timerProvider = new RealTimerProvider();
            var worker = new PackageCrawlerWorker(muxer, timerProvider);
            worker.Run();
        }

        public void Run()
        {
            logger.LogInformation("Starting crawler");
            var querier = new RedisGetSetQuery<INuspecPackageInfo>(CreatePackagesDbConnection(), Constants.Redis.PackageKeyForName);
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
