
using System.Collections.Generic;

namespace Globo.PIC.Domain.Models
{
	public class Distribution
    {
        public long number { get; set; }
        public long quantity { get; set; }
        public string projectNumber { get; set; }

        public string taskNumber { get; set; }

        public Expenditure expenditure { get; set; }
        public List<AccountingSegmentRequisition> accountingSegments { get; set; }
    }
}
