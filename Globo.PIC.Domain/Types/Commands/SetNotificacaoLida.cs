using System.ComponentModel;
using System.Threading;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Commands
{
    public class SetNotificacaoLida : DomainCommand
    {

		/// <summary>
		/// 
		/// </summary>
		[Description("Notificação")]
		public Notificacao Notification { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool IsRead { get; set; }

	}
}
