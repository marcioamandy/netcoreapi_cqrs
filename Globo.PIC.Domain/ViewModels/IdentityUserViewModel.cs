using System.Collections.Generic;

namespace Globo.PIC.Domain.ViewModels
{
	public class IdentityUserViewModel
	{
		/// <summary>
		/// Nome do usuário.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Sobrenome do usuário.
		/// </summary>
		public string LastName { get; set; }


		/// <summary>
		/// Apelido do usuário.
		/// </summary>
		public string Apelido { get; set; }

		/// <summary>
		/// Nome de Usuário.
		/// </summary>
		public string Login { get; set; }

		/// <summary>
		/// Email do usuário.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<RoleViewModel> Roles { get; set; }

		/// <summary>
		/// Tem area?
		/// </summary>
		public bool IsActive { get; set; }

		/// <summary>
		/// Autorização do usuário.
		/// </summary>
		public AuthorizationViewModel Authorization { get; set; }

		/// <summary>
        /// 
        /// </summary>
		public UnidadeNegocioViewModel UnidadeNegocio { get; set; }

		/// <summary>
        /// 
        /// </summary>
		public DepartamentoViewModel Departamento { get; set; }

		public IdentityUserViewModel()
		{
			Authorization = new AuthorizationViewModel() { Roles = new string [] { } };
			Roles = new List<RoleViewModel>();
		}
	}
}
