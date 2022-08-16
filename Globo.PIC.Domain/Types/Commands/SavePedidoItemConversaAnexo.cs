using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.Entities;
using System.ComponentModel;
using System.Threading;

namespace Globo.PIC.Domain.Types.Commands
{
    /// <summary>
	/// 
	/// </summary>
    public class SavePedidoItemConversaAnexo : DomainCommand
    {
        /// <summary>
		/// 
		/// </summary>
		[Description("PedidoItemConversaAnexo")]
        public PedidoItemConversaAnexo PedidoItemConversaAnexo { get; set; }
    }
}
