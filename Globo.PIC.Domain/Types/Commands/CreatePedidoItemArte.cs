using Globo.PIC.Domain.Entities;
using System.ComponentModel;

namespace Globo.PIC.Domain.Types.Commands
{
    /// <summary>
	/// 
	/// </summary>
    public class CreatePedidoItemArte : DomainCommand
    {
        /// <summary>
		/// 
		/// </summary>
		[Description("PedidoItemArte")]
        public PedidoItemArte PedidoItemArte { get; set; }
    }
}
