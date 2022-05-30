using System.Net.Http.Formatting;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using SmartFormat;
using SmartFormat.Core.Settings;

namespace Toast.Common.Configurations
{
    /// <summary>
    /// This represents the app settings entity for Toast API.
    /// </summary>
    public class ToastSettings
    {
        /// <summary>
        /// Gets the name of the app settings.
        /// </summary>
        public const string Name = "Toast";

        /// <summary>
        /// Gets or sets the base URL.
        /// </summary>
        public virtual string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public virtual string Version { get; set; }

        /// <summary>
        /// Gets the <see cref="SmartFormatter"/> instance.
        /// </summary>
        public virtual SmartFormatter Formatter { get; } = Smart.CreateDefaultSmartFormat(new SmartSettings() { CaseSensitivity = CaseSensitivityType.CaseInsensitive });

        /// <summary>
        /// Gets the <see cref="JsonMediaTypeFormatter"/> instance.
        /// </summary>
        public JsonMediaTypeFormatter JsonFormatter { get; } = new JsonMediaTypeFormatter()
        {
            SerializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }
        };
    }

    /// <summary>
    /// This represents the app settings entity for Toast API.
    /// </summary>
    /// <typeparam name="T">Type of entyt representing the endpoints.</typeparam>
    public class ToastSettings<T> : ToastSettings
    {
        /// <summary>
        /// Gets or sets the endpoints object.
        /// </summary>
        public virtual T Endpoints { get; set; }
    }
}