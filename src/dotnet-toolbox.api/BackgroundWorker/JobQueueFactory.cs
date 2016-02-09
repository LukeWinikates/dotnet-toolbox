using StackExchange.Redis;

namespace dotnet_toolbox.api.BackgroundWorker
{

    public class JobQueueFactory : IJobQueueFactory
    {
        IDatabase database;

        public JobQueueFactory(IDatabase database)
        {
            this.database = database;
        }

        public IJobQueue ForQueueName(string queueName)
        {
            return new JobQueue(database, queueName);
        }
    }

    public interface IJobQueueFactory
    {
        IJobQueue ForQueueName(string queueName);
    }
}