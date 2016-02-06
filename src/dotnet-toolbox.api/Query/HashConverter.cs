using System.Linq;
using dotnet_toolbox.api.Models;
using StackExchange.Redis;

namespace dotnet_toolbox.api.Query
{
    public class HashConverter
    {
        INuspecPackageInfo packageInfo;
        public HashConverter(INuspecPackageInfo packageInfo)
        {
            this.packageInfo = packageInfo;
        }

        public HashEntry[] AsRedisHash()
        {
            return new[] {
                new HashEntry("Id", this.packageInfo.Id),
                new HashEntry("Owners", this.packageInfo.Owners),
                new HashEntry("Version", this.packageInfo.Version),
                new HashEntry("Description", this.packageInfo.Description),
            }.Where(e => !e.Value.IsNull).ToArray();
        }

        public void FromRedisHash(HashEntry[] entries)
        {
            packageInfo.Id = entries.ValueFor("Id");
            packageInfo.Owners = entries.ValueFor("Owners");
            packageInfo.Version = entries.ValueFor("Version");
            packageInfo.Description = entries.ValueFor("Description");
        }
    }
}