using Xunit;
using dotnet_toolbox.api.Controllers;
using dotnet_toolbox.api.Nuget;
using Moq;

namespace dotnet_toolbox.api.tests.Controllers
{
    public class PackagesControllerTest
    {
        Mock<INugetApi> mockNugetApi = new Mock<INugetApi>();

        [Fact]
        public void Post_ChecksNugetForPackageToExist()
        {
            var controller = new PackagesController(mockNugetApi.Object);
            controller.Post(new PackagesController.CreatePackageRequest { Name = "GameOfLife" });
            mockNugetApi.Verify(m => m.GetPackage("GameOfLife"));
        }
    }
}