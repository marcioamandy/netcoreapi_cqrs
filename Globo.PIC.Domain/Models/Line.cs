
using System.Collections.Generic;

namespace Globo.PIC.Domain.Models
{

	public class Line
    {

        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string currencyCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string deliverToLocationCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string destinationTypeCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string item { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool negotiatedByPreparerFlag { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int number { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal quantity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal unitPrice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string requestedDeliveryDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string requesterEmail { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string typeCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string destinationOrganizationCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? agreementId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? agreementLineId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string sourceLineId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string supplierId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Distribution> distribution { get; set; }
        
    }
}
