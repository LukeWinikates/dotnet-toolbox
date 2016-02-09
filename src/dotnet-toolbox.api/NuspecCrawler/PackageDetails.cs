
using dotnet_toolbox.api.Models;
using dotnet_toolbox.api.Query;
using StackExchange.Redis;

namespace dotnet_toolbox.api.NuspecCrawler
{
    public class PackageDetails : INuspecPackageInfo, IRedisHashable
    {
        public string Description { get; set; }
        public string Owners { get; set; }
        public string Id { get; set; }
        public string Version { get; set; }

        public HashEntry[] AsRedisHash() => new HashConverter(this).AsRedisHash();

        public void FromRedisHash(HashEntry[] entries) => new HashConverter(this).FromRedisHash(entries);
    }
}