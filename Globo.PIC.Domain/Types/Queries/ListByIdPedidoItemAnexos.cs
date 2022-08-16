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
	public class ListByIdPedidoItemAnexos :
		IRequest<List<PedidoItemAnexo>>
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("id pedido item para pesquisa de pedido item imagem")]
		public long IdPedidoItem { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("id pedido item imagem para pesquisa de pedido item imagem")]
		public long IdPedidoItemImagem { get; set; }
		public CancellationToken CancellationToken { get; set; }
	}
}
