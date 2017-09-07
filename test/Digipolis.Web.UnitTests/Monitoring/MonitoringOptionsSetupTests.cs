using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Digipolis.Web.Monitoring;
using Xunit;

namespace Digipolis.Web.UnitTests.Versioning
{
    public class MonitoringOptionsSetupTests
    {
        [Fact]
        private void OptionsServicesIsSet()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var optionsSetup = new MonitoringOptionsSetup(serviceProviderMock.Object);

            Assert.Same(serviceProviderMock.Object, optionsSetup.OptionsServices);
        }

        [Fact]
        private void ConventionIsAdded()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var options = new TestMonitoringOptions(new MonitoringOptions() { Route = "myroute" });
            serviceProviderMock.Setup(svp => svp.GetService(typeof(IOptions<MonitoringOptions>))).Returns(options);

            var mvcOptions = new MvcOptions();
            var setup = new MonitoringOptionsSetup(serviceProviderMock.Object);

            Assert.Equal(0, mvcOptions.Conventions.Count);

            setup.Configure(mvcOptions);

            Assert.Equal(1, mvcOptions.Conventions.Count);
        }
    }
}
