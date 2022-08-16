using MediatR;
using System.ComponentModel;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Models;

namespace Globo.PIC.Domain.Types.Queries
{
	/// <summary>
	/// 
	/// </summary>
	public class GetByIdPedidoItemVeiculo :
		IRequest<PedidoItemVeiculo>
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("id do filtro para pesquisa de pedido Item")]
		public long IdItem { get; set; }
    }
}
