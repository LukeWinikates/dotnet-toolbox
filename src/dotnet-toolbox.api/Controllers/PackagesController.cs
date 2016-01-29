using Microsoft.AspNet.Mvc;
using dotnet_toolbox.api.Models;
using dotnet_toolbox.api.Nuget;
using dotnet_toolbox.api.Query;

namespace dotnet_toolbox.api.Controllers
{
    [Route("api/[controller]")]
    public class PackagesController : Controller
    {
        INugetApi nugetApi;
        IPackageCrawlerJobQueue queue;
        private IGetSetQuerier<Package> redisQuerier;

        public PackagesController(INugetApi nugetApi, IPackageCrawlerJobQueue queue, IGetSetQuerier<Package> redisQuery)
        {
            this.nugetApi = nugetApi;
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
            var existingPackage = this.redisQuerier.Get(package.Name);
            if (existingPackage == null)
            {
                redisQuerier.Set(package.Name, new Package { Name = package.Name });
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
