using MediatR;
using System.ComponentModel;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Models;

namespace Globo.PIC.Domain.Types.Queries
{
	/// <summary>
	/// 
	/// </summary>
	public class GetByIdPedido :
		IRequest<Pedido>
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("id do filtro para pesquisa de pedido")]
		public long Id { get; set; }
    }
}
