using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Commands
{
    public class DeleteAllPedidoItemArte : DomainCommand
    {

        /// <summary>
        /// id do pedido
        /// </summary>
        public long IdPedido { get; set; }

    }
}
