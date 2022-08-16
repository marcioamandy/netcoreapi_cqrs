using MediatR;
using System.ComponentModel;
using System.IO;
using Globo.PIC.Domain.Entities;
using System.Threading;

namespace Globo.PIC.Domain.Types.Queries
{
	/// <summary>
	/// 
	/// </summary>
	public class GetByIdPedidoItemArteEntrega :
		IRequest<PedidoItemArteEntrega>
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("id Pedido Item Entrega para pesquisa de pedido item compra Entrega")]
		public long Id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("id Pedido Item para pesquisa de pedido item compra Entrega")]
		public long IdPedidoItem { get; set; }

		public CancellationToken CancellationToken { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("id do pedido para pesquisa de pedido item")]
		public long IdPedido { get; set; }
	}
}
