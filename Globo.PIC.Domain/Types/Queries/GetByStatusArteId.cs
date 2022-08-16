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
	public class GetByStatusPedidoItemArteId :
		IRequest<StatusPedidoItemArte>
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("id do filtro para pesquisa de status do Pedido Item Arte")]
		public long Id { get; set; }
	}
}
