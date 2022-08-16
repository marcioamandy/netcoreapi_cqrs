using Globo.PIC.Domain.Entities;
using System.ComponentModel;

namespace Globo.PIC.Domain.Types.Commands
{
    /// <summary>
	/// 
	/// </summary>
    public class CreatePedidoItemArteEntrega : DomainCommand
    {

        /// <summary>
        /// 
        /// </summary>
        public long IdPedidoItem { get; set; }

        /// <summary>
		/// 
		/// </summary>
		[Description("PedidoItemArteEntrega")]
        public PedidoItemArteEntrega PedidoItemEntrega { get; set; }
    }
}
