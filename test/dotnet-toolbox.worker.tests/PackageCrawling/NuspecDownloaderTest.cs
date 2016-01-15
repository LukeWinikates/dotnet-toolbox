using dotnet_toolbox.worker.PackageCrawling;
using Xunit;

namespace dotnet_toolbox.worker.tests.PackageCrawling
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