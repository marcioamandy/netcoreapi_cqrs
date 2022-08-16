using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Globo.PIC.Domain.Enums;
using MediatR;
using System.Threading;
using Globo.PIC.Domain.Types.Queries;
using Globo.PIC.Domain.Types.Queries.Filters;

namespace Globo.PIC.Application.QueryHandlers
{
	/// <summary>
	/// 
	/// </summary>
	public class UserRoleQueryHandler :
		IRequestHandler<GetRolesByLogin, List<UserRole>> 
	{

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<UserRole> userRoleRepository;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="_userRepository"></param>
		public UserRoleQueryHandler(
			IRepository<UserRole> _userRoleRepository
			)
		{
			userRoleRepository = _userRoleRepository;
		}


        /// <summary>
        /// lista roles de usuários
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<UserRole>> IRequestHandler<GetRolesByLogin, List<UserRole>>
			.Handle(GetRolesByLogin request, CancellationToken cancellationToken)
		{
			var query = userRoleRepository
				.ListByLogin(request.Login, cancellationToken);

			return query.AsTask();
		}
    }
}