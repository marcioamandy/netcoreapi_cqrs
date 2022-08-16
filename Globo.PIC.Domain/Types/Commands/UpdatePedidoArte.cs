using System.ComponentModel;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Commands
{
    public class UpdatePedidoArte : DomainCommand
    {
        /// <summary>
        /// entidade pedido item
        /// </summary>
        [Description("PedidoArte")]
        public PedidoArte PedidoArte { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("Observacao")]
		public string Observacao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("Login Comprador Pedido")]
		public string LoginComprador { get; set; }
	}
}
