
using StackExchange.Redis;

namespace dotnet_toolbox.api.Nuget
{
    public class PackageCrawlerJobQueue : IPackageCrawlerJobQueue
    {
        public static readonly string RoutingKey = "PackageCrawlerJob";
        IDatabase database;

        public PackageCrawlerJobQueue(IDatabase redisDatabase)
        {
            this.database = redisDatabase;
        }

        public void EnqueueJob(string name)
        {
            database.ListRightPush(RoutingKey, name);
        }
    }
}