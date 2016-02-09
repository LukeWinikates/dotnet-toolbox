using System;
using dotnet_toolbox.api.Models;
using dotnet_toolbox.api.Query;
using StackExchange.Redis;

namespace dotnet_toolbox.api.DownloadStats
{
    public class Stats : IRedisHashable, IStats
    {
        public int? TotalDownloads { get; set; }

        public HashEntry[] AsRedisHash()
        {
            return new[] { new HashEntry("TotalDownloads", TotalDownloads) };
        }

        public void FromRedisHash(HashEntry[] entries)
        {
            throw new NotImplementedException();
        }
    }
}