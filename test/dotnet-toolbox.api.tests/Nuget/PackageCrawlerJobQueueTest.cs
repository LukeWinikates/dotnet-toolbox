using dotnet_toolbox.api.Nuget;
using Moq;
using StackExchange.Redis;
using Xunit;

namespace dotnet_toolbox.api.tests.Nuget
{
    public class PackageCrawlerJobQueueTest
    {
        [Fact]
        public void EnqueueJob_ShouldSendAJobToRabbitMQ()
        {
            Mock<IDatabase> mockRedisDatabase = new Mock<IDatabase>();
            var crawler = new PackageCrawlerJobQueue(mockRedisDatabase.Object);
            crawler.EnqueueJob("Cheese");
            mockRedisDatabase.Verify(
                m => m.ListRightPush((RedisKey)PackageCrawlerJobQueue.QueueName,
                (RedisValue) "Cheese", 
                When.Always, 
                CommandFlags.None));
        }
    }
}