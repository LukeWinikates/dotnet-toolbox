using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace dotnet_toolbox.api.Nuget
{
    public class NugetApi : INugetApi
    {
        public bool GetPackage(string name)
        {
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