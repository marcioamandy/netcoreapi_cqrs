using Globo.PIC.Domain.Entities;
using MediatR;

namespace Globo.PIC.Domain.Types.Events
{

	/// <summary>
	/// 
	/// </summary>
	public class OnPedidoFinalizado : INotification
	{

		/// <summary>
		/// 
		/// </summary>
		public Pedido Pedido { get; set; }

	}
}
