using System;
using System.Net;
using System.Runtime.Serialization;

namespace Toast.Common.Exceptions
{
    /// <summary>
    /// This represents the exception entity for RequestBodyNotValidException.
    /// </summary>
    public class RequestBodyNotValidException : ToastException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBodyNotValidException"/> class.
        /// </summary>
        public RequestBodyNotValidException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBodyNotValidException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public RequestBodyNotValidException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBodyNotValidException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException"><see cref="Exception"/> instance as the inner exception.</param>
        public RequestBodyNotValidException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBodyNotValidException"/> class.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> instance.</param>
        /// <param name="context"><see cref="StreamingContext"/> instance.</param>
        protected RequestBodyNotValidException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <inheritdoc />
        public override HttpStatusCode StatusCode { get; set; } = HttpStatusCode.BadRequest;
    }
}