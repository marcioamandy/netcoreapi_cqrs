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
    public class SavePedidoItemConversaAnexos : DomainCommand
    {
        /// <summary>
        /// entidade pedido item
        /// </summary>
        [Description("PedidoItemConversaAnexos")]
        public List<PedidoItemConversaAnexo> PedidoItemConversaAnexos { get; set; }

        /// <summary>
        /// cancellation token
        /// </summary>
        public CancellationToken CancellationToken { get; set; }
    }
}
