using dotnet_toolbox.api.Env;
using dotnet_toolbox.api.Models;
using dotnet_toolbox.api.Nuget;
using dotnet_toolbox.api.Query;
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
        private IGetQuerier<Package> redisQuerier;

        public PackagesController(INugetApi nugetApi, IDatabase redisDatabase, IPackageCrawlerJobQueue queue, IGetQuerier<Package> redisQuery)
        {
            this.nugetApi = nugetApi;
            this.redisDatabase = redisDatabase;
            this.queue = queue;
            this.redisQuerier = redisQuery;
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
            return redisQuerier.Get(packageName);
        }
    }
}
