using System.Collections.Generic;

namespace dotnet_toolbox.api.Models
{
    public class Category
    {
        public string Title { get; set; }
        public IEnumerable<Package> Packages { get; set; }
    }
}