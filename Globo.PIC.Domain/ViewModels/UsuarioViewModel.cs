using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.ViewModels
{
	/// <summary>
    /// 
    /// </summary>
	public class UsuarioViewModel
	{

		/// <summary>
		/// 
		/// </summary>
		[Key]
		[Required]
		[Description("Login do Usuário")]
		public string Login { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("Nome")]
		public string Name { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("Sobrenome")]
		public string LastName { get; set; }


		/// <summary>
		/// 
		/// </summary>
		[Description("Apelido")]
		public string Apelido { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("Email")]
        public string Email { get; set; }
				
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Description("Ativo")]
        public bool IsActive { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("IdDepartamento")]
		public long? IdDepartamento { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("IdDepartamento")]
		public long? IdUnidadeNegocio { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<UserRoleViewModel> Roles { get; set; }

		/// <summary>
        /// 
        /// </summary>
		public UnidadeNegocioViewModel UnidadeNegocio { get; set; }

		/// <summary>
        /// 
        /// </summary>
		public DepartamentoViewModel Departamento { get; set; }

		/// <summary>
        /// 
        /// </summary>
		public List<UsuarioConteudoViewModel> Conteudos { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public UsuarioViewModel()
		{
		}
	}
}
