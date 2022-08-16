using System.ComponentModel;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Commands
{
    public class VincularCompradorPedidoVeiculo : DomainCommand
    {
        /// <summary>
        /// Pedido
        /// </summary>
        [Description("Id do pedido para mudar o status")]
        public long IdPedido { get; set; }

        /// <summary>
        /// StatusPedidoVeiculo
        /// </summary>
        [Description("Id do novo status")]
        public long IdStatus { get; set; }

    }
}
