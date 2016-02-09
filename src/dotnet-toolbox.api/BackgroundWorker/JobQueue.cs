using StackExchange.Redis;

namespace dotnet_toolbox.api.BackgroundWorker
{
    public class JobQueue : IJobQueue
    {
        IDatabase database;
        string queueName;

        public JobQueue(IDatabase redisDatabase, string queueName)
        {
            this.database = redisDatabase;
            this.queueName = queueName;
        }

        public void EnqueueJob(string name)
        {
            database.ListRightPush(queueName, name);
        }
    }
}