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
	public class ListTrackingVeiculo :
		IRequest<List<PedidoItemVeiculoTracking>>
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("id pedido item para pesquisa de tracking")]
		public long IdPedidoItem { get; set; }
		public CancellationToken CancellationToken { get; set; }
	}
}
