using System;
using System.Runtime.Versioning;
using Microsoft.Extensions.PlatformAbstractions;

namespace Digipolis.Web.Versioning
{
    public class WebApplicationEnvironment : IApplicationEnvironment
    {
        public WebApplicationEnvironment()
        {
            ApplicationBasePath = PlatformServices.Default.Application.ApplicationBasePath;
            ApplicationName = PlatformServices.Default.Application.ApplicationName;
            ApplicationVersion = PlatformServices.Default.Application.ApplicationVersion;
            RuntimeFramework = PlatformServices.Default.Application.RuntimeFramework;
        }

        public string ApplicationBasePath { get; set; }
        public string ApplicationName { get; set; }
        public string ApplicationVersion { get; set; }
        public FrameworkName RuntimeFramework { get; set; }
    }
}
