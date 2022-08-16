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
	public class ListByIdPedidoItemAnexo :
		IRequest<List<PedidoItemAnexo>>
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("id pedido item para pesquisa de pedido item anexo")]
		public long IdPedidoItem { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("id pedido item anexo para pesquisa de pedido item imagem")]
		public long IdPedidoItemAnexo { get; set; }
		public CancellationToken CancellationToken { get; set; }
	}
}
