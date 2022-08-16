using System.ComponentModel;
using System.Threading;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Commands
{
    public class DeleteAcionamentoItem : DomainCommand
	{

		/// <summary>
		/// 
		/// </summary>
		[Description("Acionamento Item")]
		public AcionamentoItem AcionamentoItem { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public long id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public CancellationToken CancellationToken { get; set; }
	}
}
