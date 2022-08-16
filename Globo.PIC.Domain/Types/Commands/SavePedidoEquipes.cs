using Globo.PIC.Domain.Entities;
using System.ComponentModel;
using System.Collections.Generic;

namespace Globo.PIC.Domain.Types.Commands
{
    /// <summary>
	/// 
	/// </summary>
    public class SavePedidoEquipes : DomainCommand
    {
        /// <summary>
        /// entidade pedido item
        /// </summary>
        [Description("PedidoEquipes")]
        public List<Equipe> PedidoEquipes { get; set; }
    }
}
