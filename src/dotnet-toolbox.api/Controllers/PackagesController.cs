using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using NodaTime;
using dotnet_toolbox.api.Models;
using dotnet_toolbox.api.Nuget;
using dotnet_toolbox.api.Query;
using dotnet_toolbox.api.BackgroundWorker;
using dotnet_toolbox.api.Env;

namespace dotnet_toolbox.api.Controllers
{
    [Route("api/[controller]")]
    public class PackagesController : Controller
    {
        INugetApi nugetApi;
        IJobQueue packageCrawlerJobQueue;
        private IGetSetQuerier<Package> redisQuerier;
        ILatestPackagesIndex latestPackages;

        public PackagesController(INugetApi nugetApi, IJobQueueFactory queueFactory, IGetSetQuerier<Package> redisQuery, ILatestPackagesIndex latestPackages)
        {
            this.nugetApi = nugetApi;
            this.packageCrawlerJobQueue = queueFactory.ForQueueName(Constants.Redis.PackageCrawlerJobQueueName);
            this.redisQuerier = redisQuery;
            this.latestPackages = latestPackages;
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
            packageCrawlerJobQueue.EnqueueJob(package.Name);

            return new HttpStatusCodeResult(200);
        }

        private void EnsurePackageEntryExistsInDatabase(CreatePackageRequest package)
        {
            var existingPackage = this.redisQuerier.Get(package.Name);
            if (existingPackage == null)
            {
                redisQuerier.Set(package.Name, new Package { Name = package.Name });
                latestPackages.Update(SystemClock.Instance.GetCurrentInstant().Ticks, package.Name);
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
        
        [HttpGet]
        public IEnumerable<Package> GetRecent()
        {
            return latestPackages.Get().Select(redisQuerier.Get).ToArray();
        }
    }
}