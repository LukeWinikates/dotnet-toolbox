using Xunit;
using dotnet_toolbox.api.Controllers;
using dotnet_toolbox.api.Nuget;
using Moq;
using dotnet_toolbox.api.Models;
using dotnet_toolbox.api.Query;

namespace dotnet_toolbox.api.tests.Controllers
{
    public class PackagesControllerTest
    {
        Mock<INugetApi> mockNugetApi = new Mock<INugetApi>();
        PackagesController controller;
        Mock<IPackageCrawlerJobQueue> mockJobQueue = new Mock<IPackageCrawlerJobQueue>();
        Mock<IGetSetQuerier<Package>> mockRedisQuery = new Mock<IGetSetQuerier<Package>>();
        Mock<ILatestPackagesIndex> mockLatestPackagesQuery = new Mock<ILatestPackagesIndex>();

        public PackagesControllerTest()
        {
            controller = new PackagesController(
                 mockNugetApi.Object,
                 mockJobQueue.Object,
                 mockRedisQuery.Object,
                 mockLatestPackagesQuery.Object);
        }

        [Fact]
        public void Post_ChecksNugetForPackageToExist()
        {
            controller.Post(new PackagesController.CreatePackageRequest { Name = "GameOfLife" });
            mockNugetApi.Verify(m => m.GetPackage("GameOfLife"));
        }

        [Fact]
        public void Post_WhenPackageExistsOnNuget_AddsANewLibraryToTheDatabase()
        {
            mockNugetApi.Setup(m => m.GetPackage(It.IsAny<string>())).Returns(true);
            controller.Post(new PackagesController.CreatePackageRequest { Name = "GameOfLife" });
            mockRedisQuery.Verify(m => m.Set("GameOfLife", It.Is<Package>(p => p.Name == "GameOfLife")));
        }

        [Fact]
        public void Post_WhenPackageExistsOnNuget_AddsNewPackageToLatestPackagesIndex()
        {
            mockNugetApi.Setup(m => m.GetPackage(It.IsAny<string>())).Returns(true);
            controller.Post(new PackagesController.CreatePackageRequest { Name = "GameOfLife" });
            mockLatestPackagesQuery.Verify(m => m.Update(It.IsAny<long>(), "GameOfLife"));
        }

        [Fact]
        public void Post_WhenPackageDoesNotExist_DoesNotAddANewLibraryToTheDatabase()
        {
            mockNugetApi.Setup(m => m.GetPackage(It.IsAny<string>())).Returns(false);
            controller.Post(new PackagesController.CreatePackageRequest { Name = "GameOfLife" });
            mockRedisQuery.Verify(m => m.Set(It.IsAny<string>(), It.IsAny<Package>()), Times.Never());

        }

        [Fact]
        public void Post_WhenPackageExistsOnNugetAndInDatabase_DoesNotAddANewLibraryToTheDatabase()
        {
            mockRedisQuery.Setup(m => m.Get("GameOfLife")).Returns(new Package { Name = "GameOfLife" });
            mockNugetApi.Setup(m => m.GetPackage(It.IsAny<string>())).Returns(true);
            controller.Post(new PackagesController.CreatePackageRequest { Name = "GameOfLife" });
            mockRedisQuery.Verify(m => m.Set(It.IsAny<string>(), It.IsAny<Package>()), Times.Never());
        }


        [Fact]
        public void Post_WhenPackageExists_QueuesAPackageCrawlingJob()
        {
            mockNugetApi.Setup(m => m.GetPackage(It.IsAny<string>())).Returns(true);
            controller.Post(new PackagesController.CreatePackageRequest { Name = "GameOfLife" });
            mockJobQueue.Verify(m => m.EnqueueJob("GameOfLife"));
        }

        [Fact]
        public void GetByName_ReturnsPackage()
        {
            mockRedisQuery.Setup(m => m.Get("Dracula")).Returns(new Package { Name = "Dracula" });
            var package = controller.GetByName("Dracula");
            Assert.Equal("Dracula", package.Name);
        }
    }
}