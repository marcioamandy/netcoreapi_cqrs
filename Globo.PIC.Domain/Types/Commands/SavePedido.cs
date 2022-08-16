using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.Entities;
using System.ComponentModel;
using System.Threading;

namespace Globo.PIC.Domain.Types.Commands
{
    /// <summary>
	/// 
	/// </summary>
    public class SavePedido : DomainCommand
    {
        /// <summary>
		/// 
		/// </summary>
		[Description("Pedido")]
        public Pedido Pedido { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Cancellation Token")]
        public CancellationToken CancellationToken { get; set; }
    }
}
