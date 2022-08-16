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
	public class GetByIdPedidoItem :
		IRequest<PedidoItem>,
		IRequest<List<PedidoItemAnexo>>,
		IRequest<List<PedidoItemConversa>>,
		IRequest<List<PedidoItemConversaAnexo>>
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("id Pedido Item para pesquisa de pedido item")]
		public long Id { get; set; }
		public CancellationToken CancellationToken { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("id do pedido para pesquisa de pedido item")]
		public long IdPedido { get; set; }
	}
}
