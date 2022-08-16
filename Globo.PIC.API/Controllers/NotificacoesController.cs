using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Types.Queries;
using Globo.PIC.Domain.ViewModels;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Types.Commands;
using System;
using Globo.PIC.Domain.Extensions;
using Microsoft.Extensions.Logging;

namespace Globo.PIC.API.Controllers
{
	/// <summary>
	/// Controladora para metodos de notificações.
	/// As notificações são geradas em background pelo sistema e esta controladora auxilia na recuperação das notificações do usuário,
	/// além disto, também é possível configurar mensagens como lidas.
	/// </summary>
	[ApiController]
	[Authorize(Policy = "MatchProfile")]
	[Route("notificacoes")]
	public class NotificacoesController : Controller
	{
		/// <summary>
		/// 
		/// </summary>
		private readonly ILogger<NotificacoesController> logger;

		/// <summary>
		/// 
		/// </summary>
		private readonly IUserProvider userProvider;

		/// <summary>
		/// 
		/// </summary>
		private readonly IMapper mapper;

		/// <summary>
		/// 
		/// </summary>
		private readonly IMediator mediator;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="_userProvider"></param>
		/// <param name="_mapper"></param>
		/// <param name="_mediator"></param>
		public NotificacoesController(ILogger<NotificacoesController> _logger, 
								IUserProvider _userProvider,
								IMapper _mapper,
								IMediator _mediator)
		{
			logger = _logger;
			userProvider = _userProvider;
			mapper = _mapper;
			mediator = _mediator;
		}

		/// <summary>
		/// Metodo que recupera o número de notificações ainda não visualizadas pelo usuario autenticado.
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns>Número de notificações ainda não visualizadas pelo usuario.</returns>
		[HttpGet("nao-vistas")]
		[ProducesResponseType(200, Type = typeof(int))]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> GetNotViewersAsync(CancellationToken cancellationToken)
		{
			var count = await mediator.Send(new GetByNotificacaoNaoVista(), cancellationToken);

			//if (count == 0) return NotFound();

			return Ok(count);
		}

		/// <summary>
		/// Método que recupera todas as notificações do usuário autenticado.
		/// </summary>
		/// <param name="filter"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<NotificationViewModel>))]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> GetNotificacoesAsync([FromQuery] NotificacaoFilterViewModel filter, CancellationToken cancellationToken)
		{
			var count = await mediator.Send(new GetByNotificacaoFilterCount() { Filter = filter }, cancellationToken);

			if (count == 0) return NotFound();

			var notifications = await mediator.Send(new GetByNotificacaoFilter() { Filter = filter }, cancellationToken);

			List<NotificationViewModel> notificationList = new List<NotificationViewModel>();

			foreach (var item in notifications.OrderByDescending(n => n.CreatedAt))
			{
				var notificationVM = mapper.Map<NotificationViewModel>(item);

				notificationVM.IsRead = item.Readers.Where(r => r.Login.Equals(userProvider.User.Login)).Any();

				notificationList.Add(notificationVM);
			}

			var pagging = new GenericPaggingViewModel<NotificationViewModel>(notificationList, filter, count);

			return Ok(pagging);
		}

		/// <summary>
		/// Metodo que marca uma notificação destinada a um usuário ou a um grupo de usuário como lida pelo usuário autenticado.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpPut("{id:long}/marcar-acessada")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400, Type = typeof(string))]
		[ProducesResponseType(422, Type = typeof(string))]
		[ProducesResponseType(500)]
		public async Task<IActionResult> PutNotificacoes([FromRoute] long id, CancellationToken cancellationToken)
		{
				if (id == 0) return BadRequest("O parametro id é obrigatório.");

				var notification = await mediator.Send<Notificacao>(new GetById(){
					Id = id
				}, cancellationToken);

				if (notification == null) return NotFound("Notificação não encontrada.");

				await mediator.Send(new SetNotificacaoLida()
				{
					Notification = notification,
					IsRead = true
				});

				return Ok();
		}
	}
}
