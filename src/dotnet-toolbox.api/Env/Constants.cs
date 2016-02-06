using StackExchange.Redis;

namespace dotnet_toolbox.api.Env
{
    public static class Constants
    {
        public static class Redis
        {
            public static readonly string PackageCrawlerJobQueueName = "PackageCrawlerJob";
            public static readonly int PACKAGES_DB = 1;
            internal static readonly string StartedPackageCrawlerJobQueueName = "PackageCrawlerJob.Started";

            public static RedisKey PackageKeyForName(string name)
            {
                return string.Format("packages:{0}", name.ToLowerInvariant());
            }
        }
    }
}
