using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Digipolis.Web.Versioning;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Digipolis.Web.UnitTests.Versioning
{
    public class WebVersioningConventionTests
    {
        [Fact]
        private void OptionsIsSet()
        {
            var options = new WebVersioningOptions() { Route = "myroute" };
            var optionsMock = new Mock<IOptions<WebVersioningOptions>>();
            optionsMock.Setup(o => o.Value).Returns(options);

            var convention = new WebVersioningConvention(optionsMock.Object);

            Assert.Same(options, convention.Options);
        }

        [Fact]
        private void RouteIsSetForVersionControllerModel()
        {
            var options = new WebVersioningOptions() { Route = "myroute" };
            var optionsMock = new Mock<IOptions<WebVersioningOptions>>();
            optionsMock.Setup(o => o.Value).Returns(options);

            var convention = new WebVersioningConvention(optionsMock.Object);
            var model = new ControllerModel(typeof(VersionController).GetTypeInfo(), new List<object>());

            convention.Apply(model);

            Assert.Equal(1, model.RouteValues.Count);
            Assert.Equal("myroute", model.RouteValues.First().Value);
            Assert.Equal("WebVersioningRoute", model.RouteValues.First().Key);
        }

        [Fact]
        private void RouteIsNotSetForNonVersionControllerModel()
        {
            var options = new WebVersioningOptions() { Route = "myroute" };
            var optionsMock = new Mock<IOptions<WebVersioningOptions>>();
            optionsMock.Setup(o => o.Value).Returns(options);

            var convention = new WebVersioningConvention(optionsMock.Object);
            var model = new ControllerModel(typeof(TestController).GetTypeInfo(), new List<object>());

            convention.Apply(model);

            Assert.Equal(0, model.RouteValues.Count);
        }

        [Fact]
        private void RouteNullIsNotSetForVersionController()
        {
            var options = new WebVersioningOptions() { Route = null };
            var optionsMock = new Mock<IOptions<WebVersioningOptions>>();
            optionsMock.Setup(o => o.Value).Returns(options);

            var convention = new WebVersioningConvention(optionsMock.Object);
            var model = new ControllerModel(typeof(VersionController).GetTypeInfo(), new List<object>());

            convention.Apply(model);

            Assert.Equal(0, model.RouteValues.Count);
        }

        [Fact]
        private void RouteEmptyIsNotSetForVersionController()
        {
            var options = new WebVersioningOptions() { Route = "" };
            var optionsMock = new Mock<IOptions<WebVersioningOptions>>();
            optionsMock.Setup(o => o.Value).Returns(options);

            var convention = new WebVersioningConvention(optionsMock.Object);
            var model = new ControllerModel(typeof(VersionController).GetTypeInfo(), new List<object>());

            convention.Apply(model);

            Assert.Equal(0, model.RouteValues.Count);
        }

        [Fact]
        private void RouteWhitespaceIsNotSetForVersionController()
        {
            var options = new WebVersioningOptions() { Route = "   " };
            var optionsMock = new Mock<IOptions<WebVersioningOptions>>();
            optionsMock.Setup(o => o.Value).Returns(options);

            var convention = new WebVersioningConvention(optionsMock.Object);
            var model = new ControllerModel(typeof(VersionController).GetTypeInfo(), new List<object>());

            convention.Apply(model);

            Assert.Equal(0, model.RouteValues.Count);
        }
    }
}
