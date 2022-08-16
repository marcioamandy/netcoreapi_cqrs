using MediatR;
using System.ComponentModel;
using Globo.PIC.Domain.Entities;
using System.Collections.Generic;

namespace Globo.PIC.Domain.Types.Queries
{
	/// <summary>
	/// 
	/// </summary>
	public class ListItemConversaIdByIdPedidoItem :
		IRequest<List<PedidoItemConversa>>,
		IRequest<List<PedidoItemConversaAnexo>>
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("id pedido para pesquisa de pedido item conversa")]
		public long IdPedido { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("id pedido item para pesquisa de pedido item conversa")]
		public long IdPedidoItem { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("id pedido item conversa para pesquisa de pedido item conversa")]
		public long IdPedidoItemConversa { get; set; }
	}
}
