using System.Collections.Generic;

namespace Globo.PIC.Domain.Models
{

	public class ResultShoppingCatalog
	{

		/// <summary>
        /// 
        /// </summary>
		public int TotalResults { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public List<ShoppingCatalogItem> Items { get; set; } = new List<ShoppingCatalogItem>();

	}

}
