using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.Entities;
using System.ComponentModel;
using System.Threading;

namespace Globo.PIC.Domain.Types.Commands
{
    /// <summary>
	/// 
	/// </summary>
    public class SavePedidoAnexo : DomainCommand
    {
        /// <summary>
		/// 
		/// </summary>
		[Description("PedidoAnexos")]
        public PedidoAnexo PedidoAnexos { get; set; }
    }
}
