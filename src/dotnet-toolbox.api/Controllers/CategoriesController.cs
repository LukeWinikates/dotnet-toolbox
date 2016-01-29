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
        private IGetSetQuerier<Package> querier;

        public CategoriesController(IGetSetQuerier<Package> querier)
        {
            this.querier = querier;
        }

        public IEnumerable<Category> GetAll()
        {
            return new[] {
                new Category { Title = "Web Frameworks",
                    Packages = Packages("Nancy", "Microsoft.AspNet.Mvc")
                },
                new Category { Title = "Dependency Injection",
                    Packages = Packages("Ninject", "Castle.Windsor", "Autofac")
                },
                new Category { Title = "Unit Test Runners",
                    Packages = Packages("xunit", "NUnit")
                }
            };
        }

        private IEnumerable<Package> Packages(params string[] packages)
        {
            return packages.Select(querier.Get);
        }
    }
}