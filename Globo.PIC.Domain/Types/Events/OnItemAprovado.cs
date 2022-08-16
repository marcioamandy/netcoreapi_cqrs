using Globo.PIC.Domain.Entities;
using MediatR;

namespace Globo.PIC.Domain.Types.Events
{

	/// <summary>
	/// 
	/// </summary>
	public class OnItemEntregue : INotification
	{

		/// <summary>
		/// 
		/// </summary>
		public PedidoItem PedidoItem { get; set; }

	}
}
