using System.ComponentModel;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Commands
{
    public class UpdateAcionamentoItem : DomainCommand
    {

        /// <summary>
		/// 
		/// </summary>
		[Description("Acionamento Item")]
        public AcionamentoItem AcionamentoItem { get; set; }
    }
}
