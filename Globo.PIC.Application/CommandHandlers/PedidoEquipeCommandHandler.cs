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
using Globo.PIC.Domain.Types.Queries;
using Globo.PIC.Domain.Exceptions;
namespace Globo.PIC.Application.Services.Commands
{
	/// <summary>
	/// 
	/// </summary>
	public class PedidoEquipeCommandHandler :
		IRequestHandler<DeletePedidoEquipe>,
		IRequestHandler<UpdatePedidoEquipe>,
		IRequestHandler<SavePedidoEquipe>,
        IRequestHandler<SavePedidoEquipes> 
    {

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<Equipe> pedidoEquipeRepository;
		  
		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<Pedido> pedidoRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<Usuario> usuarioRepository;

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
		public PedidoEquipeCommandHandler(
			IUnitOfWork _unitOfWork,
			IMediator _mediator,
			IRepository<Equipe> _pedidoEquipeRepository,
			IRepository<Pedido> _pedidoRepository,
            IRepository<Usuario> _usuarioRepository )
        {
			unitOfWork = _unitOfWork;
			mediator = _mediator;
			pedidoEquipeRepository = _pedidoEquipeRepository;
			pedidoRepository = _pedidoRepository;
            usuarioRepository = _usuarioRepository;
        }
        protected void RunEntityValidation(Equipe equipe)
        {
            if (equipe == null)
                throw new ApplicationException("A equipe está vazia!");

            if (equipe.IdPedido == 0)
                throw new ApplicationException("O ID Pedido está zerado!");

            if (equipe.Login == "")
                throw new ApplicationException("O login é obrigatório!");
        }

        public void RunEntityValidationList(List<Equipe> equipeList)
        {
            if (equipeList.Count() == 0)
                throw new ApplicationException("A equipe está vazia!");

            foreach (var equipe in equipeList)
                RunEntityValidation(equipe);
        }
        async Task<Unit> IRequestHandler<SavePedidoEquipes, Unit>.Handle(SavePedidoEquipes request, CancellationToken cancellationToken)
        {
            try
            {
                var result = false;
                RunEntityValidationList(request.PedidoEquipes);

                var pedido = await pedidoRepository.GetById(request.PedidoEquipes[0].IdPedido, cancellationToken);
                if (pedido == null)
                    throw new NotFoundException("pedido não encontrado!");

                unitOfWork.BeginTransaction();
                foreach (var equipe in request.PedidoEquipes)
                {

                    var usuario = await usuarioRepository.GetByLogin(equipe.Login, cancellationToken);
                    if (usuario == null)
                        throw new NotFoundException("Usuário não encontrado!");
                    equipe.Usuario = usuario;
                    equipe.Pedido = pedido;

                    pedidoEquipeRepository.AddOrUpdate(equipe, cancellationToken);
                    result = unitOfWork.SaveChanges();
                    if (!result) throw new ApplicationException("An error has occured.");
                }

                unitOfWork.CommitTransaction();
            }
            catch (Exception error)
            {
                unitOfWork.RollbackTransaction();
                throw new ApplicationException(error.Message);
            }
            return Unit.Value;
        }

        Task<Unit> IRequestHandler<DeletePedidoEquipe, Unit>.Handle(DeletePedidoEquipe request, CancellationToken cancellationToken)
        {

            var pedidoEquipe = pedidoEquipeRepository.GetAll().Where(x => x.Login == request.Login && x.IdPedido == request.IdPedido);

            if (pedidoEquipe == null)
                throw new NotFoundException("equipe não encontrada!");

            pedidoEquipeRepository.Remove(pedidoEquipe);

            var result = unitOfWork.SaveChanges();

            if (!result) throw new ApplicationException("An error has occured.");

            return Task.FromResult(Unit.Value);

        }
         

        async Task<Unit> IRequestHandler<UpdatePedidoEquipe, Unit>.Handle(UpdatePedidoEquipe request, CancellationToken cancellationToken)
        {
            try
            {
                RunEntityValidation(request.PedidoEquipe);
                var pedidoEquipe = await pedidoEquipeRepository.GetById(request.PedidoEquipe.Id, cancellationToken);
                if (pedidoEquipe == null)
                    throw new NotFoundException("Equipe não encontrada!");

                var pedido = await pedidoRepository.GetById(request.PedidoEquipe.IdPedido, cancellationToken);
                if (pedido==null)
                    throw new NotFoundException("Pedido não encontrado!");
                pedidoEquipe.Pedido = pedido;

                var usuario = await usuarioRepository.GetByLogin(request.PedidoEquipe.Login, cancellationToken);
                if (usuario == null)
                    throw new NotFoundException("Usuário não encontrado!");
                pedidoEquipe.Usuario = usuario;

                pedidoEquipeRepository.AddOrUpdate(request.PedidoEquipe, cancellationToken);
                var result = unitOfWork.SaveChanges();
                if (!result) throw new ApplicationException("An error has occured.");

            }
            catch (Exception error)
            {
                throw new ApplicationException(error.Message);
            }
            return Unit.Value;
        } 
        
        async Task<Unit> IRequestHandler<SavePedidoEquipe, Unit>.Handle(SavePedidoEquipe request, CancellationToken cancellationToken)
        {
            try
            {
                RunEntityValidation(request.PedidoEquipe);
                var pedido = await pedidoRepository.GetById(request.PedidoEquipe.IdPedido, cancellationToken);
                if (pedido == null)
                    throw new NotFoundException("Pedido não encontrado!");
                request.PedidoEquipe.Pedido = pedido;

                var usuario = await usuarioRepository.GetByLogin(request.PedidoEquipe.Login, cancellationToken);
                if (usuario == null)
                    throw new NotFoundException("Usuário não encontrado!");
                request.PedidoEquipe.Usuario = usuario;
                
                pedidoEquipeRepository.AddOrUpdate(request.PedidoEquipe, cancellationToken);
                var result = unitOfWork.SaveChanges();
                if (!result) throw new ApplicationException("An error has occured.");
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
            return Unit.Value;
        }
    }
}