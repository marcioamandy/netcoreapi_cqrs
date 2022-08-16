using System;
using System.Collections.Generic;
using System.Linq;
using Globo.PIC.API.Configurations.Attributes;
using Globo.PIC.Domain.Enums;
using Globo.PIC.Domain.Extensions;
using Globo.PIC.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Globo.PIC.API.Controllers
{
	/// <summary>
	/// 
	/// </summary>
	[ApiController]
	[Route("tiponegociacoes")]
	[Authorize(Policy = "MatchProfile")]
	public class TipoNegociacoesController : ControllerBase
	{

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filter"></param>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<TipoNegociacaoViewModel>))]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		public IActionResult GetPerfil()
		{
			List<TipoNegociacaoViewModel> userList = new List<TipoNegociacaoViewModel>();

			foreach (var item in Enum.GetValues(typeof(TipoNegociacao)))
				userList.Add(ToViewModel(item));

			return Ok(userList.OrderBy(u => u.Nome));
		}

		private TipoNegociacaoViewModel ToViewModel(object item)
		{
			var enumItem = (TipoNegociacao)item;

			return new TipoNegociacaoViewModel()
			{
				Nome = enumItem.GetEnumDescription(),
				Id = (int)enumItem
			};
		}
	}
}
