using Xunit;
using dotnet_toolbox.api.Nuget;

namespace dotnet_toolbox.api.tests.Nuget
{
    public class NugetApiTest
    {
        [Fact]
        public void GetPackage_QueriesNugetApi()
        {
            var nuget = new NugetApi();
            Assert.True(nuget.GetPackage("Moq"));
            Assert.False(nuget.GetPackage("Moqq"));
            Assert.False(nuget.GetPackage(null));
        }
    }
}