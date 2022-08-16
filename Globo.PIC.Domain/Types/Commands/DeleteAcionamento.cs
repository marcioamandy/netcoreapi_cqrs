using System.ComponentModel;
using System.Threading;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Commands
{
    public class DeleteAcionamento : DomainCommand
	{

		/// <summary>
		/// 
		/// </summary>
		[Description("Acionamento")]
		public Acionamento Acionamento { get; set; }

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
