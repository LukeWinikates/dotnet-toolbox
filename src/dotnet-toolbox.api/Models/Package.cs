namespace dotnet_toolbox.api.Models
{
    public class Package : INuspecPackageInfo
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Owners { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
    }
}