using Xunit;

namespace dotnet_toolbox.api.tests.Controllers
{
    public class ValuesControllerTest
    {
        [Fact]
        public void Get_ReturnsAnArray()
        {
            var controller = new dotnet_toolbox.api.Controllers.ValuesController();
            Assert.Contains("value1", controller.Get());
            Assert.Contains("value2", controller.Get());
        }
    }
}
