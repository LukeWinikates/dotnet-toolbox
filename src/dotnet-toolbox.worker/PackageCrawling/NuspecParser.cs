using System.IO;
using System.Linq;
using System.Xml.Linq;
using dotnet_toolbox.common.Models;

namespace dotnet_toolbox.worker.PackageCrawling
{
    public class NuspecParser
    {
        public NuspecParser.PackageDetails Parse(string nuspecXml)
        {
            var doc = XDocument.Load(new StringReader(nuspecXml));
            return new PackageDetails
            {
                Version = ValueForElement(doc, "version"),
                Owners = ValueForElement(doc, "owners"),
                Description = ValueForElement(doc, "description"),
                Id = ValueForElement(doc, "id")
            };
        }

        private static string ValueForElement(XDocument doc, string elementShortName)
        {
            return doc.Descendants().FirstOrDefault(el => el.Name.LocalName == elementShortName)?.Value?.ToString();
        }

        public class PackageDetails : INuspecPackageInfo
        {
            public string Description { get; set; }
            public string Owners { get; set; }
            public string Id { get; set; }
            public string Version { get; set; }
        }
    }
}