using MediatR;
using System.ComponentModel;
using System.IO;
using Globo.PIC.Domain.Entities;
using System.Threading;
using System.Collections.Generic;

namespace Globo.PIC.Domain.Types.Queries
{
	/// <summary>
	/// 
	/// </summary>
	public class ListByIdPedidoItemDevolucao :
		IRequest<List<PedidoItemArteDevolucao>>
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("id pedido para pesquisa de pedido item Compra")]
		public long IdPedido { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("id pedido item para pesquisa de pedido item Compra")]
		public long IdPedidoItem { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("id pedido item devolucao para pesquisa de pedido item devolucao")]
		public long IdPedidoItemDevolucao { get; set; }
		public CancellationToken CancellationToken { get; set; }
	}
}
