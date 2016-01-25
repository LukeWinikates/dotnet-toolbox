namespace dotnet_toolbox.api.Models
{
    public interface INuspecPackageInfo
    {
        string Id { get; }
        string Owners { get; }
        string Version { get; }
        string Description { get; }
    }
}