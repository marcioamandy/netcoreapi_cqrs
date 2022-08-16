using Globo.PIC.Domain.Entities;
using MediatR;

namespace Globo.PIC.Domain.Types.Events
{

	/// <summary>
	/// 
	/// </summary>
	public class OnStatusRCAlterada : INotification {

		/// <summary>
        /// 
        /// </summary>
		public RC RC { get; set; }
	}
}
