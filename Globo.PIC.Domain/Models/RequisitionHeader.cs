
namespace Globo.PIC.Domain.Models
{
	public class RequisitionHeader
	{
		public long RequisitionHeaderId { get; set; }
		public long RequisitioningBUId { get; set; }
		public string Requisition { get; set; }
		public string RequisitioningBU { get; set; }
	}
}
