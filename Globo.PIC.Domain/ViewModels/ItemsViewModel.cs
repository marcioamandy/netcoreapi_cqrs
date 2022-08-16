using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Globo.PIC.Domain.ViewModels
{
    public class ItemsViewModel
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
        public string InventoryOrganizationName
        {
            get; set;
        }
    }
}
