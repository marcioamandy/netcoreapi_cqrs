using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.Entities;
using System.ComponentModel;
using System.Threading;

namespace Globo.PIC.Domain.Types.Commands
{
    /// <summary>
	/// 
	/// </summary>
    public class SaveRequisition : DomainCommand
    {
        /// <summary>
		/// 
		/// </summary>
		[Description("IdPedido")]
        public long IdPedido { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Cancellation Token")]
        public CancellationToken CancellationToken { get; set; }
    }
}
