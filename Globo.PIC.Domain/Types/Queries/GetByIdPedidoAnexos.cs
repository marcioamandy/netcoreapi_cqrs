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
	public class GetByIdPedidoAnexos :
		IRequest<PedidoAnexo>
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("id Pedido Anexos para pesquisa de pedido anexos")]
		public long Id { get; set; }
		public CancellationToken CancellationToken { get; set; }
	}
}
