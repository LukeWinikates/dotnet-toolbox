using Xunit;
using dotnet_toolbox.api.Models;
using dotnet_toolbox.api.Query;
using Moq;
using StackExchange.Redis;
using dotnet_toolbox.api.Env;
using Newtonsoft.Json;

namespace dotnet_toolbox.api.tests.Models
{
    public class RedisQueryTest
    {
        [Fact]
        public void Get_WhenTypeIsPackage_FindsThePackageInRedisDb()
        {
            var mockRedisDatabase = new Mock<IDatabase>();
            mockRedisDatabase.Setup(m => m.StringGet(("packages.GameOfLife"), CommandFlags.None))
                .Returns(JsonConvert.SerializeObject(new Package{ Name = "GameOfLife"}));
            var subject = new RedisGetQuery<Package>(mockRedisDatabase.Object, Constants.Redis.PackageKeyForName);
            var package = subject.Get("GameOfLife");
            Assert.Equal(package.Name, "GameOfLife");
        }
    }
}