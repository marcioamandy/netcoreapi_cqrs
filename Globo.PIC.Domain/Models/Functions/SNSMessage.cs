using System;
using System.Collections.Generic;

namespace Globo.PIC.Domain.Models.Functions
{
    public class SNSMessage
    {
        /// <summary>
        /// The message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The attributes associated with the message.
        /// </summary>
        public IDictionary<string, MessageAttribute> MessageAttributes { get; set; }

        /// <summary>
        /// The message id.
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// The message signature.
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// The signature version used to sign the message.
        /// </summary>
        public string SignatureVersion { get; set; }

        /// <summary>
        /// The URL for the signing certificate.
        /// </summary>
        public string SigningCertUrl { get; set; }

        /// <summary>
        /// The subject for the message.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// The message time stamp.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// The topic ARN.
        /// </summary>
        public string TopicArn { get; set; }

        /// <summary>
        /// The message type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The message unsubscribe URL.
        /// </summary>
        public string UnsubscribeUrl { get; set; }
    }
}
