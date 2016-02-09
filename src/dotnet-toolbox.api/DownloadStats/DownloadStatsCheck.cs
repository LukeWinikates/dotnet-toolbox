using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;

namespace dotnet_toolbox.api.DownloadStats
{
    public class DownloadStatsCheck
    {
        public Stats Download(string nugetPackageName, string version)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(string.Format("https://www.nuget.org/api/v2/Packages(Id='{0}',Version='{1}')", nugetPackageName, version));
            var byteTask = client.GetByteArrayAsync(string.Empty);
            byteTask.Wait();
            return Encoding.UTF8.GetString(byteTask.Result)
                .Pipe(xml => XDocument.Load(new StringReader(xml)))
                .Pipe(xml => new Stats
                {
                    TotalDownloads = Int32.Parse(xml.Descendants().FirstOrDefault(el => el.Name.LocalName == "DownloadCount")?.Value?.ToString())
                });
        }

        public class Stats
        {
            public int TotalDownloads { get; set; }
        }
    }
}