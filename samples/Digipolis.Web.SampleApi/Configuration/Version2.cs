using System;
using Microsoft.OpenApi.Models;

namespace Digipolis.Web.SampleApi.Configuration
{
    /// <summary>
    /// Contains all information for V2 of the API
    /// </summary>
    public class Version2 : OpenApiInfo
    {
        /// <summary>
        ///     Constructs OpenApiInfo object for version 2 of the API
        /// </summary>
        public Version2()
        {
            Version = Versions.V2;
            Title = "API V2";
            Description = "Description for V2 of the API";
            Contact = new OpenApiContact {Email = "info@digipolis.be", Name = "Digipolis", Url = new Uri("https://www.digipolis.be")};
            TermsOfService = new Uri("https://www.digipolis.be/tos");
            License = new OpenApiLicense
            {
                Name = "My License",
                Url = new Uri("https://www.digipolis.be/licensing")
            };
        }
    }
}