using Moq;
using Xunit;
using dotnet_toolbox.api.NuspecCrawler;
using dotnet_toolbox.api.Query;
using dotnet_toolbox.api.BackgroundWorker;

namespace dotnet_toolbox.api.tests.NuspecCrawler
{
    public class CrawlerTest
    {
        Mock<INuspecDownloader> mockNuspecDownloader = new Mock<INuspecDownloader>();
        Mock<IGetSetQuerier<PackageDetails>> mockQuerier = new Mock<IGetSetQuerier<PackageDetails>>();
        Mock<IJobQueue> downloadStatsJobQueue = new Mock<IJobQueue>();
        Crawler crawler;

        public CrawlerTest()
        {
            var packageDetails = @"
            <metadata>
          <id>AutoMapper</id>
          <version>LatestAndGreatest</version>
          <description>turns objects into different objects</description>
          <owners>Jimmy Bogard I think?</owners>
          </metadata>";
            mockNuspecDownloader.Setup(m => m.Download("AutoMapper"))
                .Returns(packageDetails);
            crawler = new Crawler(mockQuerier.Object, mockNuspecDownloader.Object, downloadStatsJobQueue.Object);

        }

        [Fact]
        public void CrawlProject_GetsNuspecFromNugetAndStoresContents()
        {
            crawler.CrawlProject("AutoMapper");
            mockQuerier.Verify(m => m.Set("AutoMapper", nuspecPackageInfoLike()));
        }

        [Fact]
        public void CrawlProject_QueuesADownloadStatsCheck()
        {
            crawler.CrawlProject("AutoMapper");
            downloadStatsJobQueue.Verify(m => m.EnqueueJob("AutoMapper"));
        }

        private PackageDetails nuspecPackageInfoLike()
        {
            return It.Is<PackageDetails>(rv =>
                 rv.Id == "AutoMapper" &&
                 rv.Version == "LatestAndGreatest" &&
                 rv.Description == "turns objects into different objects" &&
                 rv.Owners == "Jimmy Bogard I think?"
            );
        }
    }
}