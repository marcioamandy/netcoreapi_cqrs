using Globo.PIC.Domain.Entities;
using MediatR;

namespace Globo.PIC.Domain.Types.Events
{
    public class OnPedidoItemArteCriado : INotification
    {

        /// <summary>
        /// 
        /// </summary>
        public PedidoItemArte PedidoItemArte { get; set; }
        
    }
}
