using dotnet_toolbox.api.NuspecCrawler;
using Xunit;

namespace dotnet_toolbox.api.tests.NuspecCrawler
{
    public class NuspecDownloaderTest
    {
        [Fact]
        public void DownloadNuspecBytes_ConstructsUrlCorrectly()
        {
            var downloader = new NuspecDownloader();
            Assert.Contains("EntityFramework", downloader.Download("EntityFramework"));
        }
    }
}