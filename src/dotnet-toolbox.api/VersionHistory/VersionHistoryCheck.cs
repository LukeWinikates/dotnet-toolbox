using System.Linq;
using System.Net.Http;
using System.Text;
using dotnet_toolbox.api.Models;
using Newtonsoft.Json.Linq;

namespace dotnet_toolbox.api.VersionHistory
{
    public class VersionHistoryCheck : IVersionHistoryCheck
    {
        public VersionsList Download(string nugetPackageName)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new System.Uri(string.Format("https://api.nuget.org/v3/registration1/{0}/index.json", nugetPackageName.ToLowerInvariant()));
            var byteTask = client.GetByteArrayAsync(string.Empty);
            byteTask.Wait();
            return Encoding.UTF8.GetString(byteTask.Result)
                .Pipe(jsonString => JObject.Parse(jsonString)["items"][0]["items"])
                .Select(entryJson => new Version
                {
                    VersionNumber = entryJson["catalogEntry"]["version"].Value<string>(),
                    Timestamp = entryJson["catalogEntry"]["published"].Value<string>()
                })
                .Pipe(list => new VersionsList
                {
                    Versions = list.ToList()
                });
        }
    }

    public interface IVersionHistoryCheck
    {
        VersionsList Download(string nugetPackageName);
    }
}