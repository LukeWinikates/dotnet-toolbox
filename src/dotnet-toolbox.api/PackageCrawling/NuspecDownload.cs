namespace dotnet_toolbox.api.PackageCrawling
{
    public class NuspecDownload : INuspecDownload
    {
        public NuspecParser.PackageDetails Download(string packageName)
        {
            var download = new NuspecDownloader().Download(packageName);
            return new NuspecParser().Parse(download);
        }
    }

    public interface INuspecDownload
    {
        NuspecParser.PackageDetails Download(string packageName);
    }
}