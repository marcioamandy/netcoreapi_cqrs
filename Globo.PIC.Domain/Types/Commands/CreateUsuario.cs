using Globo.PIC.Domain.Entities;
using System.ComponentModel;

namespace Globo.PIC.Domain.Types.Commands
{
	/// <summary>
	/// 
	/// </summary>
	public class CreateUsuario : DomainCommand
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("Usuário")]
		public Usuario Usuario { get; set; }
	}
}
