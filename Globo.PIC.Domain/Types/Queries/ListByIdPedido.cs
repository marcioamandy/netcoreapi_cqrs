using MediatR;
using System.ComponentModel;
using Globo.PIC.Domain.Entities;
using System.Collections.Generic;

namespace Globo.PIC.Domain.Types.Queries
{
	/// <summary>
	/// 
	/// </summary>
	public class ListByIdPedido :
		IRequest<List<Pedido>>,
        IRequest<List<PedidoItem>>,
		IRequest<List<Equipe>>
    {
		
		/// <summary>
		/// 
		/// </summary>
		[Description("id do filtro para pesquisa de pedido")]
		public long IdPedido { get; set; }

	}
}
