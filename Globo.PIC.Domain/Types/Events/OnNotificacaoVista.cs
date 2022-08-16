using MediatR;

namespace Globo.PIC.Domain.Types.Events
{
	public class OnNotificacaoVista : INotification
	{

		/// <summary>
		/// 
		/// </summary>
		public long[] NotificacaoIds { get; set; }

	}
}
