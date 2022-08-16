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
    public class CreatePedidoItemArteEntregas : DomainCommand
    {

        /// <summary>
        /// 
        /// </summary>
        public long IdPedidoItem { get; set; }

        /// <summary>
        /// entidade pedido item
        /// </summary>
        [Description("PedidoItemArteEntregas")]
        public List<PedidoItemArteEntrega> PedidoItemEntregas { get; set; }

    }
}
