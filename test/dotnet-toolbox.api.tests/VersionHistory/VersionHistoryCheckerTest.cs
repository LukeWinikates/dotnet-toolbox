using System.Collections.Generic;
using dotnet_toolbox.api.Models;
using dotnet_toolbox.api.Query;
using dotnet_toolbox.api.VersionHistory;
using Moq;
using Xunit;

namespace dotnet_toolbox.api.tests.VersionHistory
{
    public class VersionHistoryCheckerTest
    {
        [Fact]
        public void CrawlProject_GetsNuspecFromNugetAndStoresContents()
        {
            var mockVersionHistoryCheck = new Mock<IVersionHistoryCheck>();
            var mockVersionsListQuerier = new Mock<IGetSetQuerier<VersionsList>>();

            mockVersionHistoryCheck.Setup(m => m.Download("Nancy"))
                .Returns(new VersionsList
                {
                    Versions = new List<Version> {
                    new Version {
                        VersionNumber = "1",
                        Timestamp = "2013-01-01"
                    }
                }
                });

            new VersionHistoryChecker(mockVersionsListQuerier.Object, mockVersionHistoryCheck.Object)
                .LoadVersionHistory("Nancy");
            mockVersionsListQuerier.Verify(m => m.Set("Nancy", It.Is<VersionsList>(s => s.Versions.Count == 1)));
        }
    }
}