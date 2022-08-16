using MediatR;
using System.ComponentModel;
using Globo.PIC.Domain.Entities;
using System.Collections.Generic;

namespace Globo.PIC.Domain.Types.Queries
{

	/// <summary>
	/// 
	/// </summary>
	public class ListItemArteEntregaIdByIdPedidoItem :
		IRequest<List<PedidoItemArteEntrega>>
	{

		/// <summary>
		/// 
		/// </summary>
		[Description("id pedido para pesquisa de pedido item Entrega")]
		public long IdPedido { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("id pedido item para pesquisa de pedido item Entrega")]
		public long IdPedidoItem { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("id pedido item Entrega para pesquisa de pedido item Entrega")]
		public long IdPedidoItemEntrega { get; set; }
	}
}
