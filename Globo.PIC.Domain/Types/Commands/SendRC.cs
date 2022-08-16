using Globo.PIC.Domain.Entities;
using System.ComponentModel;

namespace Globo.PIC.Domain.Types.Commands
{

    /// <summary>
	/// 
	/// </summary>
    public class SendRC : DomainCommand
    {

        /// <summary>
		/// 
		/// </summary>
		[Description("PedidoItem")]
        public PedidoItem[] PedidoItens { get; set; }

    }
}
