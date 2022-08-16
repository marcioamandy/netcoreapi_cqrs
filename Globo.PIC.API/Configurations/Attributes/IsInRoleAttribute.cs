using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Globo.PIC.API.Configurations.Attributes
{
	/// <summary>
	/// 
	/// </summary>
	public class IsInRoleAttribute : TypeFilterAttribute
	{

		/// <summary>
		/// 
		/// </summary>
		/// <param name="roles"></param>
		public IsInRoleAttribute(params object[] roles) : base(typeof(IsInRoleFilter))
		{
			Arguments = new object[] { roles };
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class IsInRoleFilter : IAuthorizationFilter
	{
		readonly object[] _roles;
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="roles"></param>
		public IsInRoleFilter(params object[] roles)
		{
			_roles = roles;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		public void OnAuthorization(AuthorizationFilterContext context)
		{
			var hasClaim = context.HttpContext.User.Claims.Any(c => 
				c.Type == ClaimTypes.Role && 
				_roles.Select(i => ((Domain.Enums.Role)i).ToString()).Contains(c.Value)
			);

			if (!hasClaim)
			{
				context.Result = new ForbidResult();
			}
		}
	}
}
