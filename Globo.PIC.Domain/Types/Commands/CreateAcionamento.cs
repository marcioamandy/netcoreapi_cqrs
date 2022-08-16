using Globo.PIC.Domain.Entities;
using System.ComponentModel;

namespace Globo.PIC.Domain.Types.Commands
{
    public class CreateAcionamento : DomainCommand
    {

        /// <summary>
		/// 
		/// </summary>
		[Description("Acionamento")]
        public Acionamento Acionamento { get; set; }
    }
}
