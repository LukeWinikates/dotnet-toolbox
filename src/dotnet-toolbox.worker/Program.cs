using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting Background Worker Process");
            var muxer = ConnectionMultiplexer.Connect(EnvironmentReader.FromEnvironment().RedisConnectionString);
            var db = muxer.GetDatabase(Constants.Redis.PACKAGES_DB);
            Console.WriteLine("DB Connection Successful");
            var timerProvider = new RealTimerProvider();
            Console.WriteLine("Starting crawler");            
            new PackageCrawlerJobListener(timerProvider, db, new Crawler(db, new NuspecDownload())).Listen();
            Console.Read(); // block forever
        }
    }

    public class RealTimerProvider : ITimerProvider
    {
        public void StartWithCallback(Action action)
        {
            var timer = new Timer(_ => action(), null, 1000, 1000);
        }
    }
}
