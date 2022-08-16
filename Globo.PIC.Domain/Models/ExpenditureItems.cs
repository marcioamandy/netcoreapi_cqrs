
using System.Collections.Generic;

namespace Globo.PIC.Domain.Models
{
	public class ExpenditureItems
	{
		/// <summary>
		/// 
		/// </summary>
		public List<Expenditures> items { get; set; }

		public ExpenditureItems()
		{
			items = new List<Expenditures>();
		}
	}
}
