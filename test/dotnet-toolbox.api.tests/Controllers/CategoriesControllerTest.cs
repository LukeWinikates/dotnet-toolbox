using Xunit;
using dotnet_toolbox.api.Controllers;
using System.Linq;
using Moq;
using dotnet_toolbox.api.Query;
using dotnet_toolbox.api.Models;
using System;

namespace dotnet_toolbox.api.tests.Controllers
{
    public class CategoriesControllerTest
    {
        [Fact]
        public void GetAll_ShouldReturnTheTop3Categories()
        {
            var mockGetQuerier = new Mock<IGetQuerier<Package>>();
            mockGetQuerier.Setup(m => m.Get(It.IsAny<string>())).Returns((Func<string,Package>)PackageFromName);

            var categories = new CategoriesController(mockGetQuerier.Object).GetAll();
            Assert.Equal(new[] { "Web Frameworks", "Dependency Injection", "Unit Test Runners" }, categories.Select(c => c.Title));
            Assert.Equal(new[] { "Nancy", "Microsoft.AspNet.Mvc" },categories.First().Packages.Select(l => l.Name));
            Assert.Equal(new[] { "Ninject", "Castle.Windsor", "Autofac"},categories.ElementAt(1).Packages.Select(l => l.Name));
            Assert.Equal(new[] { "xunit", "NUnit" },categories.ElementAt(2).Packages.Select(l => l.Name));
        }

        private Package PackageFromName(string name)
        {
            return new Package { Name = name };
        }
    }

}