using System.ComponentModel;
using System.Threading;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Enums;
namespace Globo.PIC.Domain.Types.Commands
{
	/// <summary>
	/// 
	/// </summary>
	public class UpdateUser : DomainCommand
	{ 

		/// <summary>
		/// 
		/// </summary>
		[Description("User")]
		public Usuario User { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("Perfis")]
		public Role[] Roles { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool Update { get; set; }

	}
}
