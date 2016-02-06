namespace dotnet_toolbox.api.Models
{
    public interface INuspecPackageInfo
    {
        string Id { get; set; }
        string Owners { get; set; }
        string Version { get; set; }
        string Description { get; set; }
    }
}