using Newtonsoft.Json;

namespace Digipolis.Web.Api.JsonConverters
{
    internal static class JsonSerializerSettingsComposer
    {
        internal static void Initialize(this JsonSerializerSettings settings)
        {
            settings.ContractResolver = new BaseContractResolver();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Converters.Add(new TimeSpanConverter());
            settings.Converters.Add(new GuidConverter());
            settings.Formatting = Formatting.None;
        }
    }
}
