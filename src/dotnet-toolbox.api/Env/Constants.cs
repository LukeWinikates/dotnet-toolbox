using StackExchange.Redis;

namespace dotnet_toolbox.api.Env
{
    public static class Constants
    {
        public static class Redis
        {
            public static readonly string PackageCrawlerJobQueueName = "PackageCrawlerJob";
            public static readonly string DownloadStatsCheckerQueue = "DownloadStatsCheckerJob";
            public static readonly string PackageVersionsCrawlerJob = "PackageVersionsCrawlerJob";
            public static readonly int PACKAGES_DB = 1;

            public static RedisKey PackageKeyForName(string name)
            {
                return string.Format("packages:{0}", name.ToLowerInvariant());
            }
        }
    }
}
