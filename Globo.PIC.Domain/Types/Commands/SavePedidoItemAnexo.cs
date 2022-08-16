using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.Entities;
using System.ComponentModel;
using System.Threading;

namespace Globo.PIC.Domain.Types.Commands
{
    /// <summary>
	/// 
	/// </summary>
    public class SavePedidoItemAnexo : DomainCommand
    {
        /// <summary>
		/// 
		/// </summary>
		[Description("PedidoItemImagem")]
        public PedidoItemAnexo PedidoItemAnexos { get; set; }
    }
}
