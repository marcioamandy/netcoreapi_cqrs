using System.Collections.Generic;
using System.ComponentModel;

namespace Globo.PIC.Domain.ViewModels.Filters
{
	public class PedidoItemCompraDocumentosFilterViewModel : BaseFilterViewModel
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("Campo de busca para Id Pedido Item")]
		public long IdItem { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("Campo de busca para Id Pedido")]
		public long IdPedido { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("Campo de busca para Id Pedido Item Compra")]
		public long IdCompra { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public PedidoItemCompraDocumentosFilterViewModel()
		{
		}
	}
}
