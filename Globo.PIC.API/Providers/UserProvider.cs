using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Linq;
using System.Security.Claims;
using Globo.PIC.API.Claims;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Enums;
using Globo.PIC.Domain.Extensions;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Models;
using System.Threading;

namespace Globo.PIC.API.Providers
{
	/// <summary>
	/// O objetivo deste provedor é fornece para todas as camadas que dependem de um usuário autenticado, 
	/// uma interface simplificada e adequada aos modelos da aplicação.
	/// </summary>
	public class UserProvider : IUserProvider
	{

		/// <summary>
		/// 
		/// </summary>
		private readonly IHttpContextAccessor request;
		 
		/// <summary>
		/// 
		/// </summary>
		/// <param name="_request"></param>
		public UserProvider(IHttpContextAccessor _request )
		{
			request = _request; 
		}

		/// <summary>
		/// IdentityUser carregado apartir do Usuario Principal no Contexto HTTP.
		/// </summary>
		/// <returns></returns>
		public IdentityUser User { get => GetIdentity(); }

		/// <summary>
		/// Usuario Principal no Contexto HTTP.
		/// </summary>
		/// <returns></returns>
		public ClaimsPrincipal Principal { get => request.HttpContext?.User; }

		/// <summary>
		/// Encapsulamento da função IsInRole, com suporte ao tipo de enum Roles proprietário da aplicação.
		/// </summary>
		/// <param name="role"></param>
		/// <returns></returns>
		public bool IsRole(Role role) => request.HttpContext?.User == null ? false : request.HttpContext.User.IsInRole(role.ToString());
		 
		/// <summary>
		/// Preenche Identidade do Usuario com as informações do Usuario Principal.
		/// </summary>
		/// <returns></returns>
		private IdentityUser GetIdentity()
		{
			if (Principal == null) return null;

			var userSerialized = Principal.FindFirst(AppClaimTypes.User);
			var user = JsonConvert.DeserializeObject<Usuario>(userSerialized.Value);

			CancellationTokenSource cancellationToken = new CancellationTokenSource();

			var Authorization = new Authorization()
			{
				Roles = Principal.FindAll(r => 
					new string[] { ClaimTypes.Role, ClaimTypes.UserData }.Contains(r.Type)
				).Select(r => r.Value)
			};

			return new IdentityUser(user, Authorization);
		}

		/// <summary>
		/// Recupera informações do token passado no request.
		/// </summary>
		/// <returns>
		/// Retorna objeto dinâmico com a seguinte estrutura: Login, Name, LastName, Email.
		/// </returns>
		public TokenUser GetAccessToken()
		{
			var auth = new TokenUser();

			try
			{
				var login = request.HttpContext.Request.Headers["Authorization"].ToString().JWTDecode("custom:login");
				var accountName = request.HttpContext.Request.Headers["Authorization"].ToString().JWTDecode("custom:accountName");
				var given_name = request.HttpContext.Request.Headers["Authorization"].ToString().JWTDecode("given_name");
				var family_name = request.HttpContext.Request.Headers["Authorization"].ToString().JWTDecode("family_name");
				var email = request.HttpContext.Request.Headers["Authorization"].ToString().JWTDecode("email");
				var emailProvider = request.HttpContext.Request.Headers["Authorization"].ToString().JWTDecode("cognito:username");

				//Obs.: Por conta de variação no padrao retornado pelo cognito, ocorrem algumas tentativas de recuperar um login
				if (!string.IsNullOrWhiteSpace(accountName) && accountName.IndexOf('@') < 0)
					auth.Login = accountName;

				if (!string.IsNullOrWhiteSpace(login) && login.IndexOf('@') < 0)
					auth.Login = login;

				if (string.IsNullOrWhiteSpace(auth.Login))
					auth.Login = email;

				if (string.IsNullOrWhiteSpace(auth.Login))
					auth.Login = emailProvider.Replace("adcognitoprovider_", string.Empty).Replace("cognito_", string.Empty).Replace("Cognito-login_", string.Empty);

				auth.Login = auth.Login.ToLower();

				auth.Name = given_name;
				auth.LastName = family_name;
				auth.Email = email;

			}
			catch { }

			return auth;
		}
	}
}
