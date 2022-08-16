using System;
using System.Collections.Generic;

namespace Globo.PIC.Domain.Models
{
    public class TarefaModel
    {

        /// <summary>
        //"id": 100000034613633
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        //"projectId": 300000070465072
        /// </summary>
        public long ProjectId { get; set; }

        /// <summary>
        //"number": "5.2.2"
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        //"description": "Prova 2"
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        //"level": 3
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        //"startDate": "23-06-2021"
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        //"endDate": "23-06-2021"
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        //"chargeable": true
        /// </summary>
        public bool Chargeable { get; set; }

        /// <summary>
        //"parentId": "100000034613631"
        /// </summary>
        public long? ParentId { get; set; }

        /// <summary>
        //"parentNumber": "5.2"
        /// </summary>
        public string ParentNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProjectNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<AccountingSegment> AccountingSegments { get; set; }
    }
}
