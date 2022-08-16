using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Commands
{
    public class DeletePedidoItemCompraDocumentos : DomainCommand
    {
        /// <summary>
        /// id do pedido item Compra Documentos
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// id do pedido
        /// </summary>
        public long IdCompra  { get; set; }
    }
}
