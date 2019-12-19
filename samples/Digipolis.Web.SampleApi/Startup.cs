using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Digipolis.Web.Api.JsonConverters;
using Digipolis.Web.Api.Tools;
using Digipolis.Web.SampleApi.Configuration;
using Digipolis.Web.SampleApi.Data;
using Digipolis.Web.SampleApi.Logic;
using Digipolis.Web.Startup;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Digipolis.Web.SampleApi
{
    public class Startup
    {
        public Startup(IHostEnvironment env)
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
                .AddNewtonsoftJson(options => options.SerializerSettings.Initialize()) // set default Digipolis json serializer settings
                .AddApiExtensions(Configuration.GetSection("ApiExtensions"), x =>
                {
                    //Override settings made by the appsettings.json
                    x.PageSize = 10;
                    x.DisableVersioning = false;
                });

            services.AddGlobalErrorHandling<ApiExceptionMapper>();

            services.AddAuthorization();

            // Add Swagger extensions
            services.AddSwaggerGen<ApiExtensionSwaggerSettings>(o =>
            {
                Array.ForEach(AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies().GetXmlDocPaths(), doc => o.IncludeXmlComments(doc));

                o.SwaggerDoc(Versions.V1, new OpenApiInfo
                {
                    //Add Inline version
                    Version = Versions.V1,
                    Title = "API V1",
                    Description = "Description for V1 of the API",
                    Contact = new OpenApiContact {Email = "info@digipolis.be", Name = "Digipolis", Url = new Uri("https://www.digipolis.be")},
                    License = new OpenApiLicense
                    {
                        Name = "My License",
                        Url = new Uri("https://www.digipolis.be/licensing")
                    }
                });
                o.SwaggerDoc(Versions.V2, new Version2());
            });

            //Register Dependencies for example project
            services.AddScoped<IValueRepository, ValueRepository>();
            services.AddScoped<IValueLogic, ValueLogic>();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole(x => Configuration.GetSection("logging"));
                loggingBuilder.AddDebug();
            });

            //Add AutoMapper
            services.AddAutoMapper(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            // Enable Api Extensions
            app.UseApiExtensions();

            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger(c => { c.RouteTemplate = "docs/{documentName}/swagger.json"; });

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/docs/v1/swagger.json", "V1 Documentation");
                options.SwaggerEndpoint("/docs/v2/swagger.json", "V2 Documentation");
            });

            app.UseSwaggerUiRedirect();
        }
    }
}