using Globo.PIC.Domain.Entities;
using System.ComponentModel;

namespace Globo.PIC.Domain.Types.Commands
{

    /// <summary>
	/// 
	/// </summary>
    public class CreatePedidoVeiculo : DomainCommand
    {

        /// <summary>
		/// 
		/// </summary>
		[Description("Pedido Veiculo")]
        public PedidoVeiculo PedidoVeiculo { get; set; }

    }
}
