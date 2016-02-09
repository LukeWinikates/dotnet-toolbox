using System;
using dotnet_toolbox.api.Env;
using dotnet_toolbox.api.NuspecCrawler;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace dotnet_toolbox.api.BackgroundWorker
{
    public class BackgroundJobListener
    {
        ILogger logger = LoggingConfiguration.CreateLogger<BackgroundJobListener>();
        ICrawler crawler;
        ITimerProvider timerProvider;
        IDatabase redisDatabase;
        System.Threading.Timer timer;
        string queueName;
        private Action<string> work;
        string startedQueueName;

        public BackgroundJobListener(ITimerProvider timerProvider, IDatabase redisDatabase, string queueName)
        {
            this.queueName = queueName;
            this.startedQueueName = queueName + ".Started";
            this.timerProvider = timerProvider;
            this.redisDatabase = redisDatabase;
        }

        public void ListenWith(Action<string> work)
        {
            this.work = work;
            this.timer = timerProvider.StartWithCallback(DrainQueue);
        }

        private void DrainQueue()
        {
            string nextPackage = redisDatabase.ListRightPopLeftPush(this.queueName, this.startedQueueName);
            while (nextPackage != null)
            {
                try
                {
                    logger.LogInformation("{0}: New Job, {1}", this.queueName, nextPackage);
                    work(nextPackage);
                    redisDatabase.ListRemove(this.startedQueueName, nextPackage);
                    logger.LogInformation("{0}: Completed Job, {1}", this.queueName, nextPackage);
                }
                catch (Exception e)
                {
                    logger.LogError("{0} failed on job: {1} due to error: {2}", this.queueName, nextPackage, e);
                }
                nextPackage = redisDatabase.ListRightPopLeftPush(
                    this.queueName, 
                    this.startedQueueName);
            }
        }
    }
}