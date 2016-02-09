using dotnet_toolbox.api.BackgroundWorker;
using Moq;
using StackExchange.Redis;
using Xunit;

namespace dotnet_toolbox.api.tests.BackgroundWorker
{
    public class JobQueueTest
    {
        [Fact]
        public void EnqueueJob_ShouldSendAJobToRedis()
        {
            Mock<IDatabase> mockRedisDatabase = new Mock<IDatabase>();
            var crawler = new JobQueue(mockRedisDatabase.Object, "QueueName");
            crawler.EnqueueJob("Cheese");
            mockRedisDatabase.Verify(
                m => m.ListRightPush((RedisKey)"QueueName",
                (RedisValue) "Cheese", 
                When.Always, 
                CommandFlags.None));
        }
    }
}