using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Commands
{
    public class UpdatePedidoItemCompraDocumento : DomainCommand
    {
        /// <summary>
        /// entidade pedido item
        /// </summary>
        [Description("PedidoItemCompraDocumentos")]
        public PedidoItemArteCompraDocumento PedidoItemCompraDocumentos { get; set; }

    }
}
