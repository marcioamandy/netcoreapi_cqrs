using MediatR;
using System.ComponentModel; 
using Globo.PIC.Domain.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.Types.Queries
{
	/// <summary>
	/// 
	/// </summary>
	public class GetRolesByLogin : IRequest<List<UserRole>>
	{ 

		/// <summary>
		/// 
		/// </summary>
		[Required]
		[Description("Login do Usuário")]
		public string Login { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Required]
		[Description("Role")]
		public string Name { get; set; }

		public virtual Usuario User { get; set; }
	}
}
