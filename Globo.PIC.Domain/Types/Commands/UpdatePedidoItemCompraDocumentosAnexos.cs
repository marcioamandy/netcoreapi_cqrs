using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Commands
{
    public class UpdatePedidoItemCompraDocumentosAnexos : DomainCommand
    {
        /// <summary>
        /// entidade pedido item
        /// </summary>
        [Description("PedidoItemCompraAnexos")]
        public PedidoItemArteCompraDocumentoAnexo PedidoItemCompraAnexo { get; set; }

        /// <summary>
        /// entidade pedido item
        /// </summary>
        [Description("PedidoItemCompraAnexos")]
        public List<PedidoItemArteCompraDocumentoAnexo> PedidoItemCompraAnexos { get; set; }

        /// <summary>
        /// cancellation token
        /// </summary>
        public CancellationToken CancellationToken { get; set; }
    }
}
