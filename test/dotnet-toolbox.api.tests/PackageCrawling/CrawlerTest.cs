using Moq;
using Xunit;
using dotnet_toolbox.api.PackageCrawling;
using dotnet_toolbox.api.Query;

namespace dotnet_toolbox.api.tests.PackageCrawling
{
    public class CrawlerTest
    {
        [Fact]
        public void CrawlProject_GetsNuspecFromNugetAndStoresContents()
        {
            var mockNuspecDownloader = new Mock<INuspecDownload>();
            var mockQuerier = new Mock<IGetSetQuerier<NuspecParser.PackageDetails>>();

            var packageDetails = new NuspecParser.PackageDetails
            {
                Id = "AutoMapper",
                Version = "LatestAndGreatest",
                Description = "Turns objects into different objects",
                Owners = "Jimmy Bogard I think?"
            };
            mockNuspecDownloader.Setup(m => m.Download("AutoMapper"))
                .Returns(packageDetails);
            var crawler = new Crawler(mockQuerier.Object, mockNuspecDownloader.Object);
            crawler.CrawlProject("AutoMapper");
            mockQuerier.Verify(m => m.Set("AutoMapper", nuspecPackageInfoLike(packageDetails)));
        }

        private NuspecParser.PackageDetails nuspecPackageInfoLike(NuspecParser.PackageDetails packageDetails)
        {
            return It.Is<NuspecParser.PackageDetails>(rv =>
                 rv.Id == packageDetails.Id &&
                 rv.Version == packageDetails.Version &&
                 rv.Description == packageDetails.Description &&
                 rv.Owners == packageDetails.Owners
            );
        }
    }
}