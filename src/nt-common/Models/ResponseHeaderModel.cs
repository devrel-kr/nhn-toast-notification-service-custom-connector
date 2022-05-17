namespace Toast.Common.Models
{
    /// <summary>
    /// This represents the model entity for response header.
    /// </summary>
    public class ResponseHeaderModel
    {
        /// <summary>
        /// Gets or sets whether success header.
        /// </summary>
        public virtual bool IsSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the failure code header.
        /// </summary>
        public virtual int ResultCode { get; set; }

        /// <summary>
        /// Gets or sets the failure message header.
        /// </summary>
        public virtual string ResultMessage { get; set; }
    }
}