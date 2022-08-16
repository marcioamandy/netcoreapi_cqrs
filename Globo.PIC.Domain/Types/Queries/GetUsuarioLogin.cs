using MediatR;
using System.ComponentModel;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Queries
{
	/// <summary>
	/// 
	/// </summary>
	public class GetUsuarioLogin : IRequest<Usuario>
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("Login")]
		public string Login { get; set; }
	}
}
