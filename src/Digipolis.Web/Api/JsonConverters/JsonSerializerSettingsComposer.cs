using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Digipolis.Web.Api.JsonConverters
{
    public static class JsonSerializerSettingsComposer
    {
        /// <summary>
        ///     Sets the default Digipolis JSON serializer settings for Newtonsoft.JSON
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="settingsAction"></param>
        public static void Initialize(this JsonSerializerSettings settings, Action<JsonSerializerSettings> settingsAction = null)
        {
            settings.ContractResolver = new BaseContractResolver();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Converters.Add(new TimeSpanConverter());
            settings.Converters.Add(new GuidConverter());
            settings.Converters.Add(new StringEnumConverter());
            settings.Formatting = Formatting.None;

            settingsAction?.Invoke(settings);
        }
    }
}