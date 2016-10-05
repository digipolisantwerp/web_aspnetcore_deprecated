using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Digipolis.Web.Api;
using Digipolis.Web.Api.Constraints;
using Digipolis.Web.Api.Conventions;
using Digipolis.Web.UnitTests.Utilities;
using Digipolis.Web.UnitTests._TestObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.Options;
using Moq;
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
        public void CtorErrorNullParameters2()
        {
            Mock<IRouteTemplateProvider> rtp = new Mock<IRouteTemplateProvider>();
            rtp.SetupGet(x => x.Template).Returns("{apiVersion}");
            var rc = new RouteConvention(rtp.Object);
            var am = new ApplicationModel();
            Mock<ControllerModel> cm = new Mock<ControllerModel>(typeof(Controller).GetTypeInfo(), 
                new object[] { });
            cm.Object.Selectors.Add(new SelectorModel { AttributeRouteModel = new AttributeRouteModel
                {
                    Template = "api/test"
                }
            });

            am.Controllers.Add(cm.Object);
            rc.Apply(am);
            Assert.Equal("{apiVersion}/api/test", am.Controllers.First().Selectors.First().AttributeRouteModel.Template);
        }

        [Fact]
        public void AcceptFalseWhenDisableVersioningFalseAndInValid()
        {
            var acc = MvcMockHelpers.MoqHttpContext(ctx =>
            {
                ctx.Setup(x => x.RequestServices.GetService(typeof(IOptions<ApiExtensionOptions>))).Returns<object>(x => new TestApiExtensionOptions(new ApiExtensionOptions { DisableVersioning = false }));
            }).MoqActionConstraintContext();

            acc.RouteContext.RouteData.Values["apiVersion"] = "v2";

            var versions = new VersionConstraint(new string[] { "v1" });
            Assert.False(versions.Accept(acc));
        }
    }
}
