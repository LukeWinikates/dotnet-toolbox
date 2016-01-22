using System;
using dotnet_toolbox.worker.PackageCrawler;
using Moq;
using StackExchange.Redis;
using Xunit;

namespace dotnet_toolbox.worker.tests.PackageCrawling
{
    public class CrawlerTest {
        [Fact]
        public void CrawlProject_GetsNuspecFromNugetAndStoresContents() {
            // fake zippy-fetchy business -> verify the url maybe? stub out the nuspec contents
            // verify project is saved in the DB
        }
    }
}