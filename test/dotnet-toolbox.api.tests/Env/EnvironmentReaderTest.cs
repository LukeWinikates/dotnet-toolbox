using Xunit;
using dotnet_toolbox.api.Env;

namespace dotnet_toolbox.api.tests.Env
{
    public class EnvironmentReaderTest
    {
        [Fact]
        public void RedisConnectionString_WhenVCAPServicesUndefined_ReturnsLocalhost() {
            Assert.Equal("localhost", new EnvironmentReader().RedisConnectionString);
        }
    }
}