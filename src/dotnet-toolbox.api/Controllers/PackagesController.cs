using System.Collections.Generic;
using dotnet_toolbox.api.Nuget;
using Microsoft.AspNet.Mvc;

namespace dotnet_toolbox.api.Controllers
{
    [Route("api/[controller]")]
    public class PackagesController : Controller
    {
        INugetApi nugetApi;

        public PackagesController(INugetApi nugetApi)
        {
            this.nugetApi = nugetApi;
        }

        [HttpPost]
        public HttpStatusCodeResult Post([FromBody] CreatePackageRequest package)
        {
            return new HttpStatusCodeResult(nugetApi.GetPackage(package.Name) ? 200 : 404);
        }

        public class CreatePackageRequest
        {
            public string Name { get; set; }
        }
    }
}
