using System;
using AutoMapper;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Interfaces;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.Enums;
using Globo.PIC.Domain.Types.Queries;
using Globo.PIC.Domain.Exceptions;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using Globo.PIC.Domain.Models;
using Globo.PIC.Domain.Types.Events;
using Microsoft.Extensions.Logging;

namespace Globo.PIC.Application.Veiculo.CommandHandlers
{
    public class PedidoVeiculoCommandHandler :
        IRequestHandler<CreatePedidoVeiculo>,
        IRequestHandler<UpdatePedidoVeiculo>,
        IRequestHandler<MudarStatusPedidoVeiculo>,
        IRequestHandler<VincularCompradorPedidoVeiculo>

    {
        private readonly IUserProvider userProvider;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<Equipe> pedidoEquipeRepository;
        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<PedidoItem> pedidoItemRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ITasksProxy projectTask;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<StatusPedidoVeiculo> statusRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<StatusPedidoItemVeiculo> statusItemRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<Usuario> userRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<Pedido> pedidoRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<PedidoVeiculo> pedidoVeiculoRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<PedidoItemVeiculo> pedidoItemVeiculoRepository;

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
		private readonly IMapper mapper;

        /// <summary>
        /// 
        /// </summary>
        private readonly IPurchaseRequisitionProxy purchaseRequisitionProxy;

        /// <summary>
        /// 
        /// </summary>  
        public PedidoVeiculoCommandHandler(
            IUnitOfWork _unitOfWork,
            IMediator _mediator,
            IRepository<Pedido> _pedidoRepository,
            IRepository<PedidoVeiculo> _pedidoVeiculoRepository,
            IRepository<PedidoItemVeiculo> _pedidoItemVeiculoRepository,
            IRepository<StatusPedidoVeiculo> _statusRepository,
            IRepository<StatusPedidoItemVeiculo> _statusItemRepository,
            IRepository<Usuario> _userRepository,
            IRepository<Equipe> _pedidoEquipeRepository,
            IRepository<PedidoItem> _pedidoItemRepository,
            IUserProvider _userProvider,
            IPurchaseRequisitionProxy _purchaseRequisitionProxy,
            ITasksProxy _projectTask,
            IMapper _mapper
            )
        {
            unitOfWork = _unitOfWork;
            mediator = _mediator;
            pedidoRepository = _pedidoRepository;
            pedidoVeiculoRepository = _pedidoVeiculoRepository;
            pedidoItemVeiculoRepository = _pedidoItemVeiculoRepository;
            statusRepository = _statusRepository;
            statusItemRepository = _statusItemRepository;
            userRepository = _userRepository;
            pedidoEquipeRepository = _pedidoEquipeRepository;
            pedidoItemRepository = _pedidoItemRepository;
            userProvider = _userProvider;
            purchaseRequisitionProxy = _purchaseRequisitionProxy;
            projectTask = _projectTask;
            mapper = _mapper;
        }

        protected void RunEntityValidation(PedidoItem pedidoItem)
        {
            if (pedidoItem == null)
                throw new ApplicationException("O Pedido Item está vazio!");

            if (pedidoItem.LocalEntrega == "")
                throw new ApplicationException("O local de entrega é obrigatório!");

            if (pedidoItem.NomeItem == "")
                throw new ApplicationException("O Nome do item é obrigatório!");

            if (pedidoItem.RCs.Count() != 1)
                throw new ApplicationException("O item deve possuir uma e apenas uma RC!");
        }

        protected void RunEntityValidation(Pedido pedido)
        {
            if (pedido == null)
                throw new ApplicationException("O Pedido Item está vazio!");

            if (pedido.PedidoVeiculo.LocalFaturamento == "")
                throw new ApplicationException("O local de faturamento de veículo é obrigatório!");

            if (pedido.PedidoVeiculo.Pedido.IdConteudo <= 0)
                throw new ValidationException("O Conteúdo não pode ser nulo.");

            if (pedido.PedidoVeiculo.Pedido.IdProjeto <= 0)
                throw new ValidationException("O Projeto não pode ser nulo.");

            if (pedido.PedidoVeiculo.Pedido.IdTarefa <= 0)
                throw new ValidationException("A Tarefa não pode ser nulo.");
        }

        public void RunEntityValidationList(List<Pedido> pedidoViewModels)
        {
            if (pedidoViewModels.Count() == 0)
                throw new ApplicationException("O Pedido Item está vazio!");

            foreach (var pedido in pedidoViewModels)
                RunEntityValidation(pedido);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        async Task<Unit> IRequestHandler<CreatePedidoVeiculo, Unit>.Handle(CreatePedidoVeiculo request, CancellationToken cancellationToken)
        {

            if (request.PedidoVeiculo.Pedido.IdConteudo <= 0)
                throw new ValidationException("O Conteúdo não pode ser nulo.");

            if (request.PedidoVeiculo.Pedido.IdProjeto <= 0)
                throw new ValidationException("O Projeto não pode ser nulo.");

            if (request.PedidoVeiculo.Pedido.IdTarefa <= 0)
                throw new ValidationException("A Tarefa não pode ser nulo.");



            foreach (var equipe in request.PedidoVeiculo.Pedido.Equipe)
            {
                var usuario = await userRepository.GetByLogin(equipe.Login, cancellationToken);
                if (usuario == null)
                    throw new NotFoundException("usuário equipe não encontrado: " + equipe.Login);
                else
                {
                    equipe.Usuario = usuario;
                }
            }
            request.PedidoVeiculo.Id = 0;
            request.PedidoVeiculo.IdStatus = (int)PedidoVeiculoStatus.PEDIDO_RASCUNHO;
            var existStatus = await statusRepository.GetById(request.PedidoVeiculo.IdStatus, cancellationToken);

            if (existStatus == null)
                throw new NotFoundException("Status não encontrado");

            request.PedidoVeiculo.Status = existStatus;

            var tarefa = await projectTask.GetResultTaskAsync(request.PedidoVeiculo.Pedido.IdProjeto, request.PedidoVeiculo.Pedido.IdTarefa, cancellationToken);

            if (tarefa != null)
            {
                request.PedidoVeiculo.Pedido.IdTarefa = tarefa.Id;
                request.PedidoVeiculo.Pedido.DescricaoTarefa = tarefa.Description;
            }

            pedidoVeiculoRepository.AddOrUpdate(request.PedidoVeiculo, cancellationToken);

            var result = unitOfWork.SaveChanges();

            if (!result) throw new ApplicationException("An error has occured.");

            //mediator.Publish();

            return await Unit.Task;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        async Task<Unit> IRequestHandler<MudarStatusPedidoVeiculo, Unit>.Handle(MudarStatusPedidoVeiculo request, CancellationToken cancellationToken)
        {
            bool statusModificado = false;

            if (request.IdPedido <= 0)
                throw new ValidationException("O ID Pedido não pode ser nulo.");

            if (request.IdStatus <= 0)
                throw new ValidationException("O ID Status não pode ser nulo.");

            var existPedido = await pedidoRepository.GetById(request.IdPedido, cancellationToken);
            //var existPedido = pedidoRepository.GetAll().Where(a => a.Id == request.IdPedido).ToList();

            if (existPedido == null)
                throw new NotFoundException("Pedido não encontrado");

            //var existPedidoVeiculo = await pedidoVeiculoRepository.GetById(existPedido.PedidoVeiculo.Id, cancellationToken);
            var existPedidoVeiculo = pedidoVeiculoRepository.GetAll().Where(c => c.IdPedido == existPedido.Id).ToList();

            if (existPedidoVeiculo.Count <= 0)
                throw new NotFoundException("Pedido não encontrado");

            var existStatus = await statusRepository.GetById(request.IdStatus, cancellationToken);

            if (existStatus == null)
                throw new NotFoundException("Status não encontrado");

            if (existPedido.PedidoVeiculo.IdStatus != request.IdStatus)
                statusModificado = true;

            if (statusModificado)
            {

                switch (existPedidoVeiculo.FirstOrDefault().IdStatus)
                {
                    case (int)PedidoVeiculoStatus.PEDIDO_RASCUNHO:
                        {
                            if ((request.IdStatus != (int)PedidoVeiculoStatus.PEDIDO_ENVIADO) &&
                                (request.IdStatus != (int)PedidoVeiculoStatus.PEDIDO_CANCELADO))
                                throw new ApplicationException("Mudança do status do pedido não permitido para o status atual");

                            break;
                        }
                    case (int)PedidoVeiculoStatus.PEDIDO_ENVIADO:
                        {
                            if ((request.IdStatus == (int)PedidoVeiculoStatus.PEDIDO_RASCUNHO) &&
                                (request.IdStatus == (int)PedidoVeiculoStatus.PEDIDO_FINALIZADO))
                                throw new ApplicationException("Mudança do status do pedido não permitido para o status atual");

                            break;
                        }
                    case (int)PedidoVeiculoStatus.PEDIDO_EMANDAMENTO:
                        {
                            if ((request.IdStatus == (int)PedidoVeiculoStatus.PEDIDO_RASCUNHO) &&
                                (request.IdStatus == (int)PedidoVeiculoStatus.PEDIDO_ENVIADO) &&
                                (request.IdStatus == (int)PedidoVeiculoStatus.PEDIDO_DEVOLVIDO))
                                throw new ApplicationException("Mudança do status do pedido não permitido para o status atual");

                            break;
                        }
                    case (int)PedidoVeiculoStatus.PEDIDO_FINALIZADO:
                        {
                            if ((request.IdStatus != (int)PedidoVeiculoStatus.PEDIDO_CANCELADO))
                                throw new ApplicationException("Mudança do status do pedido não permitido para o status atual");

                            break;
                        }
                    case (int)PedidoVeiculoStatus.PEDIDO_CANCELADO:
                        {
                            if ((request.IdStatus != (int)PedidoVeiculoStatus.PEDIDO_CANCELADO))
                                throw new ApplicationException("Mudança do status do pedido não permitido para o status atual");

                            break;
                        }
                    default:
                        break;
                }

                if (request.IdStatus == (int)PedidoVeiculoStatus.PEDIDO_CANCELADO && string.IsNullOrEmpty(request.JustificativaCancelamento))
                    throw new ApplicationException("Justificativa de cancelamento é obrigatória");
                else
                {
                    if (request.IdStatus == (int)PedidoVeiculoStatus.PEDIDO_CANCELADO)
                    {
                        existPedidoVeiculo.FirstOrDefault().Pedido.JustificativaCancelamento = request.JustificativaCancelamento;
                        existPedidoVeiculo.FirstOrDefault().Pedido.DataCancelamento = DateTime.Now;
                        existPedidoVeiculo.FirstOrDefault().Pedido.CanceladoPorLogin= userProvider.User.Login;
                        var existUserCancelamento = await userRepository.GetByLogin(userProvider.User.Login, cancellationToken);

                        if (existUserCancelamento == null)
                            throw new NotFoundException("Usuário não encontrado");
                        existPedidoVeiculo.FirstOrDefault().Pedido.CanceladoPor = existUserCancelamento;
                    }
                }

                if (request.IdStatus == (int)PedidoVeiculoStatus.PEDIDO_DEVOLVIDO && string.IsNullOrEmpty(request.JustificativaDevolucao))
                    throw new ApplicationException("Justificativa de devolução é obrigatória");
                else
                {
                    if (request.IdStatus == (int)PedidoVeiculoStatus.PEDIDO_DEVOLVIDO)
                    {
                        existPedidoVeiculo.FirstOrDefault().Pedido.JustificativaDevolucao = request.JustificativaDevolucao;
                        existPedidoVeiculo.FirstOrDefault().Pedido.DataDevolucao = DateTime.Now;
                        existPedidoVeiculo.FirstOrDefault().Pedido.DevolvidoPorLogin = userProvider.User.Login;
                        var existUserDevolucao = await userRepository.GetByLogin(userProvider.User.Login, cancellationToken);

                        if (existUserDevolucao == null)
                            throw new NotFoundException("Usuário não encontrado");
                        existPedidoVeiculo.FirstOrDefault().Pedido.DevolvidoPor = existUserDevolucao;
                    }
                }

                if (request.IdStatus == (int)PedidoVeiculoStatus.PEDIDO_ENVIADO)
                    existPedidoVeiculo.FirstOrDefault().DataEnvio = DateTime.Now;


                existPedidoVeiculo.FirstOrDefault().IdStatus = request.IdStatus;
                existPedidoVeiculo.FirstOrDefault().Status = existStatus;

                //todo: remover persistência do EventHandle, persistir nos CommandHandlers
                pedidoVeiculoRepository.AddOrUpdate(existPedidoVeiculo.FirstOrDefault(), cancellationToken);

                var result = unitOfWork.SaveChanges();
                if (!result) throw new ApplicationException("An error has occured.");
                
                await mediator.Publish(new OnStatusPedidoVeiculoAlterado()
                {
                    Pedido = existPedidoVeiculo.FirstOrDefault(),
                    idStatus = request.IdStatus

                });
                
            }

            return await Unit.Task;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        async Task<Unit> IRequestHandler<UpdatePedidoVeiculo, Unit>.Handle(UpdatePedidoVeiculo request, CancellationToken cancellationToken)
        {
            bool
                //dtNecessidadeModificada = false,
                //dtReenvioModificada = false,
                statusModificado = false;

            if (request.PedidoVeiculo.Pedido.IdConteudo <= 0)
                throw new ValidationException("O Conteúdo não pode ser nulo.");

            if (request.PedidoVeiculo.Pedido.IdProjeto <= 0)
                throw new ValidationException("O Projeto não pode ser nulo.");

            if (request.PedidoVeiculo.Pedido.IdTarefa <= 0)
                throw new ValidationException("A Tarefa não pode ser nulo.");

            if (string.IsNullOrWhiteSpace(request.PedidoVeiculo.LocalFaturamento))
                throw new ValidationException("O Local de Faturamento não pode ser nulo.");

            var existPedido = await pedidoRepository.GetById(request.PedidoVeiculo.Id, cancellationToken);

            if (existPedido == null)
                throw new NotFoundException("Pedido não encontrado");

            //var existPedidoVeiculo = await pedidoVeiculoRepository.GetById(request.PedidoVeiculo.Pedido.Id, cancellationToken);
            var existPedidoVeiculo = pedidoVeiculoRepository.GetAll().Where(c => c.IdPedido == existPedido.Id).ToList();

            if (existPedidoVeiculo.Count <= 0)
                throw new NotFoundException("Pedido Item não encontrado");

            statusModificado = existPedidoVeiculo.FirstOrDefault().IdStatus != request.PedidoVeiculo.IdStatus;

            existPedido = await pedidoRepository.GetById(request.PedidoVeiculo.Pedido.Id, cancellationToken);

            request.PedidoVeiculo.Id = existPedido.PedidoVeiculo.Id;
            request.PedidoVeiculo.Pedido = existPedido;

            if (request.PedidoVeiculo.Pedido.Equipe != null)
            {
                List<Equipe> newEquipe = new List<Equipe>();

                var equipes = pedidoEquipeRepository.GetAll().Where(a => a.IdPedido == existPedido.Id).ToList();

                pedidoEquipeRepository.Remove(equipes);

                foreach (var equipe in request.PedidoVeiculo.Pedido.Equipe)
                {
                    var usuario = await userRepository.GetByLogin(equipe.Login, cancellationToken);
                    if (usuario == null)
                        throw new NotFoundException("usuário equipe não encontrado: " + equipe.Login);
                    else
                    {
                        newEquipe.Add(new Equipe()
                        {
                            Login = usuario.Login,
                            Usuario = usuario
                        });
                    }
                }

                existPedido.Equipe = newEquipe;
            }

            existPedido.Titulo = request.PedidoVeiculo.Pedido.Titulo;
            existPedido.IdConteudo = request.PedidoVeiculo.Pedido.IdConteudo;
            existPedido.IdProjeto = request.PedidoVeiculo.Pedido.IdProjeto;
            existPedido.LocalEntrega = request.PedidoVeiculo.Pedido.LocalEntrega;
            existPedido.Finalidade = request.PedidoVeiculo.Pedido.Finalidade;
            existPedido.CentroCusto = request.PedidoVeiculo.Pedido.CentroCusto;
            existPedido.Conta = request.PedidoVeiculo.Pedido.Conta;

            if (request.PedidoVeiculo.Pedido.IdTarefa != existPedido.IdTarefa)
            {
                var tarefa = await projectTask.GetResultTaskAsync(request.PedidoVeiculo.Pedido.IdProjeto, request.PedidoVeiculo.Pedido.IdTarefa, cancellationToken);

                if (tarefa != null)
                {
                    existPedido.IdTarefa = tarefa.Id;
                    existPedido.DescricaoTarefa = tarefa.Description;
                }
            }

            await mediator.Send(new UpdatePedido() { Pedido = existPedido }, cancellationToken);

            if (request.PedidoVeiculo.DataChegada.HasValue)
                existPedidoVeiculo.FirstOrDefault().DataChegada = request.PedidoVeiculo.DataChegada;

            if (request.PedidoVeiculo.DataDevolucao.HasValue)
                existPedidoVeiculo.FirstOrDefault().DataDevolucao = request.PedidoVeiculo.DataDevolucao;

            existPedidoVeiculo.FirstOrDefault().PreProducao = request.PedidoVeiculo.PreProducao;
            existPedidoVeiculo.FirstOrDefault().LocalFaturamento = request.PedidoVeiculo.LocalFaturamento;

            pedidoVeiculoRepository.AddOrUpdate(existPedidoVeiculo.FirstOrDefault(), cancellationToken);

            var result = unitOfWork.SaveChanges();

            if (!result) throw new ApplicationException("An error has occured.");

            return await Unit.Task;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        async Task<Unit> IRequestHandler<VincularCompradorPedidoVeiculo, Unit>.Handle(VincularCompradorPedidoVeiculo request, CancellationToken cancellationToken)
        {
            bool statusModificado = false;

            if (request.IdPedido <= 0)
                throw new ValidationException("O ID Pedido não pode ser nulo.");

            if (request.IdStatus <= 0)
                throw new ValidationException("O ID Status não pode ser nulo.");

            var existPedido = await pedidoRepository.GetByIdPedidoWithOutRoles(request.IdPedido, cancellationToken);
            //var existPedido = pedidoRepository.GetAll().Where(a => a.Id == request.IdPedido).ToList();

            if (existPedido == null)
                throw new NotFoundException("Pedido não encontrado");

            request.IdStatus = (int)PedidoVeiculoStatus.PEDIDO_EMANDAMENTO;

            if (existPedido.PedidoVeiculo.IdStatus != request.IdStatus && existPedido.PedidoVeiculo.IdStatus == (int)PedidoVeiculoStatus.PEDIDO_ENVIADO)
                statusModificado = true;
            else
                return await Unit.Task;

            var existPedidoItem = pedidoItemRepository.GetAll().Where(a => a.IdPedido == request.IdPedido).ToList();

            if (existPedidoItem.Count <= 0)
                return await Unit.Task;
                //throw new NotFoundException("Pedido item não encontrado");

            var existPedidoVeiculo = pedidoVeiculoRepository.GetAll().Where(c => c.IdPedido == existPedido.Id).ToList();

            if (existPedidoVeiculo.Count <= 0)
                return await Unit.Task;
                //throw new NotFoundException("Pedido não encontrado");

            var existStatus = await statusRepository.GetById(request.IdStatus, cancellationToken);

            if (existStatus == null)
                return await Unit.Task;
                //throw new NotFoundException("Status não encontrado");

            var existUser = await userRepository.GetByLogin(userProvider.User.Login, cancellationToken);

            if (existUser == null)
                return await Unit.Task;
                //throw new NotFoundException("Usuário não encontrado");

            var existPerfil = existUser.Roles.Where(a => a.Name == "PERFIL_COMPRADOR_VEICULOS").ToList();

            if (existPerfil.Count <= 0)
                return await Unit.Task;
                //throw new NotFoundException("Perfil não é de comprador de veículos");

            if (statusModificado)
            {

                switch (existPedidoVeiculo.FirstOrDefault().IdStatus)
                {
                    case (int)PedidoVeiculoStatus.PEDIDO_RASCUNHO:
                        {
                            if ((request.IdStatus != (int)PedidoVeiculoStatus.PEDIDO_ENVIADO) && (request.IdStatus != (int)PedidoVeiculoStatus.PEDIDO_CANCELADO))
                                throw new ApplicationException("Mudança do status do pedido não permitido para o status atual");

                            break;
                        }
                    case (int)PedidoVeiculoStatus.PEDIDO_ENVIADO:
                        {
                            if ((request.IdStatus == (int)PedidoVeiculoStatus.PEDIDO_RASCUNHO) && (request.IdStatus == (int)PedidoVeiculoStatus.PEDIDO_FINALIZADO))
                                throw new ApplicationException("Mudança do status do pedido não permitido para o status atual");

                            break;
                        }
                    case (int)PedidoVeiculoStatus.PEDIDO_EMANDAMENTO:
                        {
                            if ((request.IdStatus == (int)PedidoVeiculoStatus.PEDIDO_RASCUNHO) && (request.IdStatus == (int)PedidoVeiculoStatus.PEDIDO_ENVIADO))
                                throw new ApplicationException("Mudança do status do pedido não permitido para o status atual");

                            break;
                        }
                    case (int)PedidoVeiculoStatus.PEDIDO_FINALIZADO:
                        {
                            if ((request.IdStatus != (int)PedidoVeiculoStatus.PEDIDO_CANCELADO))
                                throw new ApplicationException("Mudança do status do pedido não permitido para o status atual");

                            break;
                        }
                    case (int)PedidoVeiculoStatus.PEDIDO_CANCELADO:
                        {
                            if ((request.IdStatus != (int)PedidoVeiculoStatus.PEDIDO_CANCELADO))
                                throw new ApplicationException("Mudança do status do pedido não permitido para o status atual");

                            break;
                        }
                    default:
                        break;
                }

                existPedidoVeiculo.FirstOrDefault().IdStatus = request.IdStatus;
                existPedidoVeiculo.FirstOrDefault().Status = existStatus;
                existPedidoVeiculo.FirstOrDefault().CompradoPorLogin = existUser.Login;
                existPedidoVeiculo.FirstOrDefault().Comprador = existUser;

                //todo: remover persistência do EventHandle, persistir nos CommandHandlers
                pedidoVeiculoRepository.AddOrUpdate(existPedidoVeiculo.FirstOrDefault(), cancellationToken);

                var result = unitOfWork.SaveChanges();
                if (!result) throw new ApplicationException("An error has occured.");

                await mediator.Publish(new OnStatusPedidoVeiculoAlterado()
                {
                    Pedido = existPedidoVeiculo.FirstOrDefault(),
                    idStatus = request.IdStatus

                });

            }

            return await Unit.Task;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ///
        /*
        async Task<Unit> IRequestHandler<UpdatePedidoItemVeiculo, Unit>.Handle(UpdatePedidoItemVeiculo request, CancellationToken cancellationToken)
        {

            try
            {
                bool statusChange = false;

                RunEntityValidation(request.PedidoItemVeiculo.PedidoItem);

                var existPedidoItem = await pedidoRepository.GetById(request.PedidoItemVeiculo.IdPedidoItem, cancellationToken);

                if (existPedidoItem == null)
                    throw new BadRequestException("id pedido item não encontrado!");

                var vltotal = request.PedidoItemVeiculo.PedidoItem.Quantidade * request.PedidoItemVeiculo.PedidoItem.Valor;

                request.PedidoItemVeiculo.PedidoItem.ValorItens = vltotal;

                await mediator.Send(new UpdatePedidoItem()
                {
                    PedidoItem = request.PedidoItemVeiculo.PedidoItem
                }, cancellationToken);
                
                existPedidoItem = await pedidoItemRepository.GetById(request.PedidoItemVeiculo.PedidoItem.Id, cancellationToken);

                request.PedidoItemArte.Id = existPedidoItem.PedidoItemArte.Id;
                request.PedidoItemArte.PedidoItem = existPedidoItem;

                var pedidoItemCompra = pedidoItemCompraRepository.GetAll().Where(a => a.IdPedidoItemArte == request.PedidoItemArte.Id).ToList();

                if (pedidoItemCompra.Count > 0)
                    throw new NotFoundException(string.Format("Item {0} não pode ser editado, já existe compra vinculada.", request.PedidoItemArte.IdPedidoItem));

                if (request.PedidoItemArte.IdStatus == 0)
                {
                    if (existPedidoItem.PedidoItemArte.IdStatus == 0)
                        if (string.IsNullOrWhiteSpace(existPedidoItem.RCs.FirstOrDefault().Acordo))
                        {
                            request.PedidoItemArte.IdStatus = (int)StatusItem.ITEM_SOLICITACAODECOMPRACRIADA;
                            statusChange = true;
                        }
                        else
                        {
                            request.PedidoItemArte.IdStatus = (int)StatusItem.ITEM_SOLICITACAODECOMPRAENVIADA;
                            statusChange = true;
                        }
                    else
                        request.PedidoItemArte.IdStatus = existPedidoItem.PedidoItemArte.IdStatus;
                }
                else
                    statusChange = true;


                if (!string.IsNullOrWhiteSpace(existPedidoItem.PedidoItemArte.CompradoPorLogin) && existPedidoItem.PedidoItemArte.CompradoPorLogin != null)
                {
                    request.PedidoItemArte.CompradoPorLogin = existPedidoItem.PedidoItemArte.CompradoPorLogin;
                    request.PedidoItemArte.CompradoPor = existPedidoItem.PedidoItemArte.CompradoPor;
                }
                else if (userProvider.IsRole(Role.PERFIL_BASE_SUPRIMENTOS) && !string.IsNullOrWhiteSpace(request.PedidoItemArte.CompradoPorLogin))
                {
                    request.PedidoItemArte.IdStatus = (int)StatusItem.ITEM_ATRIBUIDOAOCOMPRADOR;
                    statusChange = true;
                    request.PedidoItemArte.CompradoPor = await userRepository.GetByLogin(request.PedidoItemArte.CompradoPorLogin, cancellationToken);
                    if (!DBNull.Value.Equals(request.PedidoItemArte.DataVinculoComprador) || (request.PedidoItemArte.DataVinculoComprador != existPedidoItem.PedidoItemArte.DataVinculoComprador))
                        request.PedidoItemArte.DataVinculoComprador = DateTime.Now;

                    await mediator.Publish(new OnItemAtribuidoComprador() { Pedido = existPedido });
                }
                else if (!userProvider.IsRole(Role.PERFIL_BASE_SUPRIMENTOS) && !string.IsNullOrWhiteSpace(request.PedidoItemArte.CompradoPorLogin))
                    throw new BadRequestException("Somente usuário da base de suprimentos pode vincular um comprador.");

                var status = await statusItemRepository.GetById(request.PedidoItemArte.IdStatus, cancellationToken);
                if (status == null)
                    throw new NotFoundException("status não encontrado!");
                else
                    request.PedidoItemArte.Status = status;

                if (!string.IsNullOrWhiteSpace(request.PedidoItemArte.CompradoPorLogin))
                {
                    if (!string.IsNullOrWhiteSpace(existPedidoItem.RCs.FirstOrDefault().Acordo))
                        throw new BadRequestException("Não é permitido atribuir comprador para itens com acordo.");

                    request.PedidoItemArte.CompradoPor = await userRepository.GetByLogin(request.PedidoItemArte.CompradoPorLogin, cancellationToken);
                }

                if (!string.IsNullOrWhiteSpace(request.PedidoItemArte.PedidoItem.ReprovadoPorLogin))
                    request.PedidoItemArte.PedidoItem.ReprovadoPor = await userRepository.GetByLogin(request.PedidoItemArte.PedidoItem.ReprovadoPorLogin, cancellationToken);

                request.PedidoItemArte.TrackingArte = trackingRepository.GetAll().Where(a => a.IdPedidoItemArte == request.PedidoItemArte.Id).ToList();

                pedidoItemArteRepository.AddOrUpdate(request.PedidoItemArte, cancellationToken);

                var result = unitOfWork.SaveChanges();

                if (!result) throw new ApplicationException("An error has occured.");

                if (statusChange)
                {
                    await mediator.Send(new AddTrackingArte()
                    {
                        PedidoItemArteTracking = new PedidoItemArteTracking()
                        {
                            IdPedidoItemArte = request.PedidoItemArte.Id,
                            StatusId = request.PedidoItemArte.IdStatus,
                            ChangeById = existPedido.CriadoPorLogin
                        }
                    }, cancellationToken);
                }

                //var pedidoWithOutRoles = pedidoRepository.GetByIdPedidoWithOutRoles(request.PedidoItem.Pedido.Id, cancellationToken).Result;

                await mediator.Publish(new OnVerificarStatus() { Pedido = existPedido });

                await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = request.PedidoItemArte.PedidoItem });
                
            }
            catch (Exception error)
            {
                throw new ApplicationException(error.Message);
            }
            return Unit.Value;
        }
        */
        //        /// <summary>
        //        /// 
        //        /// </summary>
        //        /// <param name="request"></param>
        //        async Task<Unit> IRequestHandler<UpdatePedido, Unit>.Handle(UpdatePedido request, CancellationToken cancellationToken)
        //        {
        //            bool
        //                dtNecessidadeModificada = false,
        //                dtReenvioModificada = false,
        //                statusModificado = false;

        //            if (request.Pedido.IdConteudo <= 0)
        //                throw new ValidationException("O Conteúdo não pode ser nulo.");

        //            if (request.Pedido.IdProjeto <= 0)
        //                throw new ValidationException("O Projeto não pode ser nulo.");

        //            if (request.Pedido.IdTarefa <= 0)
        //                throw new ValidationException("A Tarefa não pode ser nulo.");

        //            if (string.IsNullOrWhiteSpace(request.Pedido.LocalUtilizacao))
        //                throw new ValidationException("O Local de Utilização não pode ser nulo.");

        //            var existPedido = await pedidoRepository.GetByIdPedidoWithOutRoles(request.Pedido.Id, cancellationToken);

        //            if (existPedido == null)
        //                throw new NotFoundException("Pedido não encontrado");

        //            //todo: persistir aqui a data necessidades
        //            dtNecessidadeModificada = !(
        //                (existPedido.DataNecessidade.HasValue ? existPedido.DataNecessidade.Value.ToShortDateString() : string.Empty)
        //                == 
        //                (request.Pedido.DataNecessidade.HasValue ? request.Pedido.DataNecessidade.Value.ToShortDateString() : string.Empty)
        //            );

        //            List<Nivel> niveis = new List<Nivel>();

        //            dtReenvioModificada = !(
        //                (existPedido.DataReenvio.HasValue ? existPedido.DataReenvio.Value.ToShortDateString() : string.Empty)
        //                ==
        //                (request.Pedido.DataReenvio.HasValue ? request.Pedido.DataReenvio.Value.ToShortDateString() : string.Empty)
        //            );

        //            statusModificado = existPedido.IdStatus != request.Pedido.IdStatus;

        //            //compradorModificado = request.Pedido.; //existPedido.IdStatus != request.Pedido.IdStatus;

        //            if (request.Pedido.Equipe != null)
        //            {
        //                List<Equipe> newEquipe = new List<Equipe>();

        //                var equipes = pedidoEquipeRepository.GetAll().Where(a => a.IdPedido == existPedido.Id).ToList();
        //                pedidoEquipeRepository.Remove(equipes);                

        //                foreach (var equipe in request.Pedido.Equipe)
        //                {
        //                    var usuario = await userRepository.GetByLogin(equipe.Login, cancellationToken);
        //                    if (usuario == null)
        //                        throw new NotFoundException("usuário equipe não encontrado: " + equipe.Login);
        //                    else
        //                    {
        //                        newEquipe.Add(new Equipe()
        //                        {
        //                            Login = usuario.Login,
        //                            User = usuario
        //                        });                        
        //                    }
        //                }

        //                existPedido.Equipe = newEquipe;
        //            }

        //            if (!string.IsNullOrWhiteSpace(request.Pedido.LoginAutorizacao))
        //            {
        //                existPedido.UserAutorizacao = await userRepository.GetByLogin(request.Pedido.LoginAutorizacao, cancellationToken);
        //                existPedido.DataAutorizacao = DateTime.Now;
        //            }


        //            existPedido.DataNecessidade = request.Pedido.DataNecessidade;
        //            existPedido.Titulo=request.Pedido.Titulo;
        //            existPedido.IdConteudo=request.Pedido.IdConteudo;
        //            existPedido.IdProjeto = request.Pedido.IdProjeto;

        //            existPedido.LocalEntrega=request.Pedido.LocalEntrega;
        //            existPedido.LocalUtilizacao=request.Pedido.LocalUtilizacao;
        //            existPedido.DescricaoCena=request.Pedido.DescricaoCena;

        //            existPedido.Finalidade=request.Pedido.Finalidade;
        //            existPedido.CentroCusto=request.Pedido.CentroCusto;
        //            existPedido.Conta=request.Pedido.Conta;

        //            if (request.Pedido.DataEdicaoReenvio.HasValue)
        //                existPedido.DataEdicaoReenvio = request.Pedido.DataEdicaoReenvio.Value;

        //            if (request.Pedido.DataReenvio.HasValue)
        //                existPedido.DataReenvio = request.Pedido.DataReenvio.Value;

        //            if (request.Pedido.IdTarefa != existPedido.IdTarefa)
        //            {
        //                var tarefa = await projectTask.GetResultTaskAsync(request.Pedido.IdProjeto, request.Pedido.IdTarefa, cancellationToken);
        //                if (tarefa != null)
        //                {
        //                    existPedido.IdTarefa = tarefa.Id;
        //                    existPedido.DescricaoTarefa = tarefa.Description;
        //                }
        //            } 

        //            pedidoRepository.AddOrUpdate(existPedido, cancellationToken);

        //            var result = unitOfWork.SaveChanges();

        //            if (!result) throw new ApplicationException("An error has occured.");

        //            if (dtNecessidadeModificada)
        //                await mediator.Publish(new OnDataNecessidadeAlterada() { Pedido = existPedido });

        //            if (dtReenvioModificada)
        //                await mediator.Publish(new OnDataReenvioAlterada() { Pedido = existPedido });

        //            if (!string.IsNullOrWhiteSpace(request.LoginComprador) && (existPedido.IdStatus == (int)StatusPedido.PEDIDO_EMANDAMENTO))
        //            {
        //                if (request.Pedido.IdTipo > 0)
        //                    existPedido.IdTipo = request.Pedido.IdTipo;
        //                else
        //                    throw new NotFoundException("O Tipo do pedido não pode ser nulo quando o comprador é atribuído");

        //                if (!string.IsNullOrWhiteSpace(request.Pedido.Observacao))
        //                    existPedido.Observacao = request.Pedido.Observacao;

        //                var pedidoVM = mapper.Map<PedidoModel>(existPedido);
        //                pedidoVM.LoginComprador = request.LoginComprador;
        //                await mediator.Publish(new OnCompradorAlterado() { Pedido = pedidoVM }); 
        //            }

        //            if (!string.IsNullOrWhiteSpace(request.Pedido.LoginBase) && (existPedido.IdStatus == (int)StatusPedido.PEDIDO_ENVIADO))
        //            {
        //                existPedido.LoginBase = request.Pedido.LoginBase;

        //                var pedidoVM = mapper.Map<PedidoModel>(existPedido);
        //                await mediator.Publish(new OnBaseAlterada() { Pedido = pedidoVM });

        //                existPedido.IdStatus = (int)StatusPedido.PEDIDO_EMANDAMENTO;

        //                await mediator.Publish(new OnNovoPedido() { Pedido = existPedido });

        //            }

        //            if (statusModificado)
        //            {
        //                await mediator.Publish(new OnStatusPedidoAlterado() { 
        //                    Pedido = request.Pedido
        //                });


        //            }

        //            return await Unit.Task;
        //        }

        //        async Task<Unit> IRequestHandler<UpdatePedidoStatus, Unit>.Handle(UpdatePedidoStatus request, CancellationToken cancellationToken)
        //        {

        //            var status = await statusRepository.GetById(request.IdStatus, cancellationToken);
        //            if (status == null)
        //                throw new NotFoundException("Status não encontrado");

        //            var pedido = await pedidoRepository.GetByIdPedidoWithOutRoles(request.IdPedido, cancellationToken);
        //            if (pedido == null)
        //                throw new NotFoundException("Pedido não encontrado");

        //            pedido.IdStatus = status.Id;
        //            pedido.Status = status;

        //            pedidoRepository.AddOrUpdate(pedido, cancellationToken);

        //            var result = unitOfWork.SaveChanges();

        //            if (!result) throw new ApplicationException("An error has occured.");

        ////            var pedidoWithOutRoles = await pedidoRepository.GetByIdPedidoWithOutRoles(pedido.Id, cancellationToken);

        //            await mediator.Publish(new OnVerificarStatus()
        //            {
        //                Pedido = pedido
        //            });

        //            return await Unit.Task;
        //        }

        //        async Task<Unit> IRequestHandler<UpdatePedidoDevolucao, Unit>.Handle(UpdatePedidoDevolucao request, CancellationToken cancellationToken)
        //        {

        //            return await Unit.Task;
        //        }

        /// <summary>
        /// Serializes Entities Objects preventing the Loop Reference error
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        private static string SerializeEntityObject(object entityObject)
        {
            return JsonConvert.SerializeObject(entityObject, Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
        }

    }
}