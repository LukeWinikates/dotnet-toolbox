using System;
using System.Threading;
using StackExchange.Redis;
using dotnet_toolbox.api.Env;

namespace dotnet_toolbox.api.PackageCrawling
{
    public class PackageCrawlerWorker
    {
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
            Console.WriteLine("DB Connection Successful");
            var timerProvider = new RealTimerProvider();
            var worker = new PackageCrawlerWorker(muxer, timerProvider);
            worker.Run();
        }

        public void Run()
        {
            Console.WriteLine("Starting crawler");
            new PackageCrawlerJobListener(timerProvider, CreatePackagesDbConnection(), new Crawler(CreatePackagesDbConnection(), new NuspecDownload())).Listen();
        }

        private IDatabase CreatePackagesDbConnection()
        {
            return muxer.GetDatabase(Constants.Redis.PACKAGES_DB);
        }
    }

    public class RealTimerProvider : ITimerProvider
    {
        public Timer StartWithCallback(Action action)
        {
            var timer = new Timer(_ =>
            {
                Console.WriteLine("Timer triggered");
                action();
                Console.WriteLine("Timer callback completed");
            }, null, 1000, 2000);
            return timer;
        }
    }
}
