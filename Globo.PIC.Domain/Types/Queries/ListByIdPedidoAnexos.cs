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
	public class ListByIdPedidoAnexos :
		IRequest<List<PedidoAnexo>>
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("id pedido para pesquisa de pedido anexos")]
		public long IdPedido { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("id pedido anexos para pesquisa de pedido anexos")]
		public long IdPedidoAnexos { get; set; }
		public CancellationToken CancellationToken { get; set; }
	}
}
