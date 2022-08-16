using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.Entities;
using System.ComponentModel;
using System.Threading;

namespace Globo.PIC.Domain.Types.Commands
{
    /// <summary>
	/// 
	/// </summary>
    public class CreatePedidoItemArteCompra : DomainCommand
    {

        /// <summary>
        /// 
        /// </summary>
        public long IdPedidoItem { get; set; }

        /// <summary>
		/// 
		/// </summary>
		[Description("PedidoItemArteCompra")]
        public PedidoItemArteCompra PedidoItemArteCompra { get; set; }
    }
}
