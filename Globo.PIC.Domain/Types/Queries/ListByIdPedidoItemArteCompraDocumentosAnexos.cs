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
	public class ListByIdPedidoItemArteCompraDocumentosAnexos :
		IRequest<List<PedidoItemArteCompraDocumentoAnexo>>
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("id pedido item compra documento para pesquisa de pedido item compra documento anexo")]
		public long IdDocumento { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("id pedido item compra para pesquisa de pedido item compra documento anexo")]
		public long IdCompra { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("id pedido item compra anexo para pesquisa de pedido item Compra documento anexo")]
		public long IdPedidoItemCompraAnexo { get; set; }
		public CancellationToken CancellationToken { get; set; }
	}
}
