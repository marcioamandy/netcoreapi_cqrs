using System.ComponentModel;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Commands
{
    public class UpdatePedido : DomainCommand
    {
        /// <summary>
        /// entidade pedido
        /// </summary>
        [Description("Pedido")]
        public Pedido Pedido { get; set; }
    }
}
