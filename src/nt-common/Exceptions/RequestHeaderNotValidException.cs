using System;
using System.Net;
using System.Runtime.Serialization;

namespace Toast.Common.Exceptions
{
    public class RequestHeaderNotValidException : ToastException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestHeaderNotValidException"/> class.
        /// </summary>
        public RequestHeaderNotValidException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestHeaderNotValidException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public RequestHeaderNotValidException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestHeaderNotValidException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException"><see cref="Exception"/> instance as the inner exception.</param>
        public RequestHeaderNotValidException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestHeaderNotValidException"/> class.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> instance.</param>
        /// <param name="context"><see cref="StreamingContext"/> instance.</param>
        protected RequestHeaderNotValidException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <inheritdoc />
        public override HttpStatusCode StatusCode { get; set; } = HttpStatusCode.BadRequest;
    }
}