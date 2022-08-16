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
	public class GetByIdPedidoItemArteCompraDocumentos :
		IRequest<PedidoItemArteCompraDocumento>
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("id Pedido Item Documentos para pesquisa de pedido item compra Documentos")]
		public long Id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("id Pedido Item Compra para pesquisa de pedido item compra Documentos")]
		public long IdPedidoItemCompra { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("id Pedido Item para pesquisa de pedido item compra Documentos")]
		public long IdPedidoItem { get; set; }

		public CancellationToken CancellationToken { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("id do pedido para pesquisa de pedido item compra Documentos")]
		public long IdPedido { get; set; }
	}
}
