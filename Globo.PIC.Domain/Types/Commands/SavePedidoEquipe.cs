using Globo.PIC.Domain.Entities;
using System.ComponentModel;

namespace Globo.PIC.Domain.Types.Commands
{
    /// <summary>
	/// 
	/// </summary>
    public class SavePedidoEquipe : DomainCommand
    {
        /// <summary>
		/// 
		/// </summary>
		[Description("PedidoEquipe")]
        public Equipe PedidoEquipe { get; set; }
    }
}
