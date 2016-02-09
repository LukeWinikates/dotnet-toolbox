namespace dotnet_toolbox.api.BackgroundWorker
{
    public interface IJobQueue
    {
        void EnqueueJob(string Name);
    }
}