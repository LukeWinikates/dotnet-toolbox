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

        public static readonly string Nuspec2012Example = @"<?xml version=""1.0"" encoding=""utf-8""?>
        <package xmlns=""http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd"">
            <metadata>
                <id>StackExchange.Redis</id>
                <version>1.1.553-alpha</version>
                <requireLicenseAcceptance>false</requireLicenseAcceptance>
                <authors>Stack Exchange inc., marc.gravell</authors>
                <owners>marc.gravell</owners>
                <licenseUrl>https://raw.github.com/StackExchange/StackExchange.Redis/master/LICENSE</licenseUrl>
                <projectUrl>https://github.com/StackExchange/StackExchange.Redis</projectUrl>
                <description>High performance Redis client, incorporating both synchronous and asynchronous usage.</description>
                <summary>Redis client library</summary>
                <releaseNotes>Alpha for core-clr; if you aren't doing core-clr, you probably don't want this</releaseNotes>
                <copyright>Stack Exchange inc. 2014-</copyright>
                <tags>Async Redis Cache PubSub Messaging</tags>
                <dependencies>
                <group targetFramework="".NETFramework4.0"">
                    <dependency id=""Microsoft.Bcl"" version=""1.1.10"" />
                    <dependency id=""Microsoft.Bcl.Async"" version=""1.0.168"" />
                </group>
                <group targetFramework="".NETFramework4.5"" />
                <group targetFramework="".NETFramework4.6"" />
                <group targetFramework=""DNXCore5.0"">
                    <dependency id=""System.Collections.Concurrent"" version=""4.0.11-beta-23516"" />
                    <dependency id=""System.Collections.NonGeneric"" version=""4.0.1-beta-23516"" />
                    <dependency id=""System.Diagnostics.Debug"" version=""4.0.11-beta-23516"" />
                    <dependency id=""System.Diagnostics.Tools"" version=""4.0.1-beta-23516"" />
                    <dependency id=""System.Diagnostics.TraceSource"" version=""4.0.0-beta-23516"" />
                    <dependency id=""System.Globalization"" version=""4.0.11-beta-23516"" />
                    <dependency id=""System.IO"" version=""4.0.11-beta-23516"" />
                    <dependency id=""System.IO.Compression"" version=""4.1.0-beta-23516"" />
                    <dependency id=""System.IO.FileSystem"" version=""4.0.1-beta-23516"" />
                    <dependency id=""System.Linq"" version=""4.0.1-beta-23516"" />
                    <dependency id=""System.Net.NameResolution"" version=""4.0.0-beta-23516"" />
                    <dependency id=""System.Net.Primitives"" version=""4.0.11-beta-23516"" />
                    <dependency id=""System.Net.Security"" version=""4.0.0-beta-23516"" />
                    <dependency id=""System.Net.Sockets"" version=""4.1.0-beta-23516"" />
                    <dependency id=""System.Reflection"" version=""4.1.0-beta-23516"" />
                    <dependency id=""System.Reflection.Emit"" version=""4.0.1-beta-23516"" />
                    <dependency id=""System.Reflection.Emit.Lightweight"" version=""4.0.1-beta-23516"" />
                    <dependency id=""System.Reflection.Primitives"" version=""4.0.1-beta-23516"" />
                    <dependency id=""System.Reflection.TypeExtensions"" version=""4.1.0-beta-23516"" />
                    <dependency id=""System.Security.Cryptography.Algorithms"" version=""4.0.0-beta-23516"" />
                    <dependency id=""System.Security.Cryptography.X509Certificates"" version=""4.0.0-beta-23516"" />
                    <dependency id=""System.Text.Encoding"" version=""4.0.11-beta-23516"" />
                    <dependency id=""System.Text.RegularExpressions"" version=""4.0.11-beta-23516"" />
                    <dependency id=""System.Threading"" version=""4.0.11-beta-23516"" />
                    <dependency id=""System.Threading.Tasks"" version=""4.0.11-beta-23516"" />
                    <dependency id=""System.Threading.Thread"" version=""4.0.0-beta-23516"" />
                    <dependency id=""System.Threading.ThreadPool"" version=""4.0.10-beta-23516"" />
                    <dependency id=""System.Threading.Timer"" version=""4.0.1-beta-23516"" />
                </group>
                <group targetFramework="".NETPlatform5.5"">
                    <dependency id=""System.Collections.Concurrent"" version=""4.0.11-beta-23516"" />
                    <dependency id=""System.Collections.NonGeneric"" version=""4.0.1-beta-23516"" />
                    <dependency id=""System.Diagnostics.Debug"" version=""4.0.11-beta-23516"" />
                    <dependency id=""System.Diagnostics.Tools"" version=""4.0.1-beta-23516"" />
                    <dependency id=""System.Diagnostics.TraceSource"" version=""4.0.0-beta-23516"" />
                    <dependency id=""System.Globalization"" version=""4.0.11-beta-23516"" />
                    <dependency id=""System.IO"" version=""4.0.11-beta-23516"" />
                    <dependency id=""System.IO.Compression"" version=""4.1.0-beta-23516"" />
                    <dependency id=""System.IO.FileSystem"" version=""4.0.1-beta-23516"" />
                    <dependency id=""System.Linq"" version=""4.0.1-beta-23516"" />
                    <dependency id=""System.Net.NameResolution"" version=""4.0.0-beta-23516"" />
                    <dependency id=""System.Net.Primitives"" version=""4.0.11-beta-23516"" />
                    <dependency id=""System.Net.Security"" version=""4.0.0-beta-23516"" />
                    <dependency id=""System.Net.Sockets"" version=""4.1.0-beta-23516"" />
                    <dependency id=""System.Reflection"" version=""4.1.0-beta-23516"" />
                    <dependency id=""System.Reflection.Emit"" version=""4.0.1-beta-23516"" />
                    <dependency id=""System.Reflection.Emit.Lightweight"" version=""4.0.1-beta-23516"" />
                    <dependency id=""System.Reflection.Primitives"" version=""4.0.1-beta-23516"" />
                    <dependency id=""System.Reflection.TypeExtensions"" version=""4.1.0-beta-23516"" />
                    <dependency id=""System.Security.Cryptography.Algorithms"" version=""4.0.0-beta-23516"" />
                    <dependency id=""System.Security.Cryptography.X509Certificates"" version=""4.0.0-beta-23516"" />
                    <dependency id=""System.Text.Encoding"" version=""4.0.11-beta-23516"" />
                    <dependency id=""System.Text.RegularExpressions"" version=""4.0.11-beta-23516"" />
                    <dependency id=""System.Threading"" version=""4.0.11-beta-23516"" />
                    <dependency id=""System.Threading.Tasks"" version=""4.0.11-beta-23516"" />
                    <dependency id=""System.Threading.Thread"" version=""4.0.0-beta-23516"" />
                    <dependency id=""System.Threading.ThreadPool"" version=""4.0.10-beta-23516"" />
                    <dependency id=""System.Threading.Timer"" version=""4.0.1-beta-23516"" />
                </group>
                </dependencies>
                <frameworkAssemblies>
                <frameworkAssembly assemblyName=""mscorlib"" targetFramework="".NETFramework4.0"" />
                <frameworkAssembly assemblyName=""System"" targetFramework="".NETFramework4.0"" />
                <frameworkAssembly assemblyName=""System.Core"" targetFramework="".NETFramework4.0"" />
                <frameworkAssembly assemblyName=""Microsoft.CSharp"" targetFramework="".NETFramework4.0"" />
                <frameworkAssembly assemblyName=""System.IO.Compression"" targetFramework="".NETFramework4.5"" />
                <frameworkAssembly assemblyName=""mscorlib"" targetFramework="".NETFramework4.5"" />
                <frameworkAssembly assemblyName=""System"" targetFramework="".NETFramework4.5"" />
                <frameworkAssembly assemblyName=""System.Core"" targetFramework="".NETFramework4.5"" />
                <frameworkAssembly assemblyName=""Microsoft.CSharp"" targetFramework="".NETFramework4.5"" />
                <frameworkAssembly assemblyName=""System.IO.Compression"" targetFramework="".NETFramework4.6"" />
                <frameworkAssembly assemblyName=""mscorlib"" targetFramework="".NETFramework4.6"" />
                <frameworkAssembly assemblyName=""System"" targetFramework="".NETFramework4.6"" />
                <frameworkAssembly assemblyName=""System.Core"" targetFramework="".NETFramework4.6"" />
                <frameworkAssembly assemblyName=""Microsoft.CSharp"" targetFramework="".NETFramework4.6"" />
                </frameworkAssemblies>
            </metadata>
        </package>";

        public static readonly string WeirdNuspecExample = @"<?xml version=""1.0"" encoding=""utf-8""?>
        <package xmlns=""http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd"">
            <metadata>
                <id>For Argument's Sake</id>
                <rude>Pathological Case</rude>
            </metadata>
        </package>
        ";

        [Fact]
        public void Parse_CreatesPackageRepresentationFromNuspec()
        {
            var parser = new NuspecParser();
            var nuspecDeets = parser.Parse(NuspecExample);
            Assert.Equal("EntityFramework", nuspecDeets.Id);
            Assert.Equal("Microsoft", nuspecDeets.Owners);
            Assert.Equal("6.1.3", nuspecDeets.Version);
            Assert.StartsWith("Entity Framework is", nuspecDeets.Description);
        }

        [Fact]
        public void Parse_CreatesPackageRepresentationFromNuspec2012()
        {
            var parser = new NuspecParser();
            var nuspecDeets = parser.Parse(Nuspec2012Example);
            Assert.Equal("StackExchange.Redis", nuspecDeets.Id);
            Assert.Equal("marc.gravell", nuspecDeets.Owners);
            Assert.Equal("1.1.553-alpha", nuspecDeets.Version);
            Assert.StartsWith("High performance Redis client", nuspecDeets.Description);
        }

        [Fact]
        public void Parse_WhenNuspecLacksCertainKeys_StillWorks()
        {
            var parser = new NuspecParser();
            var nuspecDeets = parser.Parse(WeirdNuspecExample);
            Assert.Equal("For Argument's Sake", nuspecDeets.Id);
        }
    }
}