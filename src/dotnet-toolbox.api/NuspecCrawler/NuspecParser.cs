using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace dotnet_toolbox.api.NuspecCrawler
{
    public class NuspecParser
    {
        public PackageDetails Parse(string nuspecXml)
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
    }
}