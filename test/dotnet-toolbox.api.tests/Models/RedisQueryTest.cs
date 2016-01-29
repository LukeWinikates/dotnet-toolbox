using Xunit;
using dotnet_toolbox.api.Models;
using dotnet_toolbox.api.Query;
using Moq;
using StackExchange.Redis;
using dotnet_toolbox.api.Env;
using Newtonsoft.Json;
using System;
using System.Linq.Expressions;

namespace dotnet_toolbox.api.tests.Models
{
    public class RedisQueryTest
    {
        Mock<IDatabase> mockRedisDatabase = new Mock<IDatabase>();
        RedisGetSetQuery<Package> subject;

        public RedisQueryTest()
        {
            subject = new RedisGetSetQuery<Package>(mockRedisDatabase.Object, Constants.Redis.PackageKeyForName);
        }

        [Fact]
        public void Get_WhenTypeIsPackage_FindsThePackageInRedisDb()
        {
            mockRedisDatabase.Setup(m => m.StringGet(("packages.GameOfLife"), CommandFlags.None))
                .Returns(JsonConvert.SerializeObject(new Package { Name = "GameOfLife" }));
            var package = subject.Get("GameOfLife");
            Assert.Equal(package.Name, "GameOfLife");
        }

        [Fact]
        public void Get_WhenRedisValueIsNull_ReturnsNull()
        {
            mockRedisDatabase.Setup(m => m.StringGet(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).Returns(RedisValue.Null);
            Assert.Null(subject.Get("Null!"));
        }

        [Fact]
        public void Set_SavesAJsonRepresentationOfThePackage()
        {
            subject.Set("GameOfLife", new Package { Name = "GameOfLife", Version = "1.1.0" });
            Expression<Func<RedisValue,bool>> aRedisValueThatLooksRight = (r =>
                 JsonConvert.DeserializeObject<Package>(r).Pipe(p => p.Name == "GameOfLife" && p.Version == "1.1.0"));
            mockRedisDatabase.Verify(
                m => m.StringSet(Constants.Redis.PackageKeyForName("GameOfLife"), It.Is(aRedisValueThatLooksRight), null, When.Always, CommandFlags.None));
        }
    }
}