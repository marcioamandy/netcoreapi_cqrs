using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.Entities;
using System.ComponentModel;
using System.Threading;

namespace Globo.PIC.Domain.Types.Commands
{
	/// <summary>
	/// 
	/// </summary>
	public class SaveUserRole : DomainCommand
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("UserRole")]
		public UserRole UserRole { get; set; }

		/// <summary>
		/// cancellation token
		/// </summary>
		public CancellationToken CancellationToken { get; set; }
	}
}
