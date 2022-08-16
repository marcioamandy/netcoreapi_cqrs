using System.ComponentModel;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Commands
{
    public class MudarStatusPedidoItemVeiculo : DomainCommand
    {
        /// <summary>
        /// Pedido
        /// </summary>
        [Description("Id do pedido item para mudar o status")]
        public long IdPedidoItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Justifictiva do Cancelamento")]
        public string JustificativaCancelamento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Justifictiva do Devolução")]
        public string JustificativaDevolucao { get; set; }

        /// <summary>
        /// StatusPedidoVeiculo
        /// </summary>
        [Description("Id do novo status")]
        public long IdStatus { get; set; }

    }
}
