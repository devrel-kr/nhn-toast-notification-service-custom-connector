namespace Toast.Common.Models
{
    /// <summary>
    /// This represents the model entity for response. This MUST be inherited.
    /// </summary>
    public abstract class ResponseModel
    {
        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        public virtual ResponseHeaderModel Header { get; set; } = new ResponseHeaderModel();
    }

    /// <summary>
    /// This represents the model entity for response with body. This MUST be inherited.
    /// </summary>
    public abstract class ResponseModel<T> : ResponseModel
    {
        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        public virtual T Body { get; set; }
    }
}