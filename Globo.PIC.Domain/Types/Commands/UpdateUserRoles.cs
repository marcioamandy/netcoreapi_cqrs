using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Commands
{
	/// <summary>
	/// 
	/// </summary>
	public class UpdateUserRoles : DomainCommand
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("Login")]
		public string Login { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("UserRoles")]
		public List<UserRole> UserRoles { get; set; }

		/// <summary>
		/// cancellation token
		/// </summary>
		public CancellationToken CancellationToken { get; set; }
	}
}
