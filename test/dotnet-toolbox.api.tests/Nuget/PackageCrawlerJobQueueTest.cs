using dotnet_toolbox.api.Nuget;
using Moq;
using RabbitMQ.Client;
using Xunit;

namespace dotnet_toolbox.api.tests.Nuget
{
    public class PackageCrawlerJobQueueTest
    {
        [Fact]
        public void EnqueueJob_ShouldSendAJobToRabbitMQ()
        {
            var mockModel = new Mock<IModel>();
            var crawler = new PackageCrawlerJobQueue(mockModel.Object);
            crawler.EnqueueJob("Cheese");
            mockModel.Verify(m => m.BasicPublish("", PackageCrawlerJobQueue.RoutingKey, null,
            It.Is<byte[]>(bytes => System.Text.Encoding.UTF8.GetString(bytes) == "Cheese")));
        }
    }
}