//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Globo.PIC.Domain.Entities;
//using Globo.PIC.Domain.Extensions;
//using Globo.PIC.Domain.Interfaces;
//using Globo.PIC.Domain.Models;
//using Microsoft.EntityFrameworkCore;
//using Globo.PIC.Domain.Enums;
//using MediatR;
//using System.Threading;
//using Globo.PIC.Domain.Types.Queries;
//using Globo.PIC.Domain.Types.Queries.Filters;

//namespace Globo.PIC.Application.QueryHandlers
//{
//	/// <summary>
//	/// 
//	/// </summary>
//	public class UserQueryHandler :
//		IRequestHandler<GetByLogin, User>,
//		IRequestHandler<GetByUserFilter, List<User>>,
//		IRequestHandler<GetByUserFilterCount, int>
//	{

//		/// <summary>
//		/// 
//		/// </summary>
//		private readonly IRepository<User> userRepository;

//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="_userRepository"></param>
//		public UserQueryHandler(
//			IRepository<User> _userRepository
//			)
//		{
//			userRepository = _userRepository;
//		}

//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="request"></param>
//		/// <param name="cancellationToken"></param>
//		/// <returns></returns>
//		Task<User> IRequestHandler<GetByLogin, User>.Handle(GetByLogin request, CancellationToken cancellationToken)
//		{
//			return userRepository.GetByLogin(request.Login, cancellationToken);
//		}

//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="_request"></param>
//		/// <param name="cancellationToken"></param>
//		/// <returns></returns>
//		Task<List<User>> IRequestHandler<GetByUserFilter, List<User>>.Handle(GetByUserFilter _request, CancellationToken cancellationToken)
//		{
//			var request = GetQueryFilterUser(_request);

//			request = request.OrderBy(s => s.Name);

//			int.TryParse(_request.Filter.Page.ToString(), out int page);
//			page = page <= 0 ? 1 : page;

//			request = request.Skip((page - 1) * _request.Filter.PerPage).Take(_request.Filter.PerPage);

//			return request.ToListAsync(cancellationToken);
//		}

//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="_request"></param>
//		/// <param name="cancellationToken"></param>
//		/// <returns></returns>
//		async Task<int> IRequestHandler<GetByUserFilterCount, int>.Handle(GetByUserFilterCount _request, CancellationToken cancellationToken)
//		{
//			var queryUserFilter = userRepository.GetAll();
//			var roles = _request.Filter.Roles.ToList();

//			if (!string.IsNullOrEmpty(_request.Filter.Search))
//			{
//				queryUserFilter = queryUserFilter.Where(x =>
//					x.Name.ToLower().Contains(_request.Filter.Search.ToLower()) ||
//					x.LastName.ToLower().Contains(_request.Filter.Search.ToLower()) ||
//					x.Login.ToLower().Contains(_request.Filter.Search.ToLower()) 
//				);
//			}

//			//if (!string.IsNullOrWhiteSpace(_request.Filter.Search))
//			//{
//			//	queryUserFilter = queryUserFilter.Where(u =>
//			//		(u.Name ?? string.Empty).RemoveDiacritics().Contains(_request.Filter.Search.RemoveDiacritics(), StringComparison.InvariantCultureIgnoreCase) ||
//			//		(u.LastName ?? string.Empty).RemoveDiacritics().Contains(_request.Filter.Search.RemoveDiacritics(), StringComparison.InvariantCultureIgnoreCase) ||
//			//		u.Login.RemoveDiacritics().Contains(_request.Filter.Search.RemoveDiacritics(), StringComparison.InvariantCultureIgnoreCase)
//			//	);
//			//}

//			if (_request.Filter.IsActive.HasValue)
//				queryUserFilter = queryUserFilter.Where(u => u.IsActive.Equals(_request.Filter.IsActive.Value));

//			return await queryUserFilter.CountAsync(cancellationToken);


//		}

//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="query"></param>
//		/// <returns></returns>
//		private IQueryable<User> GetQueryFilterUser(GetByUserFilter request)
//		{
//			var queryUserFilter = userRepository.GetAll();
//			var roles = request.Filter.Roles.ToList();

//			//if (!string.IsNullOrWhiteSpace(request.Filter.Search))
//			//{				
//			//	queryUserFilter = queryUserFilter.Where(u =>
//			//		(u.Name ?? string.Empty).RemoveDiacritics().Contains(request.Filter.Search.RemoveDiacritics(), StringComparison.InvariantCultureIgnoreCase) ||
//			//		(u.LastName ?? string.Empty).RemoveDiacritics().Contains(request.Filter.Search.RemoveDiacritics(), StringComparison.InvariantCultureIgnoreCase) ||
//			//		u.Login.RemoveDiacritics().Contains(request.Filter.Search.RemoveDiacritics(), StringComparison.InvariantCultureIgnoreCase)
//			//	);
//			//}
//			if (!string.IsNullOrEmpty(request.Filter.Search))
//			{
//				queryUserFilter = queryUserFilter.Where(x =>
//					x.Name.ToLower().Contains(request.Filter.Search.ToLower()) ||
//					x.LastName.ToLower().Contains(request.Filter.Search.ToLower()) ||
//					x.Login.ToLower().Contains(request.Filter.Search.ToLower())
//				);
//			}

//			//if (request.Filter.IsAdm.HasValue)
//			//	if (request.Filter.IsAdm.Value)
//			//	{
//			//		if (!roles.Any(r => r.Equals(Role.GRANT_ADM_USUARIOS.ToString())))
//			//			roles.Add(Role.GRANT_ADM_USUARIOS.ToString());

//			//		queryUserFilter = queryUserFilter.Where(u => u.Roles.Any(r => r.Name.Equals(Role.GRANT_ADM_USUARIOS.ToString())));
//			//	}
//			//	else
//			//	{
//			//		roles.Remove(Role.GRANT_ADM_USUARIOS.ToString());
//			//		queryUserFilter = queryUserFilter.Where(u => !u.Roles.Any(r => r.Name.Equals(Role.GRANT_ADM_USUARIOS.ToString())));
//			//	}

//			if (request.Filter.IsActive.HasValue)
//				queryUserFilter = queryUserFilter.Where(u => u.IsActive.Equals(request.Filter.IsActive.Value));

//			//if (roles.Count() > 0)
//			//	queryUserFilter = queryUserFilter.Where(u => u.Roles.Where(r => roles.Contains(r.Name)).Any());

//			return queryUserFilter;
//		}

//    }
//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;
using Globo.PIC.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
using System.Threading;
using Globo.PIC.Domain.Enums;
using Globo.PIC.Domain.Types.Queries;
using Globo.PIC.Domain.Types.Queries.Filters;

namespace Globo.PIC.Application.QueryHandlers
{
	/// <summary>
	/// 
	/// </summary>
	public class UserQueryHandler :
		IRequestHandler<GetByLogin, User>,
		IRequestHandler<GetByUserFilter, List<User>>,
		IRequestHandler<GetByUserFilterCount, int>
	{

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<User> userRepository;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="_userRepository"></param>
		public UserQueryHandler(
			IRepository<User> _userRepository
			)
		{
			userRepository = _userRepository;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<User> IRequestHandler<GetByLogin, User>.Handle(GetByLogin request, CancellationToken cancellationToken)
		{
			return userRepository.GetByLogin(request.Login, request.CancellationToken);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="_request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<List<User>> IRequestHandler<GetByUserFilter, List<User>>.Handle(GetByUserFilter _request, CancellationToken cancellationToken)
		{
			var request = GetQueryFilterUser(_request);

			request = request.OrderBy(s => s.Name);

			int.TryParse(_request.Filter.Page.ToString(), out int page);
			page = page <= 0 ? 1 : page;

			request = request.Skip((page - 1) * _request.Filter.PerPage).Take(_request.Filter.PerPage);

			return request.ToListAsync(cancellationToken);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		private IQueryable<User> GetQueryFilterUser(GetByUserFilter request)
		{
			var queryUserFilter = userRepository.GetAll();
			var roles = request.Filter.Roles.ToList();

			if (!string.IsNullOrWhiteSpace(request.Filter.Search))
			{
				var loginStripped = request.Filter.Search.ToLower().Trim().RemoveDiacritics();

				queryUserFilter = queryUserFilter.Where(u =>
					(u.Name ?? string.Empty).ToLower().Contains(request.Filter.Search.ToLower().Trim()) ||
					(u.LastName ?? string.Empty).ToLower().Contains(request.Filter.Search.ToLower().Trim()) ||
					((u.Name ?? string.Empty).Trim() + " " + (u.LastName ?? string.Empty).Trim()).ToLower().Trim().Contains(request.Filter.Search.ToLower().Trim()) ||
					u.Login.ToLower().Contains(loginStripped)
				);
			}

			if (request.Filter.IsAdm.HasValue)
				if (request.Filter.IsAdm.Value)
				{
					if (!roles.Any(r => r.Equals(Role.GRANT_ADM_USUARIOS.ToString())))
						roles.Add(Role.GRANT_ADM_USUARIOS.ToString());

					queryUserFilter = queryUserFilter.Where(u => u.Roles.Any(r => r.Name.Equals(Role.GRANT_ADM_USUARIOS.ToString())));
				}
				else
				{
					roles.Remove(Role.GRANT_ADM_USUARIOS.ToString());
					queryUserFilter = queryUserFilter.Where(u => !u.Roles.Any(r => r.Name.Equals(Role.GRANT_ADM_USUARIOS.ToString())));
				}

			if (request.Filter.IsActive.HasValue)
				queryUserFilter = queryUserFilter.Where(u => u.IsActive.Equals(request.Filter.IsActive.Value));

			if (roles.Count() > 0)
				queryUserFilter = queryUserFilter.Where(u => u.Roles.Where(r => roles.Contains(r.Name)).Any());

			return queryUserFilter;
		}

		Task<int> IRequestHandler<GetByUserFilterCount, int>.Handle(GetByUserFilterCount request, CancellationToken cancellationToken)
		{
			var query = GetQueryFilterUser(request);

			return query.CountAsync(cancellationToken);
		}
	}
}