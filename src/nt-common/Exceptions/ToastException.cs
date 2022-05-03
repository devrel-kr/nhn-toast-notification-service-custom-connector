using System;
using System.Net;
using System.Runtime.Serialization;

namespace Toast.Common.Exceptions
{
    /// <summary>
    /// This represents the exception entity for NHN Toast.
    /// </summary>
    public abstract class ToastException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToastException"/> class.
        /// </summary>
        public ToastException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToastException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public ToastException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToastException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException"><see cref="Exception"/> instance as the inner exception.</param>
        public ToastException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToastException"/> class.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> instance.</param>
        /// <param name="context"><see cref="StreamingContext"/> instance.</param>
        protected ToastException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets or sets the <see cref=""HttpStatusCode"/> value.
        /// </summary>
        public virtual HttpStatusCode StatusCode { get; set; }
    }
}