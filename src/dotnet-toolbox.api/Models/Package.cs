using System.Linq;
using dotnet_toolbox.api.Query;
using StackExchange.Redis;

namespace dotnet_toolbox.api.Models
{
    public class Package : INuspecPackageInfo, IRedisHashable, IStats
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Owners { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }

        public int? TotalDownloads { get; set; }

        public HashEntry[] AsRedisHash() => new HashConverter(this).AsRedisHash().Concat(
            new[] { new HashEntry("Name", Name), new HashEntry("TotalDownloads", TotalDownloads) }).Where(e => !e.Value.IsNull).ToArray();

        public void FromRedisHash(HashEntry[] entries) => this.DoTo(p => p.Name = entries.ValueFor("Name"), p => new HashConverter(this).FromRedisHash(entries));
    }
}