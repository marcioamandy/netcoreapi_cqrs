using System.Collections.Generic;
using System.ComponentModel;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Commands
{
    public class UpdatePedidoItensVeiculo : DomainCommand
    {
        /// <summary>
        /// entidade pedido item
        /// </summary>
        [Description("PedidoItensVeiculo")]
        public List<PedidoItemVeiculo> PedidoItensVeiculo { get; set; }       
    }
}
