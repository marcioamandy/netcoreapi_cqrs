using MediatR;
using System.ComponentModel;
using Globo.PIC.Domain.Entities;
using System.Collections.Generic;

namespace Globo.PIC.Domain.Types.Queries
{

	/// <summary>
	/// 
	/// </summary>
	public class ListTrackingByIdItemPedido :
		IRequest<List<PedidoItemArteTracking>>,
		IRequest<List<PedidoItemVeiculoTracking>>
	{

		/// <summary>
		/// 
		/// </summary>
		[Description("id pedido item para pesquisa de tracking")]
		public long IdPedidoItem { get; set; }

	}
}
