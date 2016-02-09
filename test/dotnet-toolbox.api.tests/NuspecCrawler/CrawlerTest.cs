using Moq;
using Xunit;
using dotnet_toolbox.api.NuspecCrawler;
using dotnet_toolbox.api.Query;

namespace dotnet_toolbox.api.tests.NuspecCrawler
{
    public class CrawlerTest
    {
        [Fact]
        public void CrawlProject_GetsNuspecFromNugetAndStoresContents()
        {
            var mockNuspecDownloader = new Mock<INuspecDownloader>();
            var mockQuerier = new Mock<IGetSetQuerier<PackageDetails>>();

            var packageDetails = @"
            <metadata>
          <id>AutoMapper</id>
          <version>LatestAndGreatest</version>
          <description>turns objects into different objects</description>
          <owners>Jimmy Bogard I think?</owners>
          </metadata>";
            mockNuspecDownloader.Setup(m => m.Download("AutoMapper"))
                .Returns(packageDetails);
            var crawler = new Crawler(mockQuerier.Object, mockNuspecDownloader.Object);
            crawler.CrawlProject("AutoMapper");
            mockQuerier.Verify(m => m.Set("AutoMapper", nuspecPackageInfoLike()));
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