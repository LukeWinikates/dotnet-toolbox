using System.Linq;
using dotnet_toolbox.api.Query;
using Moq;
using NodaTime;
using NodaTime.Testing;
using StackExchange.Redis;
using Xunit;

namespace dotnet_toolbox.api.tests.Query
{
    public class LatestPackagesQueryTest
    {
        Mock<IDatabase> redisDatabase = new Mock<IDatabase>();
        IClock fakeClock = new FakeClock(Instant.FromTicksSinceUnixEpoch(1001));

        [Fact]
        public void Get_GetsTheLastBunchFromRedis()
        {
            var subject = new LatestPackagesQuery(redisDatabase.Object);
            redisDatabase.Setup(m => m.SortedSetRangeByRank("RecentPackages", 0, 9, Order.Descending, CommandFlags.None)).Returns(
                new RedisValue[] {"a", "b", "c", "d", "e", "f", "g", "h", "i", "j"
            });
            var packages = subject.Get();
            Assert.Equal("a", packages.First());
            Assert.Equal("j", packages.ElementAt(9));
        }

        [Fact]
        public void Update_AddsThePackageNameWithTheCurrentTimestamp()
        {
            var subject = new LatestPackagesQuery(redisDatabase.Object);
            subject.Update(fakeClock.GetCurrentInstant().Ticks, "NodaTime");
            redisDatabase.Verify(m => m.SortedSetAdd("RecentPackages", "nodatime", 1001, CommandFlags.None));
        }
    }
}