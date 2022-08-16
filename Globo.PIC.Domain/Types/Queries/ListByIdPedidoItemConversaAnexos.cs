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
	public class ListByIdPedidoItemConversaAnexos :
		IRequest<List<PedidoItemConversaAnexo>>
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("id pedido item conversa para pesquisa de pedido item conversa anexo")]
		public long IdPedidoItemConversa { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("id pedido item conversa anexo para pesquisa de pedido item conversa anexo")]
		public long IdPedidoItemConversaAnexo { get; set; }
		public CancellationToken CancellationToken { get; set; }
	}
}
