using System.ComponentModel;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Commands
{
    public class CreateItemVeiculo : DomainCommand
    {

        /// <summary>
        /// 
        /// </summary>
        public long PedidoId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long PedidoItemId { get; set; }

        /// <summary>
		/// 
		/// </summary>
		[Description("ItemVeiculo")]
        public ItemVeiculo ItemVeiculo { get; set; }
    }
}