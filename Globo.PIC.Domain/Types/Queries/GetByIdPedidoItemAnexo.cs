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
	public class GetByIdPedidoItemAnexo :
		IRequest<PedidoItemAnexo>
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("id Pedido Item anexo para pesquisa de pedido item anexo")]
		public long Id { get; set; }
		public CancellationToken CancellationToken { get; set; }
	}
}
