using MediatR;
using System.ComponentModel;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Queries
{
	/// <summary>
	/// 
	/// </summary>
	public class GetByIdPedidoWithOutRoles :
		IRequest<Pedido>
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("id do filtro para pesquisa de pedido")]
		public long Id { get; set; }
    }
}
