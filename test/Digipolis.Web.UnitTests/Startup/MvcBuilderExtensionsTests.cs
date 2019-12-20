using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;
using Microsoft.Extensions.ObjectPool;

namespace Digipolis.Web.UnitTests.Startup
{
    public class MvcBuilderExtensionsTests
    {
        // [Fact]
        // private void JsonOutputFormatterSupportsHAL()
        // {
        //     var services = new ServiceCollection();
        //     var manager = new ApplicationPartManager();
        //     var builder = new MvcBuilder(services, manager);
        //
        //     services.AddOptions();
        //     services.AddSingleton(typeof(ObjectPoolProvider), new DefaultObjectPoolProvider());
        //     services
        //         .AddLogging()
        //         .AddMvcCore()
        //         .AddJsonFormatters();
        //
        //     builder.AddApiExtensions();
        //
        //     var sp = services.BuildServiceProvider();
        //
        //     MvcOptions mvcOptions = sp.GetService<IOptions<MvcOptions>>().Value;
        //
        //     var jsonOutputFormatter = mvcOptions.OutputFormatters.OfType<JsonOutputFormatter>().First();
        //
        //     Assert.Contains(jsonOutputFormatter.SupportedMediaTypes, x => x == "application/hal+json");
        // }
    }
}
