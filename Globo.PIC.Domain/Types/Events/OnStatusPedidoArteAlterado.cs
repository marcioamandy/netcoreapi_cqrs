using Globo.PIC.Domain.Entities;
using MediatR;

namespace Globo.PIC.Domain.Types.Events
{
    public class OnStatusPedidoArteAlterado : INotification
    {

        /// <summary>
        /// 
        /// </summary>
        public PedidoArte PedidoArte { get; set; }

    }
}
