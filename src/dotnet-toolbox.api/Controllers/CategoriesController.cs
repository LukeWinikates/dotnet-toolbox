using System.Collections.Generic;
using System.Linq;
using dotnet_toolbox.api.Models;
using dotnet_toolbox.api.Query;
using Microsoft.AspNet.Mvc;

namespace dotnet_toolbox.api.Controllers
{
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private const string NANCY = "Nancy";
        private const string ASP_NET_MVC = "Microsoft.AspNet.Mvc";
        private const string NINJECT = "Ninject";
        private const string WINDSOR = "Castle.Windsor";
        private const string AUTOFAC = "Autofac";
        private const string STRUCTUREMAP = "StructureMap";
        private const string NUNIT = "NUnit";
        private const string XUNIT = "xunit";
        private const string MSPEC = "Machine.Specifications";
        private IGetSetQuerier<Package> querier;

        public CategoriesController(IGetSetQuerier<Package> querier)
        {
            this.querier = querier;
        }

        public IEnumerable<Category> GetAll()
        {
            return new[] {
                new Category { Title = "Web Frameworks",
                    Packages = Packages(NANCY, ASP_NET_MVC)
                },
                new Category { Title = "Dependency Injection",
                    Packages = Packages(NINJECT, WINDSOR, AUTOFAC, STRUCTUREMAP)
                },
                new Category { Title = "Unit Test Runners",
                    Packages = Packages(XUNIT, NUNIT, MSPEC)
                }
            };
        }

        private IEnumerable<Package> Packages(params string[] packages)
        {
            return packages.Select(querier.Get);
        }
        
        public static IEnumerable<string> KeyPackageNames = new string[] { NANCY, ASP_NET_MVC, NINJECT, WINDSOR, AUTOFAC, STRUCTUREMAP, XUNIT, NUNIT, MSPEC };
    }
}