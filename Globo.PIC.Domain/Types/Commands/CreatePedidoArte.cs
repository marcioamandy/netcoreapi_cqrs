using Globo.PIC.Domain.Entities;
using System.ComponentModel;

namespace Globo.PIC.Domain.Types.Commands
{

    /// <summary>
	/// 
	/// </summary>
    public class CreatePedidoArte : DomainCommand
    {

        /// <summary>
		/// 
		/// </summary>
		[Description("Pedido")]
        public PedidoArte PedidoArte { get; set; }

    }
}
