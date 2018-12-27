using Digipolis.Web.Api.Conventions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Digipolis.Web.UnitTests.Api.Conventions
{
    public class RouteConventionTests
    {
        [Fact]
        public void CtorErrorNullParameters()
        {
            Assert.Throws<ArgumentNullException>(() => new RouteConvention(null));
        }

        [Fact]
        public void ApplyErrorNullParameters()
        {
            Mock<IRouteTemplateProvider> rtp = new Mock<IRouteTemplateProvider>();
            var rc = new RouteConvention(rtp.Object);
            Assert.Throws<ArgumentNullException>(() => rc.Apply(null));
        }

        [Fact]
        public void ApplyRoutePrefixOnExistingRouteSuccess()
        {
            Mock<IRouteTemplateProvider> rtp = new Mock<IRouteTemplateProvider>();
            rtp.SetupGet(x => x.Template).Returns("{apiVersion}");
            var rc = new RouteConvention(rtp.Object);
            var am = new ApplicationModel();
            Mock<ControllerModel> cm = new Mock<ControllerModel>(typeof(Controller).GetTypeInfo(), new object[] { });
            cm.Object.Selectors.Add(new SelectorModel
            {
                AttributeRouteModel = new AttributeRouteModel
                {
                    Template = "api/test"
                }
            });

            am.Controllers.Add(cm.Object);
            rc.Apply(am);
            Assert.Equal("{apiVersion}/api/test", am.Controllers.First().Selectors.First().AttributeRouteModel.Template);
        }

        [Fact]
        public void ApplyRoutePrefixOnNonExistingRouteSuccess()
        {
            Mock<IRouteTemplateProvider> rtp = new Mock<IRouteTemplateProvider>();
            rtp.SetupGet(x => x.Template).Returns("{apiVersion}");
            var rc = new RouteConvention(rtp.Object);
            var am = new ApplicationModel();
            Mock<ControllerModel> cm = new Mock<ControllerModel>(typeof(Controller).GetTypeInfo(), new object[] { });
            cm.Object.Selectors.Add(new SelectorModel());

            am.Controllers.Add(cm.Object);
            rc.Apply(am);
            Assert.Equal("{apiVersion}", am.Controllers.First().Selectors.First().AttributeRouteModel.Template);
        }

        [Fact]
        public void ApplyIgnoreRoutePrefixOnNonExistingRouteSuccess()
        {
            Mock<IRouteTemplateProvider> rtp = new Mock<IRouteTemplateProvider>();
            rtp.SetupGet(x => x.Template).Returns("{apiVersion}");
            var rc = new RouteConvention(rtp.Object);
            var am = new ApplicationModel();
            Mock<ControllerModel> cm = new Mock<ControllerModel>(typeof(Controller).GetTypeInfo(), new object[] { new ApiExplorerSettingsAttribute { IgnoreApi = true } });
            cm.Object.Selectors.Add(new SelectorModel
            {
                AttributeRouteModel = new AttributeRouteModel
                {
                    Template = "api/test"
                }
            });

            am.Controllers.Add(cm.Object);
            rc.Apply(am);
            Assert.Equal("api/test", am.Controllers.First().Selectors.First().AttributeRouteModel.Template);
        }
    }
}
