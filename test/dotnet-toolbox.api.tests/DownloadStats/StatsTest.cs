using dotnet_toolbox.api.DownloadStats;
using dotnet_toolbox.api.Query;
using Xunit;

namespace dotnet_toolbox.api.tests.DownloadStats
{
    public class StatsTest
    {
        [Fact]
        public void AsRedisHash_IncludesTotalDownloads()
        {
            var fields = new Stats { TotalDownloads = 100 * 1000 }.AsRedisHash();
            Assert.Single(fields);
            Assert.Equal(100000, fields.ValueFor("TotalDownloads"));
        }
    }
}