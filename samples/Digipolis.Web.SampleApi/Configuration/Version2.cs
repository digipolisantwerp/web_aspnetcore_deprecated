using Swashbuckle.AspNetCore.Swagger;

namespace Digipolis.Web.SampleApi.Configuration
{
    /// <summary>
    /// Contains all information for V2 of the API
    /// </summary>
    public class Version2 : Info
    {
        public Version2()
        {
            this.Version = Versions.V2;
            this.Title = "API V2";
            this.Description = "Description for V2 of the API";
            this.Contact = new Contact { Email = "info@digipolis.be", Name = "Digipolis", Url = "https://www.digipolis.be" };
            this.TermsOfService = "https://www.digipolis.be/tos";
            this.License = new License
            {
                Name = "My License",
                Url = "https://www.digipolis.be/licensing"
            };
        }
    }
}
