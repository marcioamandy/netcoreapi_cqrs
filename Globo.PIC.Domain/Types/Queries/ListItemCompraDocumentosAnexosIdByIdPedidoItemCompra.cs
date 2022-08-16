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
	public class ListItemCompraDocumentosAnexosIdByIdPedidoItemCompra :
		IRequest<List<PedidoItemArteCompraDocumentoAnexo>>
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
		[Description("id pedido item Compra para pesquisa de pedido item Compra")]
		public long IdPedidoItemCompra { get; set; }
		public CancellationToken CancellationToken { get; set; }
	}
}
