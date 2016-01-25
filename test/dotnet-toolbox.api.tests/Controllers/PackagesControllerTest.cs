using Xunit;
using dotnet_toolbox.api.Controllers;
using dotnet_toolbox.api.Nuget;
using Moq;
using StackExchange.Redis;
using Newtonsoft.Json;
using dotnet_toolbox.api.Models;

namespace dotnet_toolbox.api.tests.Controllers
{
    public class PackagesControllerTest
    {
        Mock<INugetApi> mockNugetApi = new Mock<INugetApi>();
        PackagesController controller;
        Mock<IDatabase> mockRedisDatabase = new Mock<IDatabase>();
        Mock<IPackageCrawlerJobQueue> mockJobQueue = new Mock<IPackageCrawlerJobQueue>();

        public PackagesControllerTest()
        {
            controller = new PackagesController(mockNugetApi.Object, mockRedisDatabase.Object, mockJobQueue.Object);
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
            mockRedisDatabase.Verify(m => m.StringSet("GameOfLife", It.IsAny<RedisValue>(), null, When.Always, CommandFlags.None));
        }

        [Fact]
        public void Post_WhenPackageDoesNotExist_DoesNotAddANewLibraryToTheDatabase()
        {
            mockNugetApi.Setup(m => m.GetPackage(It.IsAny<string>())).Returns(false);
            controller.Post(new PackagesController.CreatePackageRequest { Name = "GameOfLife" });
            mockRedisDatabase.Verify(
                m => m.StringSet("GameOfLife", It.IsAny<RedisValue>(), null, When.Always, CommandFlags.None), Times.Never());

        }

        [Fact]
        public void Post_WhenPackageExistsOnNugetAndInDatabase_DoesNotAddANewLibraryToTheDatabase()
        {
            mockRedisDatabase.Setup(
               m => m.StringGet(It.IsAny<RedisKey>(), CommandFlags.None)).Returns("asdfasdf");

            mockNugetApi.Setup(m => m.GetPackage(It.IsAny<string>())).Returns(true);
            controller.Post(new PackagesController.CreatePackageRequest { Name = "GameOfLife" });
            mockRedisDatabase.Verify(
               m => m.StringSet("GameOfLife", It.IsAny<RedisValue>(), null, When.Always, CommandFlags.None), Times.Never());
        }


        [Fact]
        public void Post_WhenPackageExists_QueuesAPackageCrawlingJob()
        {
            mockNugetApi.Setup(m => m.GetPackage(It.IsAny<string>())).Returns(true);
            controller.Post(new PackagesController.CreatePackageRequest { Name = "GameOfLife" });
            mockJobQueue.Verify(m => m.EnqueueJob("GameOfLife"));
        }
        
        [Fact]
        public void GetByName_ReturnsPackage() {
            var serializeObject = JsonConvert.SerializeObject(new Package { 
                Name = "Dracula"
            });
            mockRedisDatabase.Setup(m => m.StringGet("Dracula", CommandFlags.None)).Returns(serializeObject);
            var package = controller.GetByName("Dracula");
            Assert.Equal("Dracula", package.Name);
        }
    }
}