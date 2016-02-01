using System;
using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis;

namespace dotnet_toolbox.api.Query
{
    public class LatestPackagesQuery : ILatestPackagesIndex
    {
        IDatabase redisDatabase;

        public LatestPackagesQuery(IDatabase redisDatabase)
        {
            this.redisDatabase = redisDatabase;
        }

        public IEnumerable<string> Get()
        {
            return this.redisDatabase.SortedSetRangeByRank(
                "RecentPackages", 0, 9, order: Order.Descending).Select(rv => rv.ToString());
        }

        public void Update(long timestamp, string packageName)
        {
            this.redisDatabase.SortedSetAdd("RecentPackages", packageName, timestamp, CommandFlags.None);
        }
    }
}