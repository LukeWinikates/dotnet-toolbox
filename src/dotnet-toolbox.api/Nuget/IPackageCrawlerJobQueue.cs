namespace dotnet_toolbox.api.Nuget {
    public interface IPackageCrawlerJobQueue {
        void EnqueueJob(string Name);
    }
}