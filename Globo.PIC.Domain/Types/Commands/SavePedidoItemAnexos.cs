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
    public class SavePedidoItemAnexos : DomainCommand
    {
        /// <summary>
        /// entidade pedido item
        /// </summary>
        [Description("PedidoItemAnexos")]
        public List<PedidoItemAnexo> PedidoItemAnexos { get; set; }

        /// <summary>
        /// cancellation token
        /// </summary>
        public CancellationToken CancellationToken { get; set; }
    }
}
