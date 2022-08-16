using System;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Interfaces;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.Exceptions;
using Globo.PIC.Domain.Types.Events;
using Globo.PIC.Infra.Data.Repositories;

namespace Globo.PIC.Application.Services.Commands
{
	/// <summary>
	/// 
	/// </summary>
	public class UserCommandHandler :
		IRequestHandler<UpdateUserRoles>,
		IRequestHandler<SaveUserRole>,
		IRequestHandler<DeleteUserRole>,
		IRequestHandler<SaveUser>,
		IRequestHandler<UpdateUser>
	{
		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<UserRole> userRoleRepository;
		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<User> userRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<UserRole> rolesUserRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IMediator mediator;

		/// <summary>
		/// 
		/// </summary>
		private readonly IUnitOfWork unitOfWork;

		/// <summary>
		/// 
		/// </summary>  
		public UserCommandHandler(
			IUnitOfWork _unitOfWork,
			IMediator _mediator,
			IRepository<UserRole> _userRoleRepository,
			IRepository<User> _userRepository,
			IRepository<UserRole> _rolesUserRepository)
		{
			unitOfWork = _unitOfWork;
			mediator = _mediator;
			userRoleRepository = _userRoleRepository;
			userRepository = _userRepository;
			rolesUserRepository = _rolesUserRepository;
		}

      

        protected void RunEntityValidation(UserRole userRole)
        {
			if (userRole == null)
				throw new ApplicationException("A Role está Vazia!");

			if(userRole.Login=="")
				throw new ApplicationException("O Login está vazio!");

			if (userRole.Name == "")
				throw new ApplicationException("O nome da role está vazio!");
		}
	 
		 
       async Task<Unit> IRequestHandler<DeleteUserRole, Unit>.Handle(DeleteUserRole request, CancellationToken cancellationToken)
        {

			var userRole = await userRoleRepository.GetByRoleName(request.Login, request.Name, cancellationToken);
			if (userRole == null)
				throw new NotFoundException("Role não encontrada!");

			userRoleRepository.Remove(userRole);
			var result = unitOfWork.SaveChanges();

			if (!result) throw new ApplicationException("An error has occured.");

			return Unit.Value;
		}

        async Task<Unit> IRequestHandler<SaveUserRole, Unit>.Handle(SaveUserRole request, CancellationToken cancellationToken)
        {
			try
			{
				RunEntityValidation(request.UserRole);

				var user = await userRepository.GetByLogin(request.UserRole.Login, cancellationToken);
				if (user == null)
					throw new NotFoundException("usuário não encontrado!");
				else
					request.UserRole.User = user;


				userRoleRepository.AddOrUpdate(request.UserRole, cancellationToken);
				var result = unitOfWork.SaveChanges();
				if (!result) throw new ApplicationException("An error has occured.");
			}
			catch(Exception error)
            {
				throw new Exception(error.Message);
            }
			return Unit.Value;
		}

        async Task<Unit> IRequestHandler<UpdateUserRoles, Unit>.Handle(UpdateUserRoles request, CancellationToken cancellationToken)
        {
			try
			{
				unitOfWork.BeginTransaction();

				var user = await userRepository.GetByLogin(request.Login, cancellationToken);
					if (user == null)
						throw new NotFoundException("usuário não encontrado!");

				foreach ( var role  in request.UserRoles)
                {
					RunEntityValidation(role);

						role.User = user;

					userRoleRepository.AddOrUpdate(role, cancellationToken);
					var result = unitOfWork.SaveChanges();
					if (!result) throw new ApplicationException("An error has occured.");
				}
				unitOfWork.CommitTransaction();
			}
			catch (Exception error)
			{
				unitOfWork.RollbackTransaction();
				throw new Exception(error.Message);
			}
			return Unit.Value;
		} 
		Task<Unit> IRequestHandler<UpdateUser, Unit>.Handle(UpdateUser request, CancellationToken cancellationToken)
		{
		 
			var rolesUsers = rolesUserRepository.GetAll().Where(s => s.Login == request.User.Login).ToList();
			rolesUserRepository.Remove(rolesUsers);

			request.User.Roles =
			request.User.Roles.Select(r =>
				new UserRole()
				{
					Name = r.Name,
					Login = request.User.Login
				}
			).ToList();

			userRepository.AddOrUpdate(request.User, cancellationToken);

			var result = unitOfWork.Commit();

			if (!result) throw new ApplicationException("An error has occured.");

			if (request.Update)
				mediator.Publish(new OnUsuarioAlterado() { Usuario = request.User });
			else
				mediator.Publish(new OnUsuarioCriado() { Usuario = request.User });

			return Task.FromResult(Unit.Value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<Unit> IRequestHandler<SaveUser, Unit>.Handle(SaveUser request, CancellationToken cancellationToken)
		{
			var rolesUsers = userRoleRepository.GetAll().Where(s => s.Login == request.User.Login).ToList();
			userRoleRepository.Remove(rolesUsers);

			request.User.Roles =
			request.User.Roles.Select(r =>
				new UserRole()
				{
					User = request.User,
					Name = r.Name,
					Login = request.User.Login
				}
			).ToList();

			userRepository.AddOrUpdate(request.User, cancellationToken);

			var result = unitOfWork.Commit();

			if (!result) throw new ApplicationException("An error has occured.");

			return Task.FromResult(Unit.Value);
		}

	}
}