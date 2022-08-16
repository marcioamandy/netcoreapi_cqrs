using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Globo.PIC.API.Claims;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Types.Queries;

namespace Globo.PIC.API.Auth
{
	/// <summary>
	/// 
	/// </summary>
	public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
	{
		/// <summary>
		/// 
		/// </summary>
		private readonly IMediator mediator;

		/// <summary>
		/// 
		/// </summary>		
		private readonly IUserProvider userProvider;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="options"></param>
		/// <param name="logger"></param>
		/// <param name="encoder"></param>
		/// <param name="clock"></param>
		/// <param name="_mediator"></param>
		/// <param name="_userProvider"></param>
		public BasicAuthenticationHandler(
			IOptionsMonitor<AuthenticationSchemeOptions> options,
			ILoggerFactory logger,
			UrlEncoder encoder,
			ISystemClock clock,
			IMediator _mediator,
			IUserProvider _userProvider)
			: base(options, logger, encoder, clock)
		{
			mediator = _mediator;
			userProvider = _userProvider;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			if (!Request.Headers.Keys.Any(k => !string.IsNullOrWhiteSpace(k) && k.ToLower().Equals("authorization")))
				return AuthenticateResult.Fail($@"Missing Authorization Header: \n Request: { 
					JsonConvert.SerializeObject(new {
						Request.Host,
						Request.QueryString,
						Request.Query,
						Request.Headers,
						Request.IsHttps,
						Request.Method,
						Request.Protocol,
						Request.PathBase,
						Request.Path
					})} \n Header: {Request.Headers["Authorization"]}");

			var token = userProvider.GetAccessToken();

            if (string.IsNullOrWhiteSpace(token.Login))
            {
				return AuthenticateResult.Fail("Token Inesperado.");
			}

			var user = await mediator.Send(new GetUsuarioLogin()
			{
				Login = token.Login
			});

			if (user == null) user = new Usuario() { Login = token.Login };

			user.Name = token.Name;
			user.LastName = token.LastName;
			user.Email = token.Email;
			 
			var claims = new List<Claim>();

			claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Login));
			claims.Add(new Claim(ClaimTypes.Name, string.Format("{0} {1}", user.Name, user.LastName)));
			claims.Add(new Claim(ClaimTypes.GivenName, user.Name ?? string.Empty));
			claims.Add(new Claim(ClaimTypes.Surname, user.LastName ?? string.Empty));
			claims.Add(new Claim(ClaimTypes.Email, user.Email ?? string.Empty));
			claims.Add(new Claim(AppClaimTypes.IsActive, user.IsActive.ToString(), ClaimValueTypes.Boolean));
			claims.Add(new Claim(AppClaimTypes.User, Serialize(user)));

			if (user.Roles != null && user.Roles.Any())
			{
				//Resgata todos os perfis.
				var roles = user.Roles.Where(r => r.Name.StartsWith("PERFIL"));								
				
				//verifica se resta apenas uma e adiciona.
				if (roles.Count() == 1)
					claims.Add(new Claim(ClaimTypes.Role, roles.FirstOrDefault().Name));				

				//Resgata todas as grants.
				var grants = user.Roles.Where(r => r.Name.StartsWith("GRANT"));

				foreach (var grant in grants)
				{
					claims.Add(new Claim(ClaimTypes.Role, grant.Name));
				}
			}

			var identity = new ClaimsIdentity(claims, Scheme.Name);
			var principal = new ClaimsPrincipal(identity);
			var ticket = new AuthenticationTicket(principal, Scheme.Name);

			return AuthenticateResult.Success(ticket);
		}

		/// <summary>
		/// Serializes Objects preventing the Loop Reference error
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		private static string Serialize(object obj)
		{
			return JsonConvert.SerializeObject(obj, Formatting.Indented,
				new JsonSerializerSettings()
				{
					ReferenceLoopHandling = ReferenceLoopHandling.Ignore
				});
		}
	}
}
