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
	public class GetByIdPedidoItemConversaAnexos :
		IRequest<PedidoItemConversaAnexo>
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("id Pedido Item Conversa anexo para pesquisa de pedido item Conversa anexo")]
		public long Id { get; set; }
		public CancellationToken CancellationToken { get; set; }
	}
}
