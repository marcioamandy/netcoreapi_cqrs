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
	public class GetByStatusVeiculoId :
		IRequest<StatusPedidoItemVeiculo>
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("id do filtro para pesquisa de status veículo")]
		public long Id { get; set; }
		public CancellationToken CancellationToken { get; set; }
	}
}
