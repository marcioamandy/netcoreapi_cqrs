using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;
using Globo.PIC.API.Requirements;
using Globo.PIC.API.Claims;
using Globo.PIC.Domain.Enums;
namespace Globo.PIC.API.Auth
{

	/// <summary>
	/// 
	/// </summary>
	public class MatchUniqueProfileHandler : AuthorizationHandler<MatchUniqueProfileRequirement>
	{

		IHttpContextAccessor accessor;

		public MatchUniqueProfileHandler(IHttpContextAccessor _httpContextAccessor)
		{
			accessor = _httpContextAccessor;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="requirement"></param>
		/// <returns></returns>
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
												   MatchUniqueProfileRequirement requirement)
		{
			//Verifica se o usuario está ativo.
			if (context.User.HasClaim(c => c.Type == AppClaimTypes.IsActive && c.Value == bool.FalseString))
				return Task.CompletedTask;

			var roles = context.User.Claims.Where(r => r.Value.StartsWith("PERFIL"));
			//Verifica se o usuário possui uma e apenas uma role.
			if (roles.Count() < 1)
				return Task.CompletedTask;
			
			//verifica se o perfil do banco está em um enum de roles
			var exist = context.User.Claims.Where(r => r.Value.StartsWith("PERFIL"))
				.Select(a => a.Value).All(Enum.GetNames(typeof(Role)).Contains);

			if (!exist)
				return Task.CompletedTask;

			var role = roles.FirstOrDefault();

			context.Succeed(requirement);

			return Task.CompletedTask;
		}
	}
}
