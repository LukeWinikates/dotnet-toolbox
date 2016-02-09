using Xunit;
using dotnet_toolbox.api.DownloadStats;

namespace dotnet_toolbox.api.tests.DownloadStats
{
    public class DownloadStatsCheckTest
    {
        [Fact]
        public void Download_GetsStatsFromNugetApi()
        {
            var check = new DownloadStatsCheck();
            var stats = check.Download("Nancy", "1.4.3");
            Assert.True(stats.TotalDownloads >= 382533);
        }
    }
}