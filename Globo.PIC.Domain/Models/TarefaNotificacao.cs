using System;
using System.Collections.Generic;
using System.Text;

namespace Globo.PIC.Domain.Models
{
    public class TarefaNotificacao
    {
        /// <summary>
        /// 
        /// </summary> 
        public long Id { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public long ProjectId { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string Number { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int Level { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string StartDate { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string EndDate { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public bool Chargeable { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ParentId { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ParentNumber { get; set; }
    }
}
