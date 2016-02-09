namespace dotnet_toolbox.api.NuspecCrawler
{
    public interface INuspecDownloader
    {
        string Download(string packageName);
    }
}