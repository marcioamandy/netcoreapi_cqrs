using MediatR;
using System.ComponentModel;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Queries
{

	/// <summary>
	/// 
	/// </summary>
	public class GetByIdPedidoItemArte : IRequest<PedidoItemArte>
	{

		/// <summary>
		/// 
		/// </summary>
		[Description("id Pedido Item Arte para pesquisa de pedido item arte")]
		public long Id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("id do pedido para pesquisa de pedido item")]
		public long IdPedido { get; set; }
	}
}
