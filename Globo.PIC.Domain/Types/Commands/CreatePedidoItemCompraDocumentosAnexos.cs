using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.Entities;
using System.ComponentModel;
using System.Threading;
using System.Collections.Generic;

namespace Globo.PIC.Domain.Types.Commands
{
    /// <summary>
	/// 
	/// </summary>
    public class CreatePedidoItemCompraDocumentosAnexos : DomainCommand
    {
        /// <summary>
        /// entidade pedido item
        /// </summary>
        [Description("PedidoItemCompraAnexos")]
        public PedidoItemArteCompraDocumentoAnexo PedidoItemCompraDocumentosAnexo { get; set; }

        /// <summary>
        /// entidade pedido item
        /// </summary>
        [Description("PedidoItemCompraAnexos")]
        public List<PedidoItemArteCompraDocumentoAnexo> PedidoItemCompraDocumentosAnexos { get; set; }

        /// <summary>
        /// cancellation token
        /// </summary>
        public CancellationToken CancellationToken { get; set; }
    }
}
