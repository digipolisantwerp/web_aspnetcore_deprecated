using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Digipolis.Web.Versioning;
using Xunit;

namespace Digipolis.Web.UnitTests.Versioning
{
    public class WebVersioningOptionsSetupTests
    {
        [Fact]
        private void OptionsServicesIsSet()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var optionsSetup = new WebVersioningOptionsSetup(serviceProviderMock.Object);

            Assert.Same(serviceProviderMock.Object, optionsSetup.OptionsServices);
        }

        [Fact]
        private void ConventionIsAdded()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var options = new TestWebVersioningOptions(new WebVersioningOptions() { Route = "myroute" });
            serviceProviderMock.Setup(svp => svp.GetService(typeof(IOptions<WebVersioningOptions>))).Returns(options);

            var mvcOptions = new MvcOptions();
            var setup = new WebVersioningOptionsSetup(serviceProviderMock.Object);

            Assert.Equal(0, mvcOptions.Conventions.Count);

            setup.Configure(mvcOptions);

            Assert.Equal(1, mvcOptions.Conventions.Count);
        }
    }
}
