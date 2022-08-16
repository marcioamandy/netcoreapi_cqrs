using System;
using System.Collections.Generic;

namespace Globo.PIC.Domain.Models
{

    /// <summary>
    /// 
    /// </summary>
    public class StatusLineOCRC
    {

        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CreatedDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LastUpdatedDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DueDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string InterfaceSourceCodeLine { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LegacyId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public FornecedorStatusLine Supplier { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<RequisitionStatusLine> Requisitions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<PurchaseStatusLine> Purchases { get; set; }
    }
}
