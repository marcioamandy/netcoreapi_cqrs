using System.Collections.Generic;
using System.ComponentModel;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Commands
{

    public class UpdatePedidoItemEntregas : DomainCommand
    {

        /// <summary>
        /// 
        /// </summary>
        public long IdPedidoItem { get; set; }

        /// <summary>
        /// entidade pedido item
        /// </summary>
        [Description("PedidoItemEntregas")]
        public List<PedidoItemArteEntrega> PedidoItemEntregas { get; set; }

    }
}
