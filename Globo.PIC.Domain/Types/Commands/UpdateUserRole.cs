using System.ComponentModel;
using System.Threading;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Commands
{
	/// <summary>
	/// 
	/// </summary>
	public class UpdateUserRole : DomainCommand
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
