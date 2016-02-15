using dotnet_toolbox.api.VersionHistory;
using Xunit;

namespace dotnet_toolbox.api.tests.VersinoHistory
{
    public class VersionHistoryCheckTest
    {
        [Fact]
        public void Download_GetsVersionsFromNugetApi()
        {
            var check = new VersionHistoryCheck();
            var versions = check.Download("Nancy");
            Assert.True(versions.Versions.Count >= 41);
        }
    }
}