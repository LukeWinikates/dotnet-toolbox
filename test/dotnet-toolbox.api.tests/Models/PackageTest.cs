using System.Linq;
using Xunit;
using dotnet_toolbox.api.Models;
using dotnet_toolbox.api.Query;

namespace dotnet_toolbox.api.tests.Models
{
    public class PackageTest
    {
        [Fact]
        public void AsRedisHash_ExcludesNullFields()
        {
            var fields = new Package { Name = "Foo" }.AsRedisHash();
            Assert.Single(fields);
            var field = fields.Single();
            Assert.Equal("Name", field.Name);
            Assert.Equal("Foo", field.Value);
        }

        [Fact]
        public void AsRedisHash_IncludesAllFieldsWhenPresent()
        {
            var fields = new Package
            {
                Name = "Foo",
                Id = "Foo",
                Version = "1.5.7",
                Owners = "Perseus Fizzibuzz",
                Description = "Popular Examples",
                TotalDownloads = 30 * 1000
            }.AsRedisHash();
            
            Assert.Equal("Foo", fields.ValueFor("Name"));
            Assert.Equal("Foo", fields.ValueFor("Id"));
            Assert.Equal("1.5.7", fields.ValueFor("Version"));
            Assert.Equal("Perseus Fizzibuzz", fields.ValueFor("Owners"));
            Assert.Equal("Popular Examples", fields.ValueFor("Description"));
            Assert.Equal(30 * 1000, fields.ValueFor("TotalDownloads"));
        }
    }
}