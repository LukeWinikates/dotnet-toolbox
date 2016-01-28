using System;
using System.Collections.Generic;
using System.Linq;
using dotnet_toolbox.api.Models;
using Microsoft.AspNet.Mvc;

namespace dotnet_toolbox.api.Controllers
{
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        public IEnumerable<Category> GetAll()
        {
            return new[] { "Web Frameworks", "Dependency Injection", "Unit Test Runners" }
                .Select(n => new Category { Title = n });
        }
    }
}