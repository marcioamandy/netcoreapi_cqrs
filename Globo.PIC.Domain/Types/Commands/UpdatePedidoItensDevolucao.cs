using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Commands
{
    public class UpdatePedidoItensDevolucao : DomainCommand
    {
        /// <summary>
        /// entidade pedido item
        /// </summary>
        [Description("Pedido")]
        public long IdPedido { get; set; }

        /// <summary>
        /// entidade pedido item
        /// </summary>
        [Description("PedidoItemDevolucao")]
        public PedidoItemArteDevolucao PedidoItemDevolucao { get; set; }

    }
}
