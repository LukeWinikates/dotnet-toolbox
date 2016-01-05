using System.Collections.Generic;
using Microsoft.AspNet.Mvc;

namespace dotnet_toolbox.api.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
