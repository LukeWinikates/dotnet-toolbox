using Xunit;
using dotnet_toolbox.api.Controllers;
using dotnet_toolbox.api.Nuget;
using Moq;
using StackExchange.Redis;
using Newtonsoft.Json;
using dotnet_toolbox.api.Models;
using dotnet_toolbox.api.Env;
using System.Linq;

namespace dotnet_toolbox.api.tests.Controllers
{
    public class CategoriesControllerTest {
        [Fact]
        public void GetAll_ShouldReturnTheTop3Categories() {
            Assert.Equal(new CategoriesController().GetAll().Select(c => c.Title), new[] { "Web Frameworks", "Dependency Injection", "Unit Test Runners" });
        }
     }

}