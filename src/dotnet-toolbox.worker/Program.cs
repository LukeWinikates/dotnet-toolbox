using System;
using System.Threading;
using StackExchange.Redis;
using dotnet_toolbox.worker.PackageCrawler;
using dotnet_toolbox.worker.PackageCrawling;
using dotnet_toolbox.common.Env;
using dotnet_toolbox.common;

namespace dotnet_toolbox.worker
{
    public class Program
    {
        private ConnectionMultiplexer muxer;
        private RealTimerProvider timerProvider;

        public Program(ConnectionMultiplexer muxer, RealTimerProvider timerProvider)
        {
            this.muxer = muxer;
            this.timerProvider = timerProvider;
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("Starting Background Worker Process");
            var muxer = ConnectionMultiplexer.Connect(EnvironmentReader.FromEnvironment().RedisConnectionString);          
            Console.WriteLine("DB Connection Successful");
            var timerProvider = new RealTimerProvider();
            new Program(muxer, timerProvider).Run();
        }
        
        public void Run()
        {
            Console.WriteLine("Starting crawler");
            new PackageCrawlerJobListener(timerProvider, CreatePackagesDbConnection(), new Crawler(CreatePackagesDbConnection(), new NuspecDownload())).Listen();
            Console.Read(); // block forever            
        }

        private IDatabase CreatePackagesDbConnection()
        {
            return muxer.GetDatabase(Constants.Redis.PACKAGES_DB);
        }
    }

    public class RealTimerProvider : ITimerProvider
    {
        public void StartWithCallback(Action action)
        {
            var timer = new Timer(_ => {
                Console.WriteLine("Timer triggered");
                action();
                Console.WriteLine("Timer callback completed");
            }, null, 1000, 2000);
        }
    }
}
