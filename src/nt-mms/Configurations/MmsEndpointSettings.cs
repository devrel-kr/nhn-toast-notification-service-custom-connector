namespace Toast.Mms.Configurations
{
    /// <summary>
    /// This represents the app settings entity for Toast MMS endpoints.
    /// </summary>
    public class MmsEndpointSettings
    {
        /// <summary>
        /// Gets or sets the endpoint to send messages.
        /// </summary>
        public virtual string SendMessages { get; set; }

        /// <summary>
        /// Gets or sets the endpoint to get the list of messages.
        /// </summary>
        public virtual string ListMessages { get; set; }

        /// <summary>
        /// Gets or sets the endpoint to get a single message.
        /// </summary>
        public virtual string GetMessage { get; set; }
    }
}