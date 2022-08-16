using System.ComponentModel;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Commands
{
    public class UpdateAcionamento : DomainCommand
    {

        /// <summary>
		/// 
		/// </summary>
		[Description("Acionamento")]
        public Acionamento Acionamento { get; set; }
    }
}
