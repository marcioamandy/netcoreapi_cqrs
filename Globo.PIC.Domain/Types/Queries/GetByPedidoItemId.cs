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
	public class GetByPedidoItemId :
        IRequest<List<PedidoItemAnexo>>
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("id do filtro para pesquisa de pedido")]
		public long Id { get; set; }
        public CancellationToken CancellationToken { get; set; }
    }
}
