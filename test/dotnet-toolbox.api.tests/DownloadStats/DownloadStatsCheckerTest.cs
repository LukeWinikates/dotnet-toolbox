using dotnet_toolbox.api.DownloadStats;
using dotnet_toolbox.api.Models;
using dotnet_toolbox.api.Query;
using Moq;
using Xunit;

namespace dotnet_toolbox.api.tests.DownloadStats
{
    public class DownloadStatsCheckerTest
    {
        [Fact]
        public void CrawlProject_GetsNuspecFromNugetAndStoresContents()
        {
            var mockDownloadStatsCheck = new Mock<IDownloadStatsCheck>();
            var mockStatsQuerier = new Mock<IGetSetQuerier<Stats>>();
            var mockPackageQuerier = new Mock<IGetSetQuerier<Package>>();

            mockPackageQuerier.Setup(m => m.Get("Nancy")).Returns(new Package { Version = "1.4.3" });
            mockDownloadStatsCheck.Setup(m => m.Download("Nancy", "1.4.3"))
                .Returns(new Stats { TotalDownloads = 1000 * 1000 });

            new DownloadStatsChecker(mockPackageQuerier.Object, mockStatsQuerier.Object, mockDownloadStatsCheck.Object)
                .LoadStats("Nancy");
            mockStatsQuerier.Verify(m => m.Set("Nancy", It.Is<Stats>(s => s.TotalDownloads == 1000 * 1000)));
        }
    }
}