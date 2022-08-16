using System.ComponentModel;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Commands
{
    public class CreateItemCatalogo : DomainCommand
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
        public long ItemId { get; set; }

        /// <summary>
		/// 
		/// </summary>
		[Description("ItemCatalogo")]
        public ItemCatalogo ItemCatalogo { get; set; }
    }
}