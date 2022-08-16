using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.Entities;
using System.ComponentModel;
using System.Threading;

namespace Globo.PIC.Domain.Types.Commands
{
    public class SaveNotificacao : DomainCommand
    {

        /// <summary>
		/// 
		/// </summary>
		[Description("Notificacao")]
        public Notificacao Notificacao { get; set; }

    }
}
