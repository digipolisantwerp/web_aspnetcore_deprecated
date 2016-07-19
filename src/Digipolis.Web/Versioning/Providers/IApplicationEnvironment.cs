using System;
using System.Runtime.Versioning;

namespace Digipolis.Web.Versioning
{
    public interface IApplicationEnvironment
    {
        string ApplicationBasePath { get; set; }
        string ApplicationName { get; set; }
        string ApplicationVersion { get; set; }

        FrameworkName RuntimeFramework { get; set; }
    }
}
