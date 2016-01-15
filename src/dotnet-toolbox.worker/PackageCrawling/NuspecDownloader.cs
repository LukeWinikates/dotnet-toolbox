using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace dotnet_toolbox.worker.PackageCrawling
{
    public class NuspecDownloader
    {
        public string Download(string packageName)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://www.nuget.org/api/v2/package/");
            var byteTask = client.GetByteArrayAsync(packageName);
            byteTask.Wait();
            var first = new System.IO.Compression.ZipArchive(new MemoryStream(byteTask.Result))
                            .Entries.First(f => f.Name.Contains(".nuspec"));
            var buffer = new byte[first.Length];
            first.Open().Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer);
        }
    }
}