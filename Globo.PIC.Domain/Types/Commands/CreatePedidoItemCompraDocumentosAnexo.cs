using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.Entities;
using System.ComponentModel;
using System.Threading;

namespace Globo.PIC.Domain.Types.Commands
{
    /// <summary>
	/// 
	/// </summary>
    public class CreatePedidoItemCompraDocumentosAnexo : DomainCommand
    {
        /// <summary>
		/// 
		/// </summary>
		[Description("PedidoItemCompraAnexo")]
        public PedidoItemArteCompraDocumentoAnexo PedidoItemCompraDocumentosAnexos { get; set; }
    }
}
