using Globo.PIC.Domain.Entities;
using MediatR;

namespace Globo.PIC.Domain.Types.Events
{
    public class OnDLCancelationLine : INotification
	{

		/// <summary>
		/// 
		/// </summary>
		public PedidoItem PedidoItem { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public RC Rc { get; set; }
	}
}
