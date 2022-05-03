namespace Toast.Sms.Verification.Configurations
{
    /// <summary>
    /// This represents the app settings entity for Toast SMS endpoints.
    /// </summary>
    public class SmsVerificationEndpointSettings
    {
        /// <summary>
        /// Gets or sets the endpoint to get the list of senders.
        /// </summary>
        public virtual string ListSenders { get; set; }
    }
}