using System.Collections.Generic;
using System.Linq;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Models
{

	/// <summary>
    /// 
    /// </summary>
	public class IdentityUser
	{

		/// <summary>
		/// Login de Usuário.
		/// </summary>
		public readonly string Login;

		/// <summary>
		/// Nome do usuário.
		/// </summary>
		public readonly string Name;

		/// <summary>
		/// Sobrenome do usuário.
		/// </summary>
		public readonly string LastName;

		/// <summary>
		/// Apelido do usuário.
		/// </summary>
		public readonly string Apelido;

		/// <summary>
		/// Email do usuário.
		/// </summary>
		public readonly string Email;

		/// <summary>
		/// Está ativo?
		/// </summary>
		public readonly bool IsActive;

		/// <summary>
		/// Roles do usuario
		/// </summary>
		public readonly IEnumerable<UserRole> Roles;

		/// <summary>
		/// Autorização do usuário.
		/// </summary>
		public readonly Authorization Authorization;

		/// <summary>
        /// 
        /// </summary>
		public Departamento Departamento { get; set; }

		/// <summary>
        /// 
        /// </summary>
		public  UnidadeNegocio UnidadeNegocio { get; set; }

		/// <summary>
        /// 
        /// </summary>
		public List<Conteudo> Conteudos { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="user"></param>
		/// <param name="authorization"></param>
		public IdentityUser(Usuario user, Authorization authorization)
		{
			Name = user.Name;
			LastName = user.LastName;
			Apelido = user.Apelido; 
			Login = user.Login;
			Email = user.Email;
			Roles = user.Roles;
			IsActive = user.IsActive;
			Authorization = authorization;
			Departamento = user.Departamento;
			UnidadeNegocio = user.UnidadeNegocio;
			Conteudos = user.UsuariosConteudos.Select(x => x.Conteudo).ToList();
		}
	}
}
