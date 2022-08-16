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
	public class GetByIdPedidoItemConversa :
		IRequest<PedidoItemConversa>,
		IRequest<PedidoItemConversaAnexo>
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("id Pedido Item Conversa para pesquisa de pedido item Conversa")]
		public long Id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("id Pedido Item para pesquisa de pedido item")]
		public long IdPedidoItem { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("id do pedido para pesquisa de pedido item")]
		public long IdPedido { get; set; }
	}
}
