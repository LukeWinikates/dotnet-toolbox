using System;
using System.Net.Http;
using System.IO;
using System.Linq;
using System.Text;

namespace dotnet_toolbox.api.Nuget
{
    public class NugetApi : INugetApi
    {
        public bool GetPackage(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://www.nuget.org");
            var task = client.GetAsync("/api/v2/package-versions/" + name);
            task.Wait();
            var text = task.Result.Content.ReadAsStringAsync();
            text.Wait();
            return text.Result.Length > 2;
        }
    }

    public interface INugetApi
    {
        bool GetPackage(string name);
    }    
}