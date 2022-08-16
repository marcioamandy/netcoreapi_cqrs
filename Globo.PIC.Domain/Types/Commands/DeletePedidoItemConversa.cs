using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Commands
{
    public class DeletePedidoItemConversa : DomainCommand
    {
        /// <summary>
        /// id do pedido item
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// id do pedido
        /// </summary>
        public long IdPedidoItem { get; set; }
    }
}
