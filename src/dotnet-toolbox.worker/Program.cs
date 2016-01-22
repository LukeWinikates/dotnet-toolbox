using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using StackExchange.Redis;
using dotnet_toolbox.worker.PackageCrawler;
using dotnet_toolbox.worker.PackageCrawling;

namespace dotnet_toolbox.worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var muxer = ConnectionMultiplexer.Connect("localhost"); // TODO config value
            var db = muxer.GetDatabase(1); // TODO constants
            var timerProvider = new RealTimerProvider();            
            new PackageCrawlerJobListener(timerProvider, db, new Crawler());
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
