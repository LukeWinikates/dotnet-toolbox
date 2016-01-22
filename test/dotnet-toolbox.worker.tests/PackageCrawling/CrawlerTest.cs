using System;
using dotnet_toolbox.worker.PackageCrawler;
using dotnet_toolbox.worker.PackageCrawling;
using Moq;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using Xunit;

namespace dotnet_toolbox.worker.tests.PackageCrawling
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
                Title = "AutoMapper",
                Version = "LatestAndGreatest",
                Description = "Turns objects into different objects",
                Owners = "Jimmy Bogard I think?"
            };
            mockNuspecDownloader.Setup(m => m.Download("AutoMapper"))
                .Returns(packageDetails);
            var crawler = new Crawler(mockDb.Object, mockNuspecDownloader.Object);
            crawler.CrawlProject("AutoMapper");
            mockDb.Verify(m => m.StringSet((RedisKey)"AutoMapper", stringWithAttributesLike(packageDetails), null, When.Always, CommandFlags.None));
        }

        private RedisValue stringWithAttributesLike(NuspecParser.PackageDetails packageDetails)
        {
            return It.Is<RedisValue>(rv =>
                 JObject.Parse(rv.ToString())["Title"].ToString() == packageDetails.Title &&
                 JObject.Parse(rv.ToString())["Version"].ToString() == packageDetails.Version &&
                 JObject.Parse(rv.ToString())["Description"].ToString() == packageDetails.Description &&
                 JObject.Parse(rv.ToString())["Owners"].ToString() == packageDetails.Owners
            );
        }
    }
}