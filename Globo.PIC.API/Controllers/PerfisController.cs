using System;
using System.Collections.Generic;
using System.Linq;
using Globo.PIC.API.Configurations.Attributes;
using Globo.PIC.Domain.Enums;
using Globo.PIC.Domain.Extensions;
using Globo.PIC.Domain.ViewModels;
using Globo.PIC.Domain.ViewModels.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Globo.PIC.API.Controllers
{
	/// <summary>
	/// 
	/// </summary>
	[ApiController]
	[Route("perfis")]
	[Authorize(Policy = "MatchProfile")]
	public class PerfisController : ControllerBase
    {
		/// <summary>
		/// 
		/// </summary>
		public PerfisController() { }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filter"></param>
		/// <returns></returns>
		[HttpGet] 
		[ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<RoleViewModel>))]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		[IsInRole(Role.GRANT_ADM_USUARIOS)]
		public IActionResult GetPerfil([FromQuery] RoleFilterViewModel filter)
		{
			List<RoleViewModel> userList = new List<RoleViewModel>();

			foreach (var item in Enum.GetValues(typeof(Role)))
				userList.Add(ToViewModel(item));

			if (!string.IsNullOrWhiteSpace(filter.Type))
				userList = userList.Where(f => f.Type.Equals(filter.Type)).ToList();
			 
			var pagging = new GenericPaggingViewModel<RoleViewModel>(userList.OrderBy(u => u.Name), filter, userList.Count);

			return Ok(pagging);

		}

		private RoleViewModel ToViewModel(object item)
		{
			var enumItem = (Role)item;
			return new RoleViewModel()
			{
				Description = enumItem.GetEnumDescription(),
				Name = enumItem.ToString(),
				Type = enumItem.ToString().StartsWith("PERFIL") ? RoleType.Profile.ToString() : RoleType.Grant.ToString()
			};
		}
	}
}
