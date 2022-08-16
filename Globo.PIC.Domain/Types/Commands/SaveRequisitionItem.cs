using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.Entities;
using System.ComponentModel;
using System.Threading;

namespace Globo.PIC.Domain.Types.Commands
{
    /// <summary>
	/// 
	/// </summary>
    public class SaveRequisitionItem : DomainCommand
    {
        /// <summary>
		/// 
		/// </summary>
		[Description("PedidoItem")] 

        public PedidoItem PedidoItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Cancellation Token")]
        public CancellationToken CancellationToken { get; set; }
    }
}
