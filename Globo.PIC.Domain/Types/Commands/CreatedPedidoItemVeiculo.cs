using System.ComponentModel;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Commands
{
    public class CreatedPedidoItemVeiculo : DomainCommand
    {
        /// <summary>
        /// entidade pedido item
        /// </summary>
        [Description("PedidoItemVeiculo")]
        public PedidoItemVeiculo PedidoItemVeiculo { get; set; }
    }
}
