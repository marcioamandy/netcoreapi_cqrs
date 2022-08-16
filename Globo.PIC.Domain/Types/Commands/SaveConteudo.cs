using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.Entities;
using System.ComponentModel;
using System.Threading;

namespace Globo.PIC.Domain.Types.Commands
{
    public class SaveConteudo : DomainCommand
    {
        /// <summary>
		/// 
		/// </summary>
		[Description("Conteudo")]
        public Conteudo Conteudo { get; set; }
    }
}
