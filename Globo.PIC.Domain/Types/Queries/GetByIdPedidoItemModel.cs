using MediatR;
using System.ComponentModel;
using Globo.PIC.Domain.Models;

namespace Globo.PIC.Domain.Types.Queries
{

	/// <summary>
	/// 
	/// </summary>
	public class GetByIdPedidoItemModel :
		IRequest<PedidoItemModel>
	{

		/// <summary>
		/// 
		/// </summary>
		[Description("id Pedido Item para pesquisa de pedido item")]
		public long Id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("id do pedido para pesquisa de pedido item")]
		public long IdPedido { get; set; }

	}
}
