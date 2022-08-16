using Globo.PIC.Domain.Entities;
using System.ComponentModel;

namespace Globo.PIC.Domain.Types.Commands
{
	/// <summary>
	/// 
	/// </summary>
	public class UpdateUsuario : DomainCommand
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("User")]
		public Usuario Usuario { get; set; }
	}
}
