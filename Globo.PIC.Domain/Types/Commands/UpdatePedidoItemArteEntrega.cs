using System.ComponentModel;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Commands
{
    public class UpdatePedidoItemArteEntrega : DomainCommand
    {

        /// <summary>
        /// 
        /// </summary>
        public long IdPedidoItem { get; set; }

        /// <summary>
        /// entidade pedido item
        /// </summary>
        [Description("PedidoItemArteEntrega")]
        public PedidoItemArteEntrega PedidoItemEntrega { get; set; }

    }
}
