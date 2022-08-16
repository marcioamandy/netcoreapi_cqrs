
namespace Globo.PIC.Domain.Models
{
	public class RequisitionLine
    {
		public long RequisitionHeaderId { get; set; }
		public string RequisitionLineId { get; set; }
		public int LineNumber { get; set; }
		public int LineTypeId { get; set; }
		public string LineTypeCode { get; set; }
		public string RequisitionLineSource { get; set; }
		public long CategoryId { get; set; }
		public string CategoryName { get; set; }
		public string ItemDescription{ get; set; }
		public long ItemId { get; set; }
		public string Item { get; set; }
	}
}
