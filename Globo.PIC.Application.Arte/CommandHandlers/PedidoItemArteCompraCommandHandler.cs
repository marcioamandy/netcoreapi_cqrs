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
using Globo.PIC.Domain.Exceptions;
using Globo.PIC.Domain.Enums;
using Globo.PIC.Infra.Data.Repositories;

namespace Globo.PIC.Application.Services.Commands
{
    /// <summary>
    /// PedidoItemArteCompraCommandHandler
    /// </summary>
    public class PedidoItemArteCompraCommandHandler :

		IRequestHandler<DeletePedidoItemArteCompra>,
        IRequestHandler<UpdatePedidoItemArteCompra>,
        IRequestHandler<UpdatePedidoItemArteCompras>,
        IRequestHandler<CreatePedidoItemArteCompra>,
        IRequestHandler<CreatePedidoItemArteCompras>
    {

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemArteCompra> pedidoItemCompraRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemArteEntrega> pedidoItemEntregaRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly PedidoItemArteRepository pedidoItemArteRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<Usuario> userRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IUserProvider userProvider;

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
		public PedidoItemArteCompraCommandHandler(
			IUnitOfWork _unitOfWork,
			IMediator _mediator,
			IRepository<PedidoItemArteCompra> _pedidoItemCompraRepository,
			IRepository<PedidoItemArteEntrega> _pedidoItemEntregaRepository,
			IRepository<PedidoItemArte> _pedidoItemArteRepository,
			IRepository<Usuario> _userRepository,
			IUserProvider _userProvider
			)
		{
			unitOfWork = _unitOfWork;
			mediator = _mediator;
			pedidoItemCompraRepository = _pedidoItemCompraRepository;
			pedidoItemEntregaRepository = _pedidoItemEntregaRepository;
			pedidoItemArteRepository = _pedidoItemArteRepository as PedidoItemArteRepository;
			userRepository = _userRepository;
			userProvider = _userProvider;
		}

		protected void RunEntityValidation(PedidoItemArteCompra pedidoItem)
		{
			if (pedidoItem == null)
				throw new ApplicationException("O Pedido Item Compra está vazio!");			

			if (pedidoItem.Login == "")
				throw new ApplicationException("O Login é obrigatório!");

		}
		public void RunEntityValidationList(List<PedidoItemArteCompra> pedidoItemCompraViewModels)
		{
			if (pedidoItemCompraViewModels.Count() == 0)
				throw new ApplicationException("O Pedido Item Compra está vazio!");

			foreach (var pedidoItem in pedidoItemCompraViewModels)
				RunEntityValidation(pedidoItem);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task<Unit> IRequestHandler<UpdatePedidoItemArteCompra, Unit>.Handle(UpdatePedidoItemArteCompra request, CancellationToken cancellationToken)
        {
            try
            {
                RunEntityValidation(request.PedidoItemArteCompra);

                var pedidoItemArte = await pedidoItemArteRepository.GetById(request.PedidoItemArteCompra.IdPedidoItemArte, cancellationToken);

                if (pedidoItemArte == null)
                    throw new NotFoundException("id pedido item arte não encontrado!");
                else
                    request.PedidoItemArteCompra.PedidoItemArte = pedidoItemArte;

                var pedidoItemArteEntrega = pedidoItemEntregaRepository.GetAll()
					.Where(a => a.IdPedidoItemArte == request.PedidoItemArteCompra.IdPedidoItemArte).ToList();

                if (pedidoItemArteEntrega.Count > 0)
                    throw new NotFoundException(string.Format("Compra {0} não pode ser editada, já existe entrega vinculada.", 
                        request.PedidoItemArteCompra.Id));

                if (string.IsNullOrWhiteSpace(request.PedidoItemArteCompra.Login))
                    request.PedidoItemArteCompra.Login = userProvider.User.Login;

                if (request.PedidoItemArteCompra.DataCompra == null)
                    request.PedidoItemArteCompra.DataCompra = DateTime.Now;

                var usuario = await userRepository.GetByLogin(request.PedidoItemArteCompra.Login, cancellationToken);
                if (usuario == null)
                    throw new NotFoundException("login não encontrado!");
                else
                {
                    if (!string.IsNullOrWhiteSpace(request.PedidoItemArteCompra.PedidoItemArte.CompradoPorLogin))
                        request.PedidoItemArteCompra.Usuario = usuario;
                    else
                        throw new NotFoundException("login atribuido não tem perfil de comprador externo ou ainda não possui comprador atribuído.");
                }

                if (pedidoItemArte.IdStatus != (int)PedidoItemArteStatus.ITEM_ATRIBUIDOAOCOMPRADOR)
                    throw new NotFoundException("Item pedido ainda não foi atribuído a um comprador.");

                var compraLancada = await pedidoItemCompraRepository.GetByIdPedidoItemCompra(request.PedidoItemArteCompra.Id, cancellationToken);
                if (compraLancada == null)
                    throw new NotFoundException("Compra não encontrada!");

                if (((pedidoItemArte.QuantidadePendenteCompra + compraLancada.Quantidade) - request.PedidoItemArteCompra.Quantidade) < 0)
                    throw new NotFoundException("Quantidade informada é superior a quantidade pendente de compra.");

                var pedidoItemCompra = await pedidoItemCompraRepository.GetById(request.PedidoItemArteCompra.Id, cancellationToken);
                if (pedidoItemCompra == null)
                    throw new NotFoundException("id pedido item Compra não encontrado!");

                pedidoItemCompraRepository.AddOrUpdate(request.PedidoItemArteCompra, cancellationToken);
                var result = unitOfWork.SaveChanges();
                if (!result) throw new ApplicationException("An error has occured.");

                await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = pedidoItemArte.PedidoItem });

            }
            catch (Exception error)
            {
                throw new ApplicationException(error.Message);
            }

            return Unit.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task<Unit> IRequestHandler<UpdatePedidoItemArteCompras, Unit>.Handle(UpdatePedidoItemArteCompras request, CancellationToken cancellationToken)
        {
            var result = true;
            try
            {
                RunEntityValidationList(request.PedidoItemArteCompras);

                unitOfWork.BeginTransaction();

                foreach (var pedidoItemArteCompra in request.PedidoItemArteCompras)
                {
                    var pedidoItemEntrega = pedidoItemEntregaRepository.GetAll().Where(a => a.IdPedidoItemArte == pedidoItemArteCompra.IdPedidoItemArte).ToList();
                    if (pedidoItemEntrega != null)
                        throw new NotFoundException(string.Format("Compra {0} não pode ser editada, já existe entrega vinculada.", pedidoItemArteCompra.Id));
					  
                    var pedidoItemArte = await pedidoItemArteRepository.GetById(pedidoItemArteCompra.IdPedidoItemArte, cancellationToken);
                    
                    if (pedidoItemArte == null)
                        throw new NotFoundException("id pedido item não encontrado!");
                    else
                        pedidoItemArteCompra.PedidoItemArte = pedidoItemArte;

                    if (string.IsNullOrWhiteSpace(pedidoItemArteCompra.Login))
                        pedidoItemArteCompra.Login = userProvider.User.Login;

                    if (pedidoItemArteCompra.DataCompra == null)
                        pedidoItemArteCompra.DataCompra = DateTime.Now;

                    var usuario = await userRepository.GetByLogin(pedidoItemArteCompra.Login, cancellationToken);
                    if (usuario == null)
                        throw new NotFoundException("login não encontrado!");
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(pedidoItemArteCompra.PedidoItemArte.CompradoPorLogin))
                            pedidoItemArteCompra.PedidoItemArte.Comprador = usuario;
                        else
                            throw new NotFoundException("login atribuido não tem perfil de comprador externo ou ainda não possui comprador atribuído.");
                    }

                    if (pedidoItemArte.IdStatus != (int)PedidoItemArteStatus.ITEM_ATRIBUIDOAOCOMPRADOR)
                        throw new NotFoundException("Item pedido ainda não foi atribuído a um comprador.");

                    var compraLancada = await pedidoItemCompraRepository.GetByIdPedidoItemCompra(pedidoItemArteCompra.Id, cancellationToken);
                    if (compraLancada == null)
                        throw new NotFoundException("Compra não encontrada!");

                    if (((pedidoItemArte.QuantidadePendenteCompra + compraLancada.Quantidade) - pedidoItemArteCompra.Quantidade) < 0)
                        throw new NotFoundException("Quantidade informada é superior a quantidade pendente de compra.");

                    var pedidoItemCompra = await pedidoItemCompraRepository.GetById(pedidoItemArteCompra.Id, cancellationToken);
                    if (pedidoItemCompra == null)
                        throw new NotFoundException("id pedido item Compra não encontrado!");

                    pedidoItemCompraRepository.AddOrUpdate(pedidoItemArteCompra, cancellationToken);
                    result = unitOfWork.SaveChanges();
                    if (!result) throw new ApplicationException("An error has occured.");

                    await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = pedidoItemArte.PedidoItem });
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task<Unit> IRequestHandler<DeletePedidoItemArteCompra, Unit>.Handle(DeletePedidoItemArteCompra request, CancellationToken cancellationToken)
        {
            var pedidoItemCompra = await pedidoItemCompraRepository.GetById(request.Id, cancellationToken);

            if (pedidoItemCompra == null)
                throw new NotFoundException("id pedido item arte compra não encontrado!");

            var pedidoItemArte = await pedidoItemArteRepository.GetById(request.IdPedidoItemArte, cancellationToken);

            var existEntrega = await pedidoItemEntregaRepository.ListByIdPedidoItemEntrega(pedidoItemCompra.IdPedidoItemArte, cancellationToken);
            if (existEntrega.Count > 0)
                throw new NotFoundException(string.Format("Existe entregas para a compra {0}, não é permitido excluir a compra.", pedidoItemCompra.Id));

            pedidoItemCompraRepository.Remove(pedidoItemCompra);
            var result = unitOfWork.SaveChanges();

            if (!result) throw new ApplicationException("An error has occured.");

            await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = pedidoItemArte.PedidoItem });

            return Unit.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task<Unit> IRequestHandler<CreatePedidoItemArteCompra, Unit>.Handle(CreatePedidoItemArteCompra request, CancellationToken cancellationToken)
        {
            try
            {
                RunEntityValidation(request.PedidoItemArteCompra);

                var pedidoItemArte = await pedidoItemArteRepository.GetByIdPedidoItem(request.IdPedidoItem, cancellationToken);

                if (pedidoItemArte == null)
                    throw new NotFoundException("id pedido item arte não encontrado!");               

                if (string.IsNullOrWhiteSpace(request.PedidoItemArteCompra.Login))
                    request.PedidoItemArteCompra.Login = userProvider.User.Login;

                if (request.PedidoItemArteCompra.DataCompra == null)
                    request.PedidoItemArteCompra.DataCompra = DateTime.Now;

                if (request.PedidoItemArteCompra.Id > 0)
                    request.PedidoItemArteCompra.Id = 0;

                request.PedidoItemArteCompra.PedidoItemArte = pedidoItemArte;
                request.PedidoItemArteCompra.IdPedidoItemArte = pedidoItemArte.Id;

                var usuario = await userRepository.GetByLogin(request.PedidoItemArteCompra.Login, cancellationToken);

                if (usuario == null)
                    throw new NotFoundException("login não encontrado!");

                if (!string.IsNullOrWhiteSpace(request.PedidoItemArteCompra.PedidoItemArte.CompradoPorLogin))
                    request.PedidoItemArteCompra.Usuario = usuario;
                else
                    throw new NotFoundException("login atribuido não tem perfil de comprador externo ou ainda não possui comprador atribuído.");

                if (pedidoItemArte.IdStatus != (int)PedidoItemArteStatus.ITEM_ATRIBUIDOAOCOMPRADOR)
                    throw new NotFoundException("Item pedido ainda não foi atribuído a um comprador.");

                if (pedidoItemArte.QuantidadePendenteCompra < request.PedidoItemArteCompra.Quantidade)
                    throw new NotFoundException("Quantidade informada é superior a quantidade pendente de compra.");

                pedidoItemCompraRepository.AddOrUpdate(request.PedidoItemArteCompra, cancellationToken);

                var result = unitOfWork.SaveChanges();

                if (!result) throw new ApplicationException("An error has occured.");

                await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = pedidoItemArte.PedidoItem });

            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }

            return Unit.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task<Unit> IRequestHandler<CreatePedidoItemArteCompras, Unit>.Handle(CreatePedidoItemArteCompras request, CancellationToken cancellationToken)
        {
            try
            {
                RunEntityValidationList(request.PedidoItemArteCompras);

                unitOfWork.BeginTransaction();

                foreach (var pedidoItemArteCompra in request.PedidoItemArteCompras)
                {
                    var pedidoItemArte = await pedidoItemArteRepository.GetById(pedidoItemArteCompra.IdPedidoItemArte, cancellationToken);
                    if (pedidoItemArte == null)
                        throw new NotFoundException("id pedido item não encontrado!");
                    else
                        pedidoItemArteCompra.PedidoItemArte = pedidoItemArte;

                    if (string.IsNullOrWhiteSpace(pedidoItemArteCompra.Login))
                        pedidoItemArteCompra.Login = userProvider.User.Login;

                    if (pedidoItemArteCompra.DataCompra == null)
                        pedidoItemArteCompra.DataCompra = DateTime.Now;

                    if (pedidoItemArteCompra.Id > 0)
                        pedidoItemArteCompra.Id = 0;

                    var usuario = await userRepository.GetByLogin(pedidoItemArteCompra.Login, cancellationToken);
                    if (usuario == null)
                        throw new NotFoundException("login não encontrado!");
                    else
                    {
                        //if (userProvider.IsRole(Role.PERFIL_COMPRADOR_EXTERNA) && !string.IsNullOrWhiteSpace(pedidoItemCompras.PedidoItem.LoginComprador))
                        if (!string.IsNullOrWhiteSpace(pedidoItemArteCompra.PedidoItemArte.CompradoPorLogin))
                            pedidoItemArteCompra.Usuario = usuario;
                        else
                            throw new NotFoundException("login atribuido não tem perfil de comprador externo ou ainda não possui comprador atribuído.");
                    }

                    if (pedidoItemArte.IdStatus != (int)Domain.Enums.PedidoItemArteStatus.ITEM_ATRIBUIDOAOCOMPRADOR)
                        throw new NotFoundException("Item pedido ainda não foi atribuído a um comprador.");

                    if (pedidoItemArte.QuantidadePendenteCompra < pedidoItemArteCompra.Quantidade)
                        throw new NotFoundException("Quantidade informada é superior a quantidade pendente de compra.");

                    pedidoItemCompraRepository.AddOrUpdate(pedidoItemArteCompra, cancellationToken);
                    var result = unitOfWork.SaveChanges();
                    if (!result) throw new ApplicationException("An error has occured.");

                    await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = pedidoItemArte.PedidoItem});
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
    }
}