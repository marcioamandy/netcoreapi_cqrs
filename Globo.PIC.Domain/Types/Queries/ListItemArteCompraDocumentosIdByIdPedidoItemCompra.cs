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
	public class ListItemArteCompraDocumentosIdByIdPedidoItemCompra :
		IRequest<List<PedidoItemArteCompraDocumento>>
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
		[Description("id pedido item compra para pesquisa de pedido item Entrega")]
		public long IdPedidoItemCompra { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("id pedido item Documento para pesquisa de pedido item Entrega")]
		public long IdPedidoItemDocumento { get; set; }
		public CancellationToken CancellationToken { get; set; }
	}
}
