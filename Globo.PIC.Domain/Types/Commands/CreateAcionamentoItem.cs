using Globo.PIC.Domain.Entities;
using System.ComponentModel;

namespace Globo.PIC.Domain.Types.Commands
{
    public class CreateAcionamentoItem : DomainCommand
    {

        /// <summary>
		/// 
		/// </summary>
		[Description("Acionamento Item")]
        public AcionamentoItem AcionamentoItem { get; set; }
    }
}
