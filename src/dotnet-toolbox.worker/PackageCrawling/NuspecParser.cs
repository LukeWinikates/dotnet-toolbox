using System.IO;
using System.Linq;
using System.Xml.Linq;

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
                Title = ValueForElement(doc, "title")
            };
        }

        private static string ValueForElement(XDocument doc, string elementShortName)
        {
            return doc.Descendants("{http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd}" + elementShortName).First().Value.ToString();
        }

        public class PackageDetails
        {
            public string Description { get; set; }
            public string Owners { get; set; }
            public string Title { get; set; }
            public string Version { get; set; }
        }
    }
}