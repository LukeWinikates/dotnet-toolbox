using System;
using System.Linq.Expressions;
using Moq;
using StackExchange.Redis;
using Xunit;
using dotnet_toolbox.api.Env;
using dotnet_toolbox.api.Models;
using dotnet_toolbox.api.Query;

namespace dotnet_toolbox.api.tests.Query
{
    public class RedisGetSetQueryTest
    {
        Mock<IDatabase> mockRedisDatabase = new Mock<IDatabase>();
        RedisGetSetQuery<Package> subject;

        public RedisGetSetQueryTest()
        {
            subject = new RedisGetSetQuery<Package>(mockRedisDatabase.Object, Constants.Redis.PackageKeyForName);
        }

        [Fact]
        public void Get_WhenTypeIsPackage_FindsThePackageInRedisDb()
        {
            mockRedisDatabase.Setup(m => m.HashGetAll(Constants.Redis.PackageKeyForName("GameOfLife"), CommandFlags.None))
                .Returns(new Package { Name = "GameOfLife" }.AsRedisHash());
            var package = subject.Get("GameOfLife");
            Assert.Equal(package.Name, "GameOfLife");
        }

        [Fact]
        public void Get_WhenRedisValueIsNull_ReturnsNull()
        {
            mockRedisDatabase.Setup(m => m.HashGetAll(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).Returns(new HashEntry[] {});
            Assert.Null(subject.Get("Null!"));
        }

        [Fact]
        public void Set_SavesAJsonRepresentationOfThePackage()
        {
            subject.Set("GameOfLife", new Package { Name = "GameOfLife", Version = "1.1.0" });
            Expression<Func<HashEntry[],bool>> aRedisValueThatLooksRight = (r =>
                 new Package().DoTo(p=>p.FromRedisHash(r)).Pipe(p => p.Name == "GameOfLife" && p.Version == "1.1.0"));
            mockRedisDatabase.Verify(
                m => m.HashSet(Constants.Redis.PackageKeyForName("GameOfLife"), It.Is(aRedisValueThatLooksRight), CommandFlags.None));
        }
    }
}