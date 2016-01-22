
using dotnet_toolbox.common;
using StackExchange.Redis;

namespace dotnet_toolbox.api.Nuget
{
    public class PackageCrawlerJobQueue : IPackageCrawlerJobQueue
    {
        public static readonly string QueueName = Constants.Redis.PackageCrawlerJobQueueName;
        IDatabase database;

        public PackageCrawlerJobQueue(IDatabase redisDatabase)
        {
            this.database = redisDatabase;
        }

        public void EnqueueJob(string name)
        {
            database.ListRightPush(QueueName, name);
        }
    }
}