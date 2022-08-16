using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.Entities;
using System.ComponentModel;
using System.Threading;

namespace Globo.PIC.Domain.Types.Commands
{
    /// <summary>
	/// 
	/// </summary>
    public class CreatePedidoItemCompraDocumento : DomainCommand
    {
        /// <summary>
		/// 
		/// </summary>
		[Description("PedidoItemCompraDocumento")]
        public PedidoItemArteCompraDocumento PedidoItemCompraDocumentos { get; set; }
    }
}
