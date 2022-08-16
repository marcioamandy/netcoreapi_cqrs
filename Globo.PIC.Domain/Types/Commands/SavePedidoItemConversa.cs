using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.Entities;
using System.ComponentModel;
using System.Threading;

namespace Globo.PIC.Domain.Types.Commands
{
    /// <summary>
	/// 
	/// </summary>
    public class SavePedidoItemConversa : DomainCommand
    {
        /// <summary>
		/// 
		/// </summary>
		[Description("PedidoItemConversa")]
        public PedidoItemConversa PedidoItemConversa { get; set; }
    }
}
