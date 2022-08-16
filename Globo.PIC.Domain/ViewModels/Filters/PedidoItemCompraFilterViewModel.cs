using System.Collections.Generic;
using System.ComponentModel;

namespace Globo.PIC.Domain.ViewModels.Filters
{
	public class PedidoItemCompraFilterViewModel : BaseFilterViewModel
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("Campo de busca para Id Pedido Item")]
		public long Id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("Campo de busca para Id Pedido")]
		public long IdPedido { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public PedidoItemCompraFilterViewModel()
		{
		}
	}
}
