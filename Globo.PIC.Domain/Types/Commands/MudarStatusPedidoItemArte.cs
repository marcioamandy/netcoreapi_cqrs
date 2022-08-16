using System.ComponentModel;

namespace Globo.PIC.Domain.Types.Commands
{

    /// <summary>
    /// 
    /// </summary>
    public class MudarStatusPedidoItemArte : DomainCommand
    {

        /// <summary>
        /// Pedido
        /// </summary>
        [Description("Id do pedido para mudar o status")]
        public long IdPedido { get; set; }

        /// <summary>
        /// Pedido
        /// </summary>
        [Description("Id do pedido item para mudar o status")]
        public long[] IdPedidoItens { get; set; }

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
