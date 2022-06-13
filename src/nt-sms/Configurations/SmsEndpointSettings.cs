namespace Toast.Sms.Configurations
{
    /// <summary>
    /// This represents the app settings entity for Toast SMS endpoints.
    /// </summary>
    public class SmsEndpointSettings
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

        /// <summary>
        /// Gets or sets the endpoint to get the list of message status.
        /// </summary>
        public virtual string ListMessageStatus { get; set; }

        /// <summary>
        /// Gets or sets the endpoint to get the list of senders.
        /// </summary>
        public virtual string ListSenders { get; set; }
    }
}