using Globo.PIC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Globo.PIC.Domain.Models
{
    public class items
    {
        /// <summary>
        /// 
        /// </summary> 
        public decimal LocationId { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string LocationCode { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string LocationName { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string EffectiveStartDate { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string EffectiveEndDate { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ActiveStatus { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ActiveStatusMeaning { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? SetId { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SetCode { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SetName { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? InventoryOrganizationId { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string InventoryOrganizationName { get; set; }
    }
}
