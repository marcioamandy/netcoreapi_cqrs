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
using Globo.PIC.Infra.Data.Repositories;

namespace Globo.PIC.Application.Veiculo.CommandHandlers
{
    public class PedidoItemVeiculoCommandHandler :
        IRequestHandler<CreatedPedidoItemVeiculo>,
        IRequestHandler<UpdatePedidoItemVeiculo>,
        IRequestHandler<MudarStatusPedidoItemVeiculo>,
        IRequestHandler<DeletePedidoItensVeiculo>,
        IRequestHandler<DeletePedidoItemVeiculo>
    //IRequestHandler<UpdateItemPedido>,
    //IRequestHandler<UpdatePedidoItemStatus>,
    //IRequestHandler<UpdatePedidoItemDevolucao>
    {
        private readonly IUserProvider userProvider;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<PedidoItem> pedidoItemRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly PedidoItemVeiculoRepository pedidoItemVeiculoRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly PedidoVeiculoRepository pedidoVeiculoRepository;

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
		private readonly IRepository<Pedido> pedidoRepository;

        /// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemAnexo> pedidoItemAnexosRepository;

        /// <summary>
		/// 
		/// </summary>
		private readonly IRepository<ItemCatalogo> itemCatalogoRepository;

        /// <summary>
		/// 
		/// </summary>
		private readonly IRepository<Item> itemRepository;

        /// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemVeiculoTracking> trackingRepository;

        /// <summary>
		/// 
		/// </summary>
		private readonly IRepository<Categoria> categoriaRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<Usuario> userRepository;

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
        public PedidoItemVeiculoCommandHandler(
            IUnitOfWork _unitOfWork,
            IMediator _mediator,
            IRepository<PedidoItem> _pedidoItemRepository,
            IRepository<StatusPedidoItemVeiculo> _statusItemRepository,
            IRepository<Usuario> _userRepository,
            IRepository<PedidoItemVeiculo> _pedidoItemVeiculoRepository,
            IRepository<PedidoVeiculo> _pedidoVeiculoRepository,
            IRepository<Pedido> _pedidoRepository,
            IRepository<PedidoItemAnexo> _pedidoItemAnexosRepository,
            IRepository<PedidoItemVeiculoTracking> _trackingRepository,
            IRepository<ItemCatalogo> _itemCatalogoRepository,
            IRepository<StatusPedidoVeiculo> _statusRepository,
            IRepository<Item> _itemRepository,
            IRepository<Categoria> _categoriaRepository,
            IUserProvider _userProvider,
            IPurchaseRequisitionProxy _purchaseRequisitionProxy,
            ITasksProxy _projectTask,
            IMapper _mapper
            )
        {
            unitOfWork = _unitOfWork;
            mediator = _mediator;
            statusItemRepository = _statusItemRepository;
            userRepository = _userRepository;
            pedidoItemRepository = _pedidoItemRepository as PedidoItemRepository;
            pedidoItemVeiculoRepository = _pedidoItemVeiculoRepository as PedidoItemVeiculoRepository;
            pedidoVeiculoRepository = _pedidoVeiculoRepository as PedidoVeiculoRepository;
            pedidoRepository = _pedidoRepository;
            pedidoItemAnexosRepository = _pedidoItemAnexosRepository;
            trackingRepository = _trackingRepository;
            itemCatalogoRepository = _itemCatalogoRepository;
            itemRepository = _itemRepository;
            statusRepository = _statusRepository;
            categoriaRepository = _categoriaRepository;
            userProvider = _userProvider;
            purchaseRequisitionProxy = _purchaseRequisitionProxy;
            projectTask = _projectTask;
            mapper = _mapper;
        }

        protected void RunEntityValidation(PedidoItem pedido)
        {
            if (pedido == null)
                throw new ApplicationException("O Pedido Item está vazio!");

            if (pedido.PedidoItemVeiculo.IdTipo == 0)
                throw new ApplicationException("O tipo de veículo é obrigatório!");

            if (pedido.PedidoItemVeiculo.IdSubCategoria == 0)
                throw new ApplicationException("A subCategoria do item é obrigatório!");

            if (pedido.PedidoItemVeiculo.Modelo == "")
                throw new ApplicationException("O Modelo do item é obrigatório!");

            if (pedido.PedidoItemVeiculo.NroOpcoes == 0)
                throw new ApplicationException("O Nro de opções do item é obrigatório!");
        }

        public void RunEntityValidationList(List<PedidoItem> pedidoItemViewModels)
        {
            if (pedidoItemViewModels.Count() == 0)
                throw new ApplicationException("O Pedido Item está vazio!");

            foreach (var pedido in pedidoItemViewModels)
                RunEntityValidation(pedido);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        async Task<Unit> IRequestHandler<CreatedPedidoItemVeiculo, Unit>.Handle(CreatedPedidoItemVeiculo request, CancellationToken cancellationToken)
        {

            if (string.IsNullOrWhiteSpace(request.PedidoItemVeiculo.Modelo))
                throw new ValidationException("O Modelo não pode ser nulo.");

            if (request.PedidoItemVeiculo.IdTipo <= 0)
                throw new ValidationException("O Tipo de veículo não pode ser nulo.");

            if (request.PedidoItemVeiculo.IdSubCategoria <= 0)
                throw new ValidationException("A Subcategoria não pode ser nulo.");

            if (request.PedidoItemVeiculo.NroOpcoes <= 0)
                throw new ValidationException("O número de opções não pode ser nulo.");

            // Criação de item sempre vai ser com status = rascunho
            request.PedidoItemVeiculo.IdStatus = (int)PedidoItemVeiculoStatus.PEDIDOITEM_RASCUNHO;

            var statusVeiculo = await statusItemRepository.GetById(request.PedidoItemVeiculo.IdStatus, cancellationToken);
            //svar statusVeiculo = await mediator.Send<StatusPedidoItemVeiculo>(new GetById() { Id = request.PedidoItemVeiculo.IdStatus }, cancellationToken);

            if (statusVeiculo == null) throw new NotFoundException("Registro do status não encontrado.");

            /*
            if (pedido.IdConteudo != pedidoItens.PedidoItem.Pedido.IdConteudo)
                pedidoItemVM.OrigemVeiculo = "Veículo de Empréstimo";
            else if (pedidoItemVM.IdItem == null)
                pedidoItemVM.OrigemVeiculo = "Veículo de Pesquisa";
            else
                pedidoItemVM.OrigemVeiculo = "Veículo de Catálogo";
            */
            if (request.PedidoItemVeiculo.PedidoItem.IdItem == null)
                request.PedidoItemVeiculo.IdOrigem = (int)OrigemVeiculo.VEICULOPESQUISA;
            else
            {
                var itemCatalogo = await itemCatalogoRepository.GetById((long)request.PedidoItemVeiculo.PedidoItem.IdItem, cancellationToken);
                if (itemCatalogo == null) throw new NotFoundException("Registro do item de catálogo não encontrado.");

                if (itemCatalogo.IdConteudo != request.PedidoItemVeiculo.PedidoItem.Pedido.IdConteudo)
                    request.PedidoItemVeiculo.IdOrigem = (int)OrigemVeiculo.VEICULOEMPRESTIMO;
                else
                    request.PedidoItemVeiculo.IdOrigem = (int)OrigemVeiculo.VEICULOCATALOGO;

                var item = await itemRepository.GetById((long)request.PedidoItemVeiculo.PedidoItem.IdItem, cancellationToken);
                if (item == null) throw new NotFoundException("Registro do item não encontrado.");
                request.PedidoItemVeiculo.PedidoItem.Item = item;
            }

            request.PedidoItemVeiculo.Status = statusVeiculo;

            var tipoVeiculo = await categoriaRepository.GetById(request.PedidoItemVeiculo.IdTipo, cancellationToken);

            if (tipoVeiculo == null) throw new NotFoundException("Registro do tipo de veículo não encontrado.");

            request.PedidoItemVeiculo.Tipo = tipoVeiculo;

            var subCategoriaVeiculo = await categoriaRepository.GetById(request.PedidoItemVeiculo.IdSubCategoria, cancellationToken);

            if (subCategoriaVeiculo == null) throw new NotFoundException("Registro do tipo de veículo não encontrado.");

            request.PedidoItemVeiculo.SubCategoria = subCategoriaVeiculo;

            pedidoItemVeiculoRepository.AddOrUpdate(request.PedidoItemVeiculo, cancellationToken);

            var result = unitOfWork.SaveChanges();

            if (!result) throw new ApplicationException("An error has occured.");

            //mediator.Publish();

            return await Unit.Task;
        }

        /// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		async Task<Unit> IRequestHandler<UpdatePedidoItemVeiculo, Unit>.Handle(UpdatePedidoItemVeiculo request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.PedidoItemVeiculo.PedidoItem == null)
                    throw new ApplicationException("O Pedido Item está vazio!");

                if (request.PedidoItemVeiculo.IdTipo == 0)
                    throw new ApplicationException("O tipo de veículo é obrigatório!");

                if (request.PedidoItemVeiculo.IdSubCategoria == 0)
                    throw new ApplicationException("A subCategoria do item é obrigatório!");

                if (request.PedidoItemVeiculo.Modelo == "")
                    throw new ApplicationException("O Modelo do item é obrigatório!");

                if (request.PedidoItemVeiculo.NroOpcoes == 0)
                    throw new ApplicationException("O Nro de opções do item é obrigatório!");

                var existPedido = await pedidoRepository.GetById(request.PedidoItemVeiculo.PedidoItem.IdPedido, cancellationToken);

                if (existPedido == null)
                    throw new NotFoundException("Pedido não encontrado");

                var existPedidoItem = await pedidoItemRepository.GetById(request.PedidoItemVeiculo.PedidoItem.Id, cancellationToken);

                if (existPedido == null)
                    throw new NotFoundException("Pedido item não encontrado");

                var existPedidoItemVeiculo = await pedidoItemVeiculoRepository.GetByIdPedidoItemVeiculo(existPedidoItem.Id, cancellationToken);
                //var existPedidoVeiculo = pedidoItemVeiculoRepository.GetAll().Where(c => c.IdPedidoItem == existPedidoItem.Id).ToList();

                if (existPedidoItemVeiculo == null)
                    throw new NotFoundException("Pedido item não encontrado");

                if (request.PedidoItemVeiculo.DataChegadaVeiculo.HasValue)
                    existPedidoItemVeiculo.DataChegadaVeiculo = request.PedidoItemVeiculo.DataChegadaVeiculo;

                if (request.PedidoItemVeiculo.DataDevolucaoVeiculo.HasValue)
                    existPedidoItemVeiculo.DataDevolucaoVeiculo = request.PedidoItemVeiculo.DataDevolucaoVeiculo;

                existPedidoItemVeiculo.PersonagemUtilizacao = request.PedidoItemVeiculo.PersonagemUtilizacao;
                existPedidoItemVeiculo.Modelo = request.PedidoItemVeiculo.Modelo;
                existPedidoItemVeiculo.IdTipo = request.PedidoItemVeiculo.IdTipo;
                existPedidoItemVeiculo.NroOpcoes = request.PedidoItemVeiculo.NroOpcoes;
                existPedidoItemVeiculo.Ano = request.PedidoItemVeiculo.Ano;
                existPedidoItemVeiculo.Cor = request.PedidoItemVeiculo.Cor;
                existPedidoItemVeiculo.Continuidade = request.PedidoItemVeiculo.Continuidade;
                existPedidoItemVeiculo.CenaAcao = request.PedidoItemVeiculo.CenaAcao;
                existPedidoItemVeiculo.SobreCenaAcao = request.PedidoItemVeiculo.SobreCenaAcao;

                var tipoVeiculo = await categoriaRepository.GetById(request.PedidoItemVeiculo.IdTipo, cancellationToken);

                if (tipoVeiculo == null) throw new NotFoundException("Registro do tipo de veículo não encontrado.");

                existPedidoItemVeiculo.Tipo = tipoVeiculo;

                var subCategoriaVeiculo = await categoriaRepository.GetById(request.PedidoItemVeiculo.IdSubCategoria, cancellationToken);

                if (subCategoriaVeiculo == null) throw new NotFoundException("Registro do tipo de veículo não encontrado.");

                existPedidoItemVeiculo.SubCategoria = subCategoriaVeiculo;

                existPedidoItemVeiculo.PedidoItem.Arquivos = request.PedidoItemVeiculo.PedidoItem.Arquivos;

                pedidoItemVeiculoRepository.AddOrUpdate(existPedidoItemVeiculo, cancellationToken);

                var result = unitOfWork.SaveChanges();

                if (!result) throw new ApplicationException("An error has occured.");                

                return await Unit.Task;
            }
            catch (Exception error)
            {
                throw new ApplicationException(error.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        async Task<Unit> IRequestHandler<MudarStatusPedidoItemVeiculo, Unit>.Handle(MudarStatusPedidoItemVeiculo request, CancellationToken cancellationToken)
        {
            bool statusModificado = false;

            if (request.IdPedidoItem <= 0)
                throw new ValidationException("O ID Pedido Item não pode ser nulo.");

            if (request.IdStatus <= 0)
                throw new ValidationException("O ID Status não pode ser nulo.");

            var existPedidoItem = await pedidoItemRepository.GetById(request.IdPedidoItem, cancellationToken);

            if (existPedidoItem == null)
                throw new NotFoundException("Pedido não encontrado");

            //var existPedidoItemVeiculo = await pedidoItemVeiculoRepository.GetById(existPedidoItem.PedidoItemVeiculo.Id, cancellationToken);
            var existPedidoItemVeiculo = await pedidoItemVeiculoRepository.GetByIdPedidoItemVeiculo(existPedidoItem.Id, cancellationToken);

            if (existPedidoItemVeiculo == null)
                throw new NotFoundException("Pedido não encontrado");

            existPedidoItem.PedidoItemVeiculo = existPedidoItemVeiculo;

            var existStatus = await statusItemRepository.GetById(request.IdStatus, cancellationToken);

            if (existStatus == null)
                throw new NotFoundException("Status não encontrado");

            if (existPedidoItemVeiculo.IdStatus != request.IdStatus)
                statusModificado = true;

            if (statusModificado)
            {
                bool registraTracking = false;

                switch (existPedidoItemVeiculo.IdStatus)
                {
                    case (int)PedidoItemVeiculoStatus.PEDIDOITEM_RASCUNHO:
                        {
                            registraTracking = false;

                            if ((request.IdStatus != (int)PedidoItemVeiculoStatus.PEDIDOITEM_ENVIADO))
                                throw new ApplicationException("Mudança do status do pedido não permitido para o status atual");

                            break;
                        }
                    case (int)PedidoItemVeiculoStatus.PEDIDOITEM_ENVIADO:
                        {
                            registraTracking = true;

                            if ((request.IdStatus != (int)PedidoItemVeiculoStatus.PEDIDOITEM_EMANALISE))
                                throw new ApplicationException("Mudança do status do pedido não permitido para o status atual");

                            break;
                        }
                    case (int)PedidoItemVeiculoStatus.PEDIDOITEM_EMANALISE:
                        {
                            registraTracking = true;

                            if ((request.IdStatus != (int)PedidoItemVeiculoStatus.PEDIDOITEM_OPCAOAPROVADA) &&
                                (request.IdStatus != (int)PedidoItemVeiculoStatus.PEDIDOITEM_SOLICITACAOEMPRESTIMOEXPIRADA) &&
                                (request.IdStatus != (int)PedidoItemVeiculoStatus.PEDIDOITEM_DEVOLUCAO))
                                throw new ApplicationException("Mudança do status do pedido não permitido para o status atual");

                            break;
                        }
                    case (int)PedidoItemVeiculoStatus.PEDIDOITEM_OPCAOAPROVADA:
                        {
                            registraTracking = true;

                            if ((request.IdStatus != (int)PedidoItemVeiculoStatus.PEDIDOITEM_ACIONAMENTOSOLICITADO))
                                throw new ApplicationException("Mudança do status do pedido não permitido para o status atual");

                            break;
                        }
                    case (int)PedidoItemVeiculoStatus.PEDIDOITEM_ACIONAMENTOSOLICITADO:
                        {
                            registraTracking = true;

                            if ((request.IdStatus != (int)PedidoItemVeiculoStatus.PEDIDOITEM_ACIONADO))
                                throw new ApplicationException("Mudança do status do pedido não permitido para o status atual");

                            break;
                        }
                    case (int)PedidoItemVeiculoStatus.PEDIDOITEM_ACIONADO:
                        {
                            registraTracking = true;

                            if ((request.IdStatus == (int)PedidoItemVeiculoStatus.PEDIDOITEM_RCAPROVADA))
                                throw new ApplicationException("Mudança do status do pedido não permitido para o status atual");

                            break;
                        }
                    default:
                        break;
                }


                var existPedido = pedidoRepository.GetById(existPedidoItem.IdPedido, cancellationToken).GetAwaiter().GetResult();
                if (existPedido == null)
                    throw new NotFoundException("Pedido não encontrado");

                var existUser = await userRepository.GetByLogin(userProvider.User.Login, cancellationToken);
                if (existUser == null)
                    throw new NotFoundException("Usuário não encontrado");

                switch (request.IdStatus)
                {
                    case (int)PedidoItemVeiculoStatus.PEDIDOITEM_CANCELADO:
                        {
                            registraTracking = true;

                            if (string.IsNullOrEmpty(request.JustificativaCancelamento))
                                throw new ApplicationException("Justificativa para cancelamento é obrigatória");

                            existPedidoItemVeiculo.PedidoItem.JustificativaCancelamento = request.JustificativaCancelamento;
                            existPedidoItemVeiculo.PedidoItem.DataCancelamento = DateTime.Now;
                            existPedidoItemVeiculo.PedidoItem.CanceladoPor = existUser;
                            existPedidoItemVeiculo.PedidoItem.CanceladoPorLogin = existUser.Login;

                            break;
                        }
                    case (int)PedidoItemVeiculoStatus.PEDIDOITEM_DEVOLUCAO:
                        {
                            registraTracking = true;

                            if (string.IsNullOrEmpty(request.JustificativaDevolucao))
                                throw new ApplicationException("Justificativa para devolução é obrigatória");

                            existPedidoItemVeiculo.PedidoItem.JustificativaDevolucao = request.JustificativaDevolucao;
                            existPedidoItemVeiculo.PedidoItem.DataDevolucao = DateTime.Now;
                            existPedidoItemVeiculo.PedidoItem.DevolvidoPor = existUser;
                            existPedidoItemVeiculo.PedidoItem.DevolvidoPorLogin = existUser.Login;

                            break;
                        }
                    default:
                        break;
                }

                existPedidoItemVeiculo.IdStatus = request.IdStatus;
                existPedidoItemVeiculo.Status = existStatus;
                existPedidoItemVeiculo.PedidoItem = existPedidoItem;
                existPedidoItemVeiculo.PedidoItem.Pedido = existPedido;

                //todo: remover persistência do EventHandle, persistir nos CommandHandlers
                pedidoItemVeiculoRepository.AddOrUpdate(existPedidoItemVeiculo, cancellationToken);

                var result = unitOfWork.SaveChanges();
                if (!result) throw new ApplicationException("An error has occured.");

                if (registraTracking)
                {
                    await mediator.Send(new AddTrackingVeiculo()
                    {
                        PedidoItemVeiculoTracking = new PedidoItemVeiculoTracking()
                        {
                            IdPedidoItem = existPedidoItemVeiculo.Id,
                            ChangedBy = existUser,
                            Status = existStatus,
                            PedidoItemVeiculo = existPedidoItemVeiculo,
                            StatusId = existPedidoItemVeiculo.IdStatus,
                            ChangeById = existPedido.CriadoPorLogin
                        }
                    }, cancellationToken);
                }
                /*
                await mediator.Publish(new VerificarStatusVeiculos()
                {
                    PedidoItem = existPedidoItemVeiculo
                });
                */
            }
            else
                throw new ValidationException("O status informado é o mesmo já registrado no sistema.");

            return await Unit.Task;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        async Task<Unit> IRequestHandler<DeletePedidoItemVeiculo, Unit>.Handle(DeletePedidoItemVeiculo request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
                throw new ValidationException("O ID Pedido não pode ser nulo.");

            if (request.idItem <= 0)
                throw new ValidationException("O ID Pedido Item não pode ser nulo.");

            var existPedidoItem = await pedidoItemRepository.GetById(request.idItem, cancellationToken);

            if (existPedidoItem == null)
                throw new NotFoundException("Pedido Item não encontrado");


            var existPedidoItemVeiculo = await pedidoItemVeiculoRepository.GetByIdPedidoItemVeiculo(request.idItem, cancellationToken);

            if (existPedidoItemVeiculo != null)
            {
                if (existPedidoItemVeiculo.IdStatus != (int)PedidoItemVeiculoStatus.PEDIDOITEM_RASCUNHO)
                    throw new ApplicationException(string.Format("Pedido item {0} possui o status diferente de rascunho", request.idItem));
            }

            pedidoItemRepository.DeletarPedidoItem(request.idItem, cancellationToken);

            var result = unitOfWork.SaveChanges();

            if (!result) throw new ApplicationException("An error has occured.");

            return await Unit.Task;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        async Task<Unit> IRequestHandler<DeletePedidoItensVeiculo, Unit>.Handle(DeletePedidoItensVeiculo request, CancellationToken cancellationToken)
        {

            if (request.id <= 0)
                throw new ValidationException("O ID Pedido não pode ser nulo.");

            var existPedidoItem = pedidoItemRepository.GetAll().Where(a => a.IdPedido == request.id).ToList();

            if (existPedidoItem.Count <= 0)
                throw new NotFoundException("Pedido item não encontrado");

            foreach (var pedidoItem in existPedidoItem)
            {
                var existPedidoItemVeiculo = await pedidoItemVeiculoRepository.GetByIdPedidoItemVeiculo(pedidoItem.Id, cancellationToken);

                if (existPedidoItemVeiculo != null)
                {
                    if (existPedidoItemVeiculo.IdStatus != (int)PedidoItemVeiculoStatus.PEDIDOITEM_RASCUNHO)
                        throw new ApplicationException(string.Format("Pedido item {0} possui o status diferente de rascunho", pedidoItem.Id));

                    pedidoItemRepository.DeletarPedidoItem(pedidoItem.Id, cancellationToken);

                    var result = unitOfWork.SaveChanges();

                    if (!result) throw new ApplicationException("An error has occured.");
                }
            }

            return await Unit.Task;
        }


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
