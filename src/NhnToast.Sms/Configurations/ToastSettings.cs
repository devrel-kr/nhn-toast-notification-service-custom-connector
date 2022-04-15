using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Toast.Sms.Configurations
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
        /// Gets or sets the app key.
        /// </summary>
        public virtual string AppKey { get; set; }

        /// <summary>
        /// Gets or sets the secret key.
        /// </summary>
        public virtual string SecretKey { get; set; }

        /// <summary>
        /// Gets or sets the base URL.
        /// </summary>
        public virtual string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public virtual string Version { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="SmsSettings"/> object.
        /// </summary>
        public virtual SmsEndpointSettings Endpoints { get; set; }
    }
}