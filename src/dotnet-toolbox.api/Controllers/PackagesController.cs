using dotnet_toolbox.api.Env;
using dotnet_toolbox.api.Models;
using dotnet_toolbox.api.Nuget;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace dotnet_toolbox.api.Controllers
{
    [Route("api/[controller]")]
    public class PackagesController : Controller
    {
        INugetApi nugetApi;
        IDatabase redisDatabase;
        IPackageCrawlerJobQueue queue;

        public PackagesController(INugetApi nugetApi, IDatabase redisDatabase, IPackageCrawlerJobQueue queue)
        {
            this.nugetApi = nugetApi;
            this.redisDatabase = redisDatabase;
            this.queue = queue;
        }

        [HttpPost]
        public HttpStatusCodeResult Post([FromBody] CreatePackageRequest package)
        {
            var packageExists = nugetApi.GetPackage(package.Name);
            if (!packageExists)
            {
                return new HttpStatusCodeResult(404);
            }
            EnsurePackageEntryExistsInDatabase(package);
            queue.EnqueueJob(package.Name);

            return new HttpStatusCodeResult(200);
        }

        private void EnsurePackageEntryExistsInDatabase(CreatePackageRequest package)
        {
            string packageValue = this.redisDatabase.StringGet(package.Name);
            if (packageValue == null)
            {
                var packageJson = JsonConvert.SerializeObject(new Package { Name = package.Name });
                this.redisDatabase.StringSet(Constants.Redis.PackageKeyForName(package.Name), packageJson);
            }
        }

        public class CreatePackageRequest
        {
            public string Name { get; set; }
        }

        [HttpGet]
        [Route("{packageName}")]
        public Package GetByName(string packageName)
        {
            return JsonConvert.DeserializeObject<Package>(redisDatabase.StringGet(Constants.Redis.PackageKeyForName(packageName)));
        }
    }
}
