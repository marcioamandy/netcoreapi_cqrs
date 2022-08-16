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
	[Route("tipopagamentos")]
	[Authorize(Policy = "MatchProfile")]
	public class TipoPagamentosController : ControllerBase
	{

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<TipoPagamentoViewModel>))]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		public IActionResult GetPerfil()
		{
			List<TipoPagamentoViewModel> userList = new List<TipoPagamentoViewModel>();

			foreach (var item in Enum.GetValues(typeof(TipoPagamento)))
				userList.Add(ToViewModel(item));

			return Ok(userList.OrderBy(u => u.Nome));
		}

		private TipoPagamentoViewModel ToViewModel(object item)
		{
			var enumItem = (TipoPagamento)item;
			return new TipoPagamentoViewModel()
			{
				Nome = enumItem.GetEnumDescription(),
				Id = (int)enumItem
			};
		}
	}
}
