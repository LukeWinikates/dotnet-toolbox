using dotnet_toolbox.api.Env;
using dotnet_toolbox.api.PackageCrawling;
using Moq;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using Xunit;

namespace dotnet_toolbox.api.tests.PackageCrawling
{
    public class CrawlerTest
    {
        [Fact]
        public void CrawlProject_GetsNuspecFromNugetAndStoresContents()
        {
            var mockNuspecDownloader = new Mock<INuspecDownload>();
            var mockDb = new Mock<IDatabase>();
            var packageDetails = new NuspecParser.PackageDetails
            {
                Id = "AutoMapper",
                Version = "LatestAndGreatest",
                Description = "Turns objects into different objects",
                Owners = "Jimmy Bogard I think?"
            };
            mockNuspecDownloader.Setup(m => m.Download("AutoMapper"))
                .Returns(packageDetails);
            var crawler = new Crawler(mockDb.Object, mockNuspecDownloader.Object);
            crawler.CrawlProject("AutoMapper");
            mockDb.Verify(m => m.StringSet(Constants.Redis.PackageKeyForName("AutoMapper"), stringWithAttributesLike(packageDetails), null, When.Always, CommandFlags.None));
        }

        private RedisValue stringWithAttributesLike(NuspecParser.PackageDetails packageDetails)
        {
            return It.Is<RedisValue>(rv =>
                 JObject.Parse(rv.ToString())["Id"].ToString() == packageDetails.Id &&
                 JObject.Parse(rv.ToString())["Version"].ToString() == packageDetails.Version &&
                 JObject.Parse(rv.ToString())["Description"].ToString() == packageDetails.Description &&
                 JObject.Parse(rv.ToString())["Owners"].ToString() == packageDetails.Owners
            );
        }
    }
}