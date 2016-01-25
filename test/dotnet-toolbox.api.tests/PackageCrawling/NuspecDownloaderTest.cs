using dotnet_toolbox.api.PackageCrawling;
using Xunit;

namespace dotnet_toolbox.api.tests.PackageCrawling
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