using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Digipolis.Web;
using Digipolis.Web.SampleApi.Configuration;
using AutoMapper;
using Digipolis.Web.SampleApi.Data;
using Digipolis.Web.SampleApi.Logic;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Digipolis.Web.Swagger;
using Digipolis.Web.Startup;
using Digipolis.Web.Monitoring;
using Digipolis.Web.Api;

namespace Digipolis.Web.SampleApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc()
                .AddApiExtensions(Configuration.GetSection("ApiExtensions"), x =>
                {
                    //Override settings made by the appsettings.json
                    x.PageSize = 10;
                    x.DisableVersioning = true;
                });

            services.AddGlobalErrorHandling<ApiExceptionMapper>();

           

            // Add Swagger extensions
            services.AddSwaggerGen<ApiExtensionSwaggerSettings>(o =>
            {
                o.SwaggerDoc(Versions.V1, new Info
                {
                    //Add Inline version
                    Version = Versions.V1,
                    Title = "API V1",
                    Description = "Description for V1 of the API",
                    Contact = new Contact { Email = "info@digipolis.be", Name = "Digipolis", Url = "https://www.digipolis.be" },
                    TermsOfService = "https://www.digipolis.be/tos",
                    License = new License
                    {
                        Name = "My License",
                        Url = "https://www.digipolis.be/licensing"
                    },
                });

                o.SwaggerDoc("v2", new Version2());
            });

            //Register Dependencies for example project
            services.AddScoped<IValueRepository, ValueRepository>();
            services.AddScoped<IValueLogic, ValueLogic>();

            //Add AutoMapper
            services.AddAutoMapper();

            services.AddScoped<IStatusProvider, StatusProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // Enable Api Extensions
            app.UseApiExtensions();

            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "docs/{documentName}/swagger.json";
                c.PreSerializeFilters.Add((swagger, httpReq) => swagger.Host = httpReq.Host.Value);
            });

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/docs/v1/swagger.json", "V1 Documentation");
                options.SwaggerEndpoint("/docs/v2/swagger.json", "V2 Documentation");
            });

            app.UseSwaggerUiRedirect();

        }


        //private static bool ResolveVersionSupportByRouteConstraint(ApiDescription apiDesc, string targetApiVersion)
        //{
        //    var versionConstraint = (apiDesc.Route.Constraints.ContainsKey("apiVersion"))
        //        ? apiDesc.Route.Constraints["apiVersion"] as RegexRouteConstraint
        //        : null;

        //    return (versionConstraint == null)
        //        ? ((targetApiVersion == "v1") ? true : false) // DossierController moet in v1 documentatie komen, maar de route mag niet gewijzigd worden (dus geen apiversion constraint mogelijk)
        //        : versionConstraint.Pattern.Split('|').Contains(targetApiVersion);
        //}
    }
}
