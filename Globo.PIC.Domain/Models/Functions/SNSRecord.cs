using System;
namespace Globo.PIC.Domain.Models.Functions
{
    public class SNSRecord
    {
        /// <summary>
        /// The event source.
        /// </summary>
        public string EventSource { get; set; }

        /// <summary>
        /// The event subscription ARN.
        /// </summary>
        public string EventSubscriptionArn { get; set; }

        /// <summary>
        /// The event version.
        /// </summary>
        public string EventVersion { get; set; }

        /// <summary>
        /// The SNS message.
        /// </summary>
        public SNSMessage Sns { get; set; }
    }
}
