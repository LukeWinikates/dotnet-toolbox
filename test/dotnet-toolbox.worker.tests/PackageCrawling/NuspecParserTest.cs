using dotnet_toolbox.worker.PackageCrawling;
using Xunit;

namespace dotnet_toolbox.worker.tests.PackageCrawling
{
    public class NuspecParserTest
    {
        public static readonly string NuspecExample = @"
        <package xmlns=""http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd"">
        <metadata>
          <id>EntityFramework</id>
          <version>6.1.3</version>
          <title>EntityFramework</title>
          <authors>Microsoft</authors>
          <owners>Microsoft</owners>
          <licenseUrl>http://go.microsoft.com/fwlink/?LinkID=320539</licenseUrl>
          <projectUrl>http://go.microsoft.com/fwlink/?LinkID=320540</projectUrl>
          <iconUrl>http://go.microsoft.com/fwlink/?LinkID=386613</iconUrl>
          <requireLicenseAcceptance>true</requireLicenseAcceptance>
          <description>Entity Framework is Microsoft's recommended data access technology for new applications.</description>
          <summary>Entity Framework is Microsoft's recommended data access technology for new applications.</summary>
          <language>en-US</language>
          <tags>Microsoft EF Database Data O/RM ADO.NET</tags>
          <frameworkAssemblies>
            <frameworkAssembly assemblyName=""System.ComponentModel.DataAnnotations"" targetFramework="""" />
          </frameworkAssemblies>
        </metadata>
      </package>";

        [Fact]
        public void Parse_CreatesPackageRepresentationFromNuspec()
        {
            var parser = new NuspecParser();
            var nuspecDeets = parser.Parse(NuspecExample);
            Assert.Equal("EntityFramework", nuspecDeets.Title);
            Assert.Equal("Microsoft", nuspecDeets.Owners);
            Assert.Equal("6.1.3", nuspecDeets.Version);
            Assert.StartsWith("Entity Framework is", nuspecDeets.Description);
        }
    }
}