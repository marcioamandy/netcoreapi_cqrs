using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Enums;
using Globo.PIC.Domain.Models;

namespace Globo.PIC.Domain.Interfaces
{
	/// <summary>
	/// O objetivo deste provedor é fornece para todas as camadas que dependem de um usuário atenticado, 
	/// uma interface simplificada e adequada ao modelos da aplicação.
	/// </summary>
	public interface IUserProvider
	{

		/// <summary>
		/// Usuario Principal no Contexto HTTP.
		/// </summary>
		ClaimsPrincipal Principal { get; }

		/// <summary>
		/// IdentityUser carregado apartir do Usuario Principal no Contexto HTTP.
		/// </summary>
		/// <returns></returns>
		IdentityUser User { get; }

		/// <summary>
		///  Encapsulamento da função IsInRole, com suporte ao tipo de enum Roles proprietário da aplicação.
		/// </summary>
		/// <param name="role"></param>
		/// <returns></returns>
		bool IsRole(Role role);

		/// <summary>
		/// Extrai login do JWT.
		/// </summary>
		/// <returns>
		/// Retorna objeto dinâmico com a seguinte estrutura: Login, Name, LastName, Email.
		/// </returns>
		TokenUser GetAccessToken();
	}
}
