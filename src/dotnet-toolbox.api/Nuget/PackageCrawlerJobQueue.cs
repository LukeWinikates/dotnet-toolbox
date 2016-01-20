using RabbitMQ.Client;

namespace dotnet_toolbox.api.Nuget
{
    public class PackageCrawlerJobQueue : IPackageCrawlerJobQueue
    {
        public static readonly string RoutingKey = "PackageCrawlerJob";
        IModel model;
        public PackageCrawlerJobQueue(IModel model)
        {
            this.model = model;
        }

        public void EnqueueJob(string name)
        {
            byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(name);
            model.BasicPublish("", RoutingKey, null, messageBodyBytes);
        }
    }
}