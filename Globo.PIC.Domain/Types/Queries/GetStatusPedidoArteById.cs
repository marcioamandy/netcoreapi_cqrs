using MediatR;
using System.ComponentModel;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Queries
{

	/// <summary>
	/// 
	/// </summary>
	public class GetStatusPedidoArteById :
		IRequest<StatusPedidoArte>
	{

		/// <summary>
		/// 
		/// </summary>
		[Description("Id")]
		public long Id { get; set; }
	}
}
