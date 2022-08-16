using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Models;
using Microsoft.EntityFrameworkCore;
using MediatR;
using System.Threading;
using Globo.PIC.Domain.Types.Queries;
using Globo.PIC.Domain.Types.Events;
using Globo.PIC.Domain.Types.Queries.Filters;

namespace Globo.PIC.Application.QueryHandlers
{
    public class NotificacaoQueryHandler :
		IRequestHandler<GetById, Notificacao>,
		IRequestHandler<GetByNotificacaoFilterCount, int>,
		IRequestHandler<GetByNotificacaoFilter, List<Notificacao>>,
		IRequestHandler<GetByNotificacaoNaoVista, int>
	{
		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<Notificacao> notificationRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IUserProvider userProvider;

		/// <summary>
		/// 
		/// </summary>
		private readonly IMediator mediator;

		public NotificacaoQueryHandler(
			IRepository<Notificacao> _notificationRepository,
			IUserProvider _userProvider,
			IMediator _mediator
			)
		{
			notificationRepository = _notificationRepository;
			userProvider = _userProvider;
			mediator = _mediator;
		}

		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		Task<Notificacao> IRequestHandler<GetById, Notificacao>.Handle(GetById request, CancellationToken cancellationToken)
		{
			var query = notificationRepository.GetById(request.Id, cancellationToken);
			
			return query.AsTask();
		}

		/// <summary>
		/// Contagem de registros de uma busca.
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		Task<int> IRequestHandler<GetByNotificacaoFilterCount, int>.Handle(GetByNotificacaoFilterCount request, CancellationToken cancellationToken)
		{
			var queryNotificationFilter = notificationRepository.GetAll().Where(p =>
				p.Assigns.Where(a =>
					userProvider.User.Authorization.Roles.Contains(a.Role) ||
					userProvider.User.Login.Equals(a.Login)
				).Any()
			);

			if (!string.IsNullOrEmpty(request.Filter.Search))
			{
				queryNotificationFilter = queryNotificationFilter
					.Where(p =>
						p.Title.RemoveDiacritics().Contains(request.Filter.Search.RemoveDiacritics(), StringComparison.InvariantCultureIgnoreCase)
					);
			}

			queryNotificationFilter = queryNotificationFilter.OrderByDescending(n => n.CreatedAt);

			return notificationRepository.CountAsync(queryNotificationFilter, cancellationToken);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		Task<List<Notificacao>> IRequestHandler<GetByNotificacaoFilter, List<Notificacao>>.Handle(GetByNotificacaoFilter request, CancellationToken cancellationToken)
		{
			var queryNotificationFilter = notificationRepository.GetAll().Where(p =>
				p.Assigns.Where(a => userProvider.User.Authorization.Roles.Contains(a.Role) || userProvider.User.Login.Equals(a.Login)).Any()
			);

			if (!string.IsNullOrEmpty(request.Filter.Search))
			{
				queryNotificationFilter = queryNotificationFilter
				.Where(m =>
					m.Title.RemoveDiacritics().Contains(request.Filter.Search.RemoveDiacritics(), StringComparison.InvariantCultureIgnoreCase)
				);
			}

			queryNotificationFilter = queryNotificationFilter.OrderByDescending(n => n.CreatedAt);

			int.TryParse(request.Filter.Page.ToString(), out int page);
			page = page <= 0 ? 1 : page;

			queryNotificationFilter = queryNotificationFilter.Skip((page - 1) * request.Filter.PerPage).Take(request.Filter.PerPage);

			mediator.Publish(new OnNotificacaoVista()
			{
				NotificacaoIds = queryNotificationFilter.Select(n => n.Id).ToArray()
			});

			return queryNotificationFilter.ToListAsync();
		}

		/// <summary>
		/// Contagem de notificações não visualizados pelo usuário
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		Task<int> IRequestHandler<GetByNotificacaoNaoVista, int>.Handle(GetByNotificacaoNaoVista request, CancellationToken cancellationToken)
		{
			var queryFilter = notificationRepository.GetAll().Where(p =>
				(p.Assigns.Where(a =>
					userProvider.User.Authorization.Roles.Contains(a.Role) ||
					userProvider.User.Login.Equals(a.Login)).Any()
				) &&
				!p.Viewers.Where(a => userProvider.User.Login.Equals(a.Login)).Any()
			);

			return notificationRepository.CountAsync(queryFilter, cancellationToken);
		}
	}
}
