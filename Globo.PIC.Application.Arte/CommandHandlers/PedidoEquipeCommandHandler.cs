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
using Globo.PIC.Domain.Types.Events;
using Globo.PIC.Domain.Types.Queries;
using Newtonsoft.Json;
using Globo.PIC.Domain.Types.Queries.Filters;
using Globo.PIC.Domain.Exceptions;
using Globo.PIC.Domain.ViewModels;
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
        private readonly IRepository<User> userRepository;

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
            IRepository<User> _userRepository )
        {
			unitOfWork = _unitOfWork;
			mediator = _mediator;
			pedidoEquipeRepository = _pedidoEquipeRepository;
			pedidoRepository = _pedidoRepository;
            userRepository = _userRepository;
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

                    var user = await userRepository.GetByLogin(equipe.Login, cancellationToken);
                    if (user == null)
                        throw new NotFoundException("usuário não encontrado!");
                    equipe.User = user;
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
                    throw new NotFoundException("equipe não encontrada!");

                var pedido = await pedidoRepository.GetById(request.PedidoEquipe.IdPedido, cancellationToken);
                if (pedido==null)
                    throw new NotFoundException("pedido não encontrado!");
                pedidoEquipe.Pedido = pedido;

                var user = await userRepository.GetByLogin(request.PedidoEquipe.Login, cancellationToken);
                if (user == null)
                    throw new NotFoundException("usuário não encontrado!");
                pedidoEquipe.User = user;

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
                    throw new NotFoundException("pedido não encontrado!");
                request.PedidoEquipe.Pedido = pedido;

                var user = await userRepository.GetByLogin(request.PedidoEquipe.Login, cancellationToken);
                if (user == null)
                    throw new NotFoundException("usuário não encontrado!");
                request.PedidoEquipe.User = user;
                
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
        
            //      async Task<Unit> IRequestHandler<UpdatePedidoItens, Unit>.Handle(UpdatePedidoItens request, CancellationToken cancellationToken)
            //      {
            //	var result = true;
            //	try
            //	{
            //		RunEntityValidationList(request.PedidoItens);

            //		foreach (var currentPedido in request.PedidoItens)
            //		{
            //			var pedidoItem = await pedidoItemRepository.GetById(currentPedido.Id, cancellationToken);

            //			if (pedidoItem == null)
            //				throw new NotFoundException("idpedido não encontrado!");

            //			if (pedidoItem.IdPedido > 0)
            //				currentPedido.Pedido
            //					= await pedidoRepository.GetById(pedidoItem.IdPedido, cancellationToken);

            //			if (pedidoItem.IdStatus > 0)
            //				currentPedido.StatusPedidoItens
            //					= await statusItemRepository.GetById(pedidoItem.IdStatus, cancellationToken);




            //			pedidoItemRepository.AddOrUpdate(currentPedido, cancellationToken);
            //			result =unitOfWork.SaveChanges();
            //		}

            //		if (!result) throw new ApplicationException("An error has occured.");
            //	}
            //	catch (Exception error)
            //	{
            //		throw new ApplicationException(error.Message);
            //	}
            //	return Unit.Value;
            //}

            //     async Task<Unit> IRequestHandler<DeletePedidoItem, Unit>.Handle(DeletePedidoItem request, CancellationToken cancellationToken)
            //      {
            //	var pedidoItem = await pedidoItemRepository.GetById(request.PedidoItem.Id, cancellationToken);

            //	if (pedidoItem == null)
            //		throw new NotFoundException("idpedido não encontrado!");

            //	pedidoItemRepository.Remove(pedidoItem);
            //	var result = unitOfWork.SaveChanges();

            //	if (!result) throw new ApplicationException("An error has occured.");

            //	return Unit.Value;
            //}

            //      async Task<Unit> IRequestHandler<SavePedidoItem, Unit>.Handle(SavePedidoItem request, CancellationToken cancellationToken)
            //      {
            //	try
            //	{
            //		RunEntityValidation(request.PedidoItem);
            //		var pedido = await pedidoRepository.GetById(request.PedidoItem.IdPedido, cancellationToken);
            //		if (pedido == null)
            //			throw new NotFoundException("idpedido não encontrado!");

            //		if (request.PedidoItem.IdPedido > 0)
            //			request.PedidoItem.Pedido
            //				= await pedidoRepository.GetById(request.PedidoItem.IdPedido, cancellationToken);

            //		if (request.PedidoItem.IdStatus > 0)
            //			request.PedidoItem.StatusPedidoItens
            //				= await statusItemRepository.GetById(request.PedidoItem.IdStatus, cancellationToken);

            //		pedidoItemRepository.AddOrUpdate(request.PedidoItem, cancellationToken);
            //		var result = unitOfWork.SaveChanges();
            //		if (!result) throw new ApplicationException("An error has occured.");
            //	}
            //	catch(Exception error)
            //          {
            //		throw new Exception(error.Message);
            //          }
            //	return Unit.Value;
            //}

            //async Task<Unit> IRequestHandler<SavePedidoItens, Unit>.Handle(SavePedidoItens request, CancellationToken cancellationToken)
            //{
            //	try
            //	{
            //		RunEntityValidationList(request.PedidoItens);
            //		foreach (var pedidoItens in request.PedidoItens)
            //		{
            //			var pedidoItem = await pedidoItemRepository.GetById(pedidoItens.Id, cancellationToken);

            //			if (pedidoItem == null)
            //				throw new NotFoundException("pedido item não encontrado!");

            //			if (pedidoItem.IdPedido > 0)
            //				pedidoItem.Pedido
            //					= await pedidoRepository.GetById(pedidoItem.IdPedido, cancellationToken);

            //			if (pedidoItem.IdStatus > 0)
            //				pedidoItem.StatusPedidoItens
            //					= await statusItemRepository.GetById(pedidoItem.IdStatus, cancellationToken);

            //			pedidoItemRepository.AddOrUpdate(pedidoItens, cancellationToken); 
            //		}
            //		var result = unitOfWork.SaveChanges();
            //		if (!result) throw new ApplicationException("An error has occured.");
            //	}
            //	catch(Exception error)
            //          {
            //		throw new ApplicationException(error.Message);
            //	}
            //	return Unit.Value;
            //}
        }
}