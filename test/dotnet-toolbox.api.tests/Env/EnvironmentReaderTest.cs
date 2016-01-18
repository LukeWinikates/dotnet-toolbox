using Xunit;
using dotnet_toolbox.api.Env;

namespace dotnet_toolbox.api.tests.Env
{
    public class EnvironmentReaderTest
    {
        const string VCAP_SERVICES_WITH_REDIS = @"{
      ""p-redis"": [
        {
          ""name"": ""dotnet-toolbox-staging-redis"",
          ""label"": ""p-redis"",
          ""tags"": [
            ""pivotal"",
            ""redis""
          ],
          ""plan"": ""shared-vm"",
          ""credentials"": {
            ""host"": ""192.168.1.1"",
            ""password"": ""passw0rd"",
            ""port"": 38943
          }
        }
      ]
    }";

        [Fact]
        public void RedisConnectionString_WhenVCAPServicesUndefined_ReturnsLocalhost()
        {
            Assert.Equal("localhost", new EnvironmentReader(null).RedisConnectionString);
        }

        [Fact]
        public void RedisConnectionString_WhenVCAPServicesPresent_ReturnsTheProperString()
        {
            Assert.Equal("192.168.1.1:38943,password=passw0rd", new EnvironmentReader(VCAP_SERVICES_WITH_REDIS).RedisConnectionString);
        }
    }
}