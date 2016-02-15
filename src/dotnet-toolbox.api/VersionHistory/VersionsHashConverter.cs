using System.Collections.Generic;
using System.Linq;
using dotnet_toolbox.api.Models;
using dotnet_toolbox.api.Query;
using StackExchange.Redis;

namespace dotnet_toolbox.api.VersionHistory
{
    public class VersionsHashConverter
    {
        private const string VERSIONS_COUNT = "Versions.Count";
        private IVersionsList versions;

        public VersionsHashConverter(IVersionsList versions)
        {
            this.versions = versions;
        }

        public HashEntry[] AsRedisHash()
        {
            if(this.versions.Versions == null) {
                return new HashEntry[]{};
            }
            var entries = new List<HashEntry>();
            var i = 0;
            entries.Add(new HashEntry(VERSIONS_COUNT, this.versions.Versions.Count));
            foreach (var v in this.versions.Versions)
            {
                entries.Add(new HashEntry(VersionNumberKey(i), v.VersionNumber));
                entries.Add(new HashEntry(TimestampKey(i), v.Timestamp));
                i++;
            }
            return entries.ToArray();
        }

        public void FromRedisHash(HashEntry[] redisHash)
        {
            var count = redisHash.IntValueFor(VERSIONS_COUNT);
            if(!count.HasValue) {
                return;
            }
            this.versions.Versions = Enumerable.Range(0, count.Value).Select(i =>
                 new Models.Version
                 {
                     VersionNumber = redisHash.ValueFor(VersionNumberKey(i)),
                     Timestamp = redisHash.ValueFor(TimestampKey(i))
                 }).ToList();
        }

        private string TimestampKey(int i) => KeyBase(i) + ".Timestamp";

        private string VersionNumberKey(int i) => KeyBase(i) + ".VersionNumber";

        private string KeyBase(int index) => string.Format("Versions[{0}]", index);
    }
}