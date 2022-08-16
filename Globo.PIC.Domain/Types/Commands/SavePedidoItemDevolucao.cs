using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.Entities;
using System.ComponentModel;
using System.Threading;

namespace Globo.PIC.Domain.Types.Commands
{
    /// <summary>
	/// 
	/// </summary>
    public class SavePedidoItemDevolucao : DomainCommand
    {
        /// <summary>
		/// 
		/// </summary>
		[Description("PedidoItemDevolucao")]
        public PedidoItemArteDevolucao PedidoItemDevolucao { get; set; }
    }
}
