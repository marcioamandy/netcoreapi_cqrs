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
	public class ListByIdPedidoItemArteEntrega :
		IRequest<List<PedidoItemArteEntrega>>
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("id pedido item compra para pesquisa de pedido item compra")]
		public long Id { get; set; }
		
		/// <summary>
		/// 
		/// </summary>
		[Description("id pedido item para pesquisa de pedido item compra")]
		public long IdPedidoItem { get; set; }
		public CancellationToken CancellationToken { get; set; }
	}
}
