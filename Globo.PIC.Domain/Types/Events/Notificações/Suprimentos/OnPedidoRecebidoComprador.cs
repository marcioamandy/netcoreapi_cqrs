using Globo.PIC.Domain.Entities;
using MediatR;

namespace Globo.PIC.Domain.Types.Events.Notificações.Suprimentos
{
    public class OnPedidoRecebidoComprador : INotification
	{

		/// <summary>
		/// 
		/// </summary>
		public Pedido Pedido { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public PedidoItem PedidoItem { get; set; }
	}
}
