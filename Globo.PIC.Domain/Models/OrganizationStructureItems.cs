
using System.Collections.Generic;

namespace Globo.PIC.Domain.Models
{
	public class OrganizationStructureItems
	{
		/// <summary>
		/// 
		/// </summary>
		public List<OrganizationStructure> items { get; set; }

		public OrganizationStructureItems()
		{
			items = new List<OrganizationStructure>();
		}
	}
}
