using System.ComponentModel;

namespace Globo.PIC.Domain.Types.Commands
{

    /// <summary>
    /// 
    /// </summary>
    public class MudarCompradorPedidoItemArte : DomainCommand
    {

        /// <summary>
        /// Pedido
        /// </summary>
        [Description("Id do pedido para mudar o comprador")]
        public long IdPedido { get; set; }

        /// <summary>
        /// Pedido
        /// </summary>
        [Description("Id do pedido item para mudar o comprador")]
        public long IdPedidoItem { get; set; }

        /// <summary>
        /// Pedido
        /// </summary>
        [Description("Id do tipo item para mudar o comprador")]
        public long IdTipo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Login do novo comprador")]
        public string Comprador { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Login do antigo comprador")]
        public string CompradorAnterior { get; set; }

        /// <summary>
		/// 
		/// </summary>
		[Description("Justificativa da atribuição")]
        public string Justificativa { get; set; }

    }
}
