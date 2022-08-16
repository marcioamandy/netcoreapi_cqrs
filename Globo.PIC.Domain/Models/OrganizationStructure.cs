
namespace Globo.PIC.Domain.Models
{
	public class OrganizationStructure
	{

		public long Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public OrganizationStructureLocation Location { get; set; }
		public string Status { get; set; }

		public OrganizationBusinessUnit OrganizationBusinessUnit { get; set; }
		public LegalEntity LegalEntity { get; set; }

		public ManagementBusinessUnit ManagementBusinessUnit { get; set; }

	}
}
