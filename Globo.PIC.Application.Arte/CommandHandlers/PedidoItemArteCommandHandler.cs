using System;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Interfaces;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Globo.PIC.Domain.Enums;
using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.Types.Events;
using Globo.PIC.Domain.Types.Queries;
using Newtonsoft.Json;
using Globo.PIC.Domain.Exceptions;
using Globo.PIC.Domain.Models;
using Globo.PIC.Infra.Data.Repositories;
using Globo.PIC.Domain.Extensions;

namespace Globo.PIC.Application.Services.Commands
{

    /// <summary>
    /// 
    /// </summary>
    public class PedidoItemArteCommandHandler :

        IRequestHandler<DeletePedidoItemArte>,
        IRequestHandler<DeleteAllPedidoItemArte>,
        IRequestHandler<UpdatePedidoItemArte>,
        IRequestHandler<SendRC>,
        IRequestHandler<MudarStatusPedidoItemArte>,
        IRequestHandler<MudarCompradorPedidoItemArte>,
        IRequestHandler<CreatePedidoItemArte>,
        //IRequestHandler<CreatePedidoItensArte>
        IRequestHandler<UpdatePedidoItemArteDevolucao>
    //IRequestHandler<UpdatePedidoItensDevolucao>
    {

        /// <summary>
        /// 
        /// </summary>
        private readonly PedidoItemRepository pedidoItemRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly PedidoItemArteRepository pedidoItemArteRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<PedidoItemArteDevolucao> pedidoItemDevolucaoRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<PedidoItemArteAtribuicao> pedidoItemAtribuicaoRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<PedidoItemArteCompra> pedidoItemCompraRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<StatusPedidoItemArte> statusItemRepository;
        /// <summary>
        /// 
        /// </summary>
        private readonly PedidoRepository pedidoRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<PedidoItemAnexo> pedidoItemAnexosRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<PedidoItemArteTracking> trackingRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<RC> rcRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<PedidoItemAnexo> arquivosRepository;

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
        private readonly ILineProxy line;

        /// <summary>
        /// 
        /// </summary>
        private readonly IPurchaseRequisitionProxy purchaseRequisitionProxy;

        private readonly IExpenditureProxy expenditureProxy;
        /// <summary>
        /// 
        /// </summary>  
        public PedidoItemArteCommandHandler(
            IUnitOfWork _unitOfWork,
            IMediator _mediator,
            IRepository<PedidoItem> _pedidoItemRepository,
            IRepository<PedidoItemArteDevolucao> _pedidoItemDevolucaoRepository,
            IRepository<PedidoItemArteAtribuicao> _pedidoItemAtribuicaoRepository,
            IRepository<PedidoItemArteCompra> _pedidoItemCompraRepository,
            IRepository<StatusPedidoItemArte> _statusItemRepository,
            IRepository<PedidoItemArte> _pedidoItemArteRepository,
            IRepository<Pedido> _pedidoRepository,
            IRepository<PedidoItemAnexo> _pedidoItemAnexosRepository,
            IRepository<PedidoItemArteTracking> _trackingRepository,
            IRepository<RC> _rcRepository,
            IRepository<PedidoItemAnexo> _arquivosRepository,
            IRepository<Usuario> _userRepository,
            IUserProvider _userProvider,
            ILineProxy _line,
            IPurchaseRequisitionProxy _purchaseRequisitionProxy, IExpenditureProxy _expenditureProxy)
        {
            unitOfWork = _unitOfWork;
            mediator = _mediator;
            pedidoItemRepository = _pedidoItemRepository as PedidoItemRepository;
            pedidoItemArteRepository = _pedidoItemArteRepository as PedidoItemArteRepository;
            pedidoRepository = _pedidoRepository as PedidoRepository;
            pedidoItemDevolucaoRepository = _pedidoItemDevolucaoRepository;
            pedidoItemAtribuicaoRepository = _pedidoItemAtribuicaoRepository;
            pedidoItemCompraRepository = _pedidoItemCompraRepository;
            statusItemRepository = _statusItemRepository;
            pedidoItemAnexosRepository = _pedidoItemAnexosRepository;
            trackingRepository = _trackingRepository;
            rcRepository = _rcRepository;
            arquivosRepository = _arquivosRepository;
            userRepository = _userRepository;
            userProvider = _userProvider;
            line = _line;
            expenditureProxy = _expenditureProxy;
            purchaseRequisitionProxy = _purchaseRequisitionProxy;
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

            if (!pedidoItem.RCs.FirstOrDefault().ItemId.HasValue ||
                pedidoItem.RCs.FirstOrDefault().ItemId <= 0 ||
                string.IsNullOrWhiteSpace(pedidoItem.RCs.FirstOrDefault().ItemCodigo))
                throw new ApplicationException("A RC precisa possuir um item válido!");
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
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task<Unit> IRequestHandler<UpdatePedidoItemArte, Unit>.Handle(UpdatePedidoItemArte request, CancellationToken cancellationToken)
        {
            try
            {
                bool statusChange = false;
                RunEntityValidation(request.PedidoItemArte.PedidoItem);

                var existPedido = await pedidoRepository.GetByIdPedidoWithOutRoles(request.PedidoItemArte.PedidoItem.IdPedido, cancellationToken);

                if (existPedido == null)
                    throw new BadRequestException("id pedido não encontrado!");

                var existPedidoItem = existPedido.Itens.Where(x => x.Id == request.PedidoItemArte.IdPedidoItem).FirstOrDefault();

                if (existPedidoItem == null)
                    throw new BadRequestException("id pedido item não encontrado!");

                if (request.PedidoItemArte.PedidoItem.DataNecessidade == null)
                {
                    if (existPedidoItem.DataNecessidade != null)
                        request.PedidoItemArte.PedidoItem.DataNecessidade = existPedidoItem.DataNecessidade;
                    else
                        if (existPedido.PedidoArte.DataNecessidade != null)
                        request.PedidoItemArte.PedidoItem.DataNecessidade = existPedido.PedidoArte.DataNecessidade;
                }

                var vltotal = request.PedidoItemArte.PedidoItem.Valor.HasValue ?
                    request.PedidoItemArte.PedidoItem.Quantidade * request.PedidoItemArte.PedidoItem.Valor.Value : 0;

                request.PedidoItemArte.PedidoItem.ValorItens = vltotal;

                await mediator.Send(new UpdatePedidoItem()
                {
                    PedidoItem = request.PedidoItemArte.PedidoItem
                }, cancellationToken);

                existPedidoItem = await pedidoItemRepository.GetById(request.PedidoItemArte.PedidoItem.Id, cancellationToken);

                request.PedidoItemArte.Id = existPedidoItem.PedidoItemArte.Id;
                request.PedidoItemArte.PedidoItem = existPedidoItem;
                request.PedidoItemArte.IdStatus = existPedidoItem.PedidoItemArte.IdStatus;
                request.PedidoItemArte.Status = existPedidoItem.PedidoItemArte.Status;

                var pedidoItemCompra = pedidoItemCompraRepository.GetAll().Where(a => a.IdPedidoItemArte == request.PedidoItemArte.Id).ToList();

                //Todo: Adequar nome do IdTipo no Item para IdTag como no Pedido
                if (pedidoItemCompra.Count > 0 && existPedidoItem.PedidoItemArte.IdTipo == (int)TagPedido.EXTERNA)
                    throw new NotFoundException(string.Format("Item {0} não pode ser editado, já existe compra vinculada.", request.PedidoItemArte.IdPedidoItem));

                if (request.PedidoItemArte.IdStatus == 0)
                {
                    if (existPedidoItem.PedidoItemArte.IdStatus == 0)
                        if (string.IsNullOrWhiteSpace(existPedidoItem.RCs.FirstOrDefault().Acordo))
                        {
                            request.PedidoItemArte.IdStatus = (int)PedidoItemArteStatus.ITEM_SOLICITACAODECOMPRACRIADA;
                            statusChange = true;
                        }
                        else
                        {
                            request.PedidoItemArte.IdStatus = (int)PedidoItemArteStatus.ITEM_SOLICITACAODECOMPRAENVIADA;
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
                    request.PedidoItemArte.Comprador = existPedidoItem.PedidoItemArte.Comprador;
                }
                else if (userProvider.IsRole(Role.PERFIL_BASE_SUPRIMENTOS) && !string.IsNullOrWhiteSpace(request.PedidoItemArte.CompradoPorLogin))
                {
                    request.PedidoItemArte.IdStatus = (int)PedidoItemArteStatus.ITEM_ATRIBUIDOAOCOMPRADOR;
                    statusChange = true;
                    request.PedidoItemArte.Comprador = await userRepository.GetByLogin(request.PedidoItemArte.CompradoPorLogin, cancellationToken);
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

                    request.PedidoItemArte.Comprador = await userRepository.GetByLogin(request.PedidoItemArte.CompradoPorLogin, cancellationToken);
                }

                if (!string.IsNullOrWhiteSpace(request.PedidoItemArte.PedidoItem.ReprovadoPorLogin))
                    request.PedidoItemArte.PedidoItem.ReprovadoPor = await userRepository.GetByLogin(request.PedidoItemArte.PedidoItem.ReprovadoPorLogin, cancellationToken);

                request.PedidoItemArte.TrackingArte = trackingRepository.GetAll().Where(a => a.IdPedidoItemArte == request.PedidoItemArte.Id && a.Ativo == true).ToList();

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

        async Task<Unit> IRequestHandler<CreatePedidoItemArte, Unit>.Handle(CreatePedidoItemArte request, CancellationToken cancellationToken)
        {

            RunEntityValidation(request.PedidoItemArte.PedidoItem);

            bool statusChange = false;

            var pedido = await pedidoRepository.GetByIdPedidoWithOutRoles(request.PedidoItemArte.PedidoItem.IdPedido, cancellationToken);
            if (pedido == null)
                throw new NotFoundException("id pedido não encontrado!");
            else
                request.PedidoItemArte.PedidoItem.Pedido = pedido;

            /*
             * 
             * Bloco para verificação de pedido item e caso existe aplica update senão insert
             * Se o status não foi passado como parametro atribui o que está na base de dados caso o item existe, caso contrário inserir o status inicial de acordo com o acordo
             *
             */
            if (request.PedidoItemArte.PedidoItem.Id > 0)
            {
                var existPedidoItem = await pedidoItemRepository.GetById(request.PedidoItemArte.PedidoItem.Id, cancellationToken);

                if (existPedidoItem == null)
                    throw new NotFoundException("id pedido item não encontrado!");

                if (request.PedidoItemArte.IdStatus == 0)
                {
                    if (existPedidoItem.PedidoItemArte.IdStatus > 0)
                        request.PedidoItemArte.IdStatus = existPedidoItem.PedidoItemArte.IdStatus;
                    else
                    {
                        if (string.IsNullOrWhiteSpace(existPedidoItem.PedidoItemArte.PedidoItem.RCs.FirstOrDefault().Acordo))
                        {
                            request.PedidoItemArte.IdStatus = (int)Domain.Enums.PedidoItemArteStatus.ITEM_SOLICITACAODECOMPRACRIADA;
                            statusChange = true;

                            if (!string.IsNullOrWhiteSpace(pedido.PedidoArte.BaseLogin))
                                request.PedidoItemArte.IdStatus = (int)Domain.Enums.PedidoItemArteStatus.ITEM_SOLICITACAODECOMPRACRIADA;

                            if (string.IsNullOrWhiteSpace(existPedidoItem.PedidoItemArte.CompradoPorLogin))
                            {
                                request.PedidoItemArte.IdStatus = (int)Domain.Enums.PedidoItemArteStatus.ITEM_ATRIBUIDOAOCOMPRADOR;
                            }
                        }
                        else
                        {
                            request.PedidoItemArte.IdStatus = (int)Domain.Enums.PedidoItemArteStatus.ITEM_SOLICITACAODECOMPRAENVIADA;
                            statusChange = true;
                        }
                    }
                }
                else
                    statusChange = true;
            }
            else
            {
                if (request.PedidoItemArte.PedidoItem.Quantidade == 0)
                    throw new ApplicationException("A quantidade do Item está zerado!");

                //if (request.PedidoItem.Valor == 0)
                //	throw new ApplicationException("O valor do Item está zerado!");

                //BUG 60948: adicionar o local de entrega do pedido no item.
                request.PedidoItemArte.PedidoItem.LocalEntrega = pedido.LocalEntrega;

                request.PedidoItemArte.QuantidadePendenteCompra = request.PedidoItemArte.PedidoItem.Quantidade;

                if (request.PedidoItemArte.IdStatus == 0)
                {
                    if (string.IsNullOrWhiteSpace(request.PedidoItemArte.PedidoItem.RCs.FirstOrDefault().Acordo))
                    {
                        request.PedidoItemArte.IdStatus = (int)PedidoItemArteStatus.ITEM_SOLICITACAODECOMPRACRIADA;
                        statusChange = true;

                        if (!string.IsNullOrWhiteSpace(pedido.PedidoArte.BaseLogin))
                        {
                            request.PedidoItemArte.IdStatus = (int)PedidoItemArteStatus.ITEM_SOLICITACAODECOMPRACRIADA;
                        }
                    }
                    else
                    {
                        request.PedidoItemArte.IdStatus = (int)PedidoItemArteStatus.ITEM_SOLICITACAODECOMPRAENVIADA;
                        statusChange = true;
                    }
                }
                else
                    statusChange = true;
            }


            if (!string.IsNullOrWhiteSpace(request.PedidoItemArte.CompradoPorLogin))
            {
                if (request.PedidoItemArte.IdTipo == 0)
                    throw new NotFoundException("O Tipo do pedido não pode ser nulo quando o comprador é atribuído");

                if (request.PedidoItemArte.PedidoItem.Id > 0)
                {
                    var existPedidoItem = await pedidoItemRepository.GetById(request.PedidoItemArte.PedidoItem.Id, cancellationToken);

                    if (existPedidoItem == null)
                        throw new NotFoundException("id pedido item não encontrado!");

                    if (request.PedidoItemArte.CompradoPorLogin != existPedidoItem.PedidoItemArte.CompradoPorLogin)
                    {
                        request.PedidoItemArte.IdStatus = (int)PedidoItemArteStatus.ITEM_ATRIBUIDOAOCOMPRADOR;

                        statusChange = true;
                    }

                    if (!DBNull.Value.Equals(request.PedidoItemArte.DataVinculoComprador) || (request.PedidoItemArte.DataVinculoComprador != existPedidoItem.PedidoItemArte.DataVinculoComprador))
                        request.PedidoItemArte.DataVinculoComprador = DateTime.Now;
                }
                else
                {
                    request.PedidoItemArte.IdStatus = (int)PedidoItemArteStatus.ITEM_ATRIBUIDOAOCOMPRADOR;

                    statusChange = true;

                    if (!DBNull.Value.Equals(request.PedidoItemArte.DataVinculoComprador))
                        request.PedidoItemArte.DataVinculoComprador = DateTime.Now;
                }

                request.PedidoItemArte.Comprador = await userRepository.GetByLogin(request.PedidoItemArte.CompradoPorLogin, cancellationToken);

                await mediator.Publish(new OnItemAtribuidoComprador() { Pedido = pedido });
            }


            if (request.PedidoItemArte.PedidoItem.DataNecessidade == null)
            {
                if (pedido.PedidoArte.DataNecessidade != null)
                    request.PedidoItemArte.PedidoItem.DataNecessidade = pedido.PedidoArte.DataNecessidade;
            }

            var status = await statusItemRepository.GetById(request.PedidoItemArte.IdStatus, cancellationToken);

            if (status == null)
                throw new NotFoundException("status não encontrado!");
            else
                request.PedidoItemArte.Status = status;

            var vltotal = request.PedidoItemArte.PedidoItem.Valor.HasValue ?
                request.PedidoItemArte.PedidoItem.Quantidade * request.PedidoItemArte.PedidoItem.Valor.Value : 0;

            request.PedidoItemArte.PedidoItem.ValorItens = vltotal;

            if (string.IsNullOrWhiteSpace(request.PedidoItemArte.PedidoItem.Numero))
            {
                int nextNum = pedidoRepository.GetProximoNumItem(request.PedidoItemArte.PedidoItem.IdPedido);
                request.PedidoItemArte.PedidoItem.Numero = nextNum.ToString();
            }

            pedidoItemArteRepository.AddOrUpdate(request.PedidoItemArte, cancellationToken);

            var result = unitOfWork.SaveChanges();

            if (!result) throw new ApplicationException("An error has occured.");

            if (request.PedidoItemArte.PedidoItem.Arquivos.Count() > 0)
            {
                foreach (var anexos in request.PedidoItemArte.PedidoItem.Arquivos)
                {
                    anexos.IdPedidoItem = request.PedidoItemArte.PedidoItem.Id;
                    anexos.PedidoItem = request.PedidoItemArte.PedidoItem;
                    pedidoItemAnexosRepository.AddOrUpdate(anexos, cancellationToken);
                    result = unitOfWork.SaveChanges();
                }
            }

            if (statusChange)
            {

                if (request.PedidoItemArte.IdStatus == (int)PedidoItemArteStatus.ITEM_SOLICITACAODECOMPRACRIADA || request.PedidoItemArte.IdStatus == (int)PedidoItemArteStatus.ITEM_ATRIBUIDOAOCOMPRADOR)
                {
                    var existTracking = trackingRepository.GetAll().Where(i => i.IdPedidoItemArte == request.PedidoItemArte.Id &&
                    i.StatusId == (int)PedidoItemArteStatus.ITEM_SOLICITACAODECOMPRACRIADA).ToList();

                    if (existTracking.Count == 0)
                    {
                        await mediator.Send(new AddTrackingArte()
                        {
                            PedidoItemArteTracking = new PedidoItemArteTracking()
                            {
                                IdPedidoItemArte = request.PedidoItemArte.Id,
                                StatusId = (int)PedidoItemArteStatus.ITEM_SOLICITACAODECOMPRACRIADA,
                                ChangeById = request.PedidoItemArte.PedidoItem.Pedido.CriadoPorLogin
                            }
                        }, cancellationToken);
                    }
                }

                await mediator.Send(new AddTrackingArte()
                {
                    PedidoItemArteTracking = new PedidoItemArteTracking()
                    {
                        IdPedidoItemArte = request.PedidoItemArte.Id,
                        StatusId = request.PedidoItemArte.IdStatus,
                        ChangeById = request.PedidoItemArte.PedidoItem.Pedido.CriadoPorLogin
                    }
                }, cancellationToken);
            }

            await mediator.Publish(new OnPedidoItemArteCriado() { PedidoItemArte = request.PedidoItemArte });

            return Unit.Value;
        }

        async Task<Unit> IRequestHandler<MudarCompradorPedidoItemArte, Unit>.Handle(MudarCompradorPedidoItemArte request, CancellationToken cancellationToken)
        {

            var pedido = await mediator.Send(new GetByIdPedidoWithOutRoles() { Id = request.IdPedido }, cancellationToken);

            if (pedido == null)
                throw new NotFoundException("Registro do pedido não encontrado.");

            var pedidoItem = await pedidoItemRepository.GetById(request.IdPedidoItem, cancellationToken);

            if (pedidoItem == null)
                throw new NotFoundException("Registro do pedido item não encontrado.");

            if (request.IdTipo == 0)
                throw new NotFoundException("O Tipo do pedido item não pode ser nulo quando o comprador é atribuído");

            if (request.Comprador == "")
                throw new ApplicationException("O comprador é obrigatório!");

            var usuarioComprador = await userRepository.GetByLogin(request.Comprador, cancellationToken);

            if (usuarioComprador == null)
                throw new NotFoundException("Registro do comprador não encontrado.");

            if (request.CompradorAnterior == "")
                throw new ApplicationException("O comprador anterior é obrigatório!");

            var usuarioCompradorAnterior = await userRepository.GetByLogin(request.CompradorAnterior, cancellationToken);

            if (usuarioCompradorAnterior == null)
                throw new NotFoundException("Registro do comprador anterior não encontrado.");

            if (request.Justificativa == "")
                throw new ApplicationException("A justificatifa é obrigatória!");


            PedidoItemArteAtribuicao pedidoItemAtribuicao = new PedidoItemArteAtribuicao()
            {
                Id = 0,
                IdPedidoItemArte = pedidoItem.PedidoItemArte.Id,
                IdTipo = request.IdTipo,
                Comprador = request.Comprador,
                CompradorAnterior = request.CompradorAnterior,
                DataAtribuicao = DateTime.UtcNow,
                Justificativa = request.Justificativa,
                PedidoItemArte = pedidoItem.PedidoItemArte,
                UsuarioComprador = usuarioComprador,
                UsuarioCompradorAnterior = usuarioCompradorAnterior
            };

            var atribuicoes = pedidoItemAtribuicaoRepository.GetAll().Where(
                                                                            a => a.IdPedidoItemArte == pedidoItem.PedidoItemArte.Id &&
                                                                            a.CompradorAnterior == request.Comprador
                                                                            ).ToList();

            if (atribuicoes.Count > 0)
                throw new NotFoundException(string.Format("Comprador {0} não pode ser atribuido ao item, já existe atribuição registrada.", request.Comprador));

            pedidoItemAtribuicaoRepository.AddOrUpdate(pedidoItemAtribuicao, cancellationToken);

            var result = unitOfWork.SaveChanges();

            if (!result) throw new ApplicationException("An error has occured.");

            return Unit.Value;
        }

        async Task<Unit> IRequestHandler<MudarStatusPedidoItemArte, Unit>.Handle(MudarStatusPedidoItemArte request, CancellationToken cancellationToken)
        {
                
            var status = await mediator.Send(new GetByStatusPedidoItemArteId() { Id = request.IdStatus }, cancellationToken);

            if (status == null)
                throw new NotFoundException("Registro do status não encontrado.");

            var pedido = await mediator.Send(new GetByIdPedidoWithOutRoles() { Id = request.IdPedido }, cancellationToken);

            if (pedido == null)
                throw new NotFoundException("Registro do pedido não encontrado.");

            var itens = pedidoItemArteRepository.GetAll().Where(x => request.IdPedidoItens.Contains(x.IdPedidoItem)).ToList();

            if (itens.Count() <= 0 || request.IdPedidoItens.Count() != itens.Count())
                throw new NotFoundException("Registro do pedido item não encontrado.");

            switch (request.IdStatus)
            {
                case (int)PedidoItemArteStatus.ITEM_CANCELAMENTOSOLICITADO:
                    {
                        if (!itens.Any(x => x.IdStatus != (int)PedidoItemArteStatus.ITEM_APROVADO ||
                            x.IdStatus != (int)PedidoItemArteStatus.ITEM_REPROVADO ||
                            x.IdStatus != (int)PedidoItemArteStatus.ITEM_CANCELADO))
                            throw new BadRequestException("Status atual do item não permite solicitação de cancelamento.");

                        if (!itens.Any(x => userProvider.User.Login == pedido.CriadoPorLogin || userProvider.User.Login == x.CompradoPorLogin))
                            throw new BadRequestException("Somente o solicitante do pedido pode efetuar a solicitação de cancelamento.");

                        if (pedido.PedidoArte.IdStatus != (int)PedidoArteStatus.PEDIDO_ENVIADO &&
                            pedido.PedidoArte.IdStatus != (int)PedidoArteStatus.PEDIDO_RASCUNHO &&
                            string.IsNullOrWhiteSpace(request.JustificativaCancelamento) &&
                            itens.Any(x => userProvider.User.Login != x.CompradoPorLogin))
                            throw new BadRequestException("Justificativa de cancelamento é obrigatória.");

                        foreach (var item in itens)
                        {
                            if (pedido.PedidoArte.IdStatus == (int)PedidoArteStatus.PEDIDO_ENVIADO)
                            {
                                if (item.IdStatus == (int)PedidoItemArteStatus.ITEM_SOLICITACAODECOMPRACRIADA)
                                    item.IdStatus = (int)PedidoItemArteStatus.ITEM_CANCELADO;
                                else
                                    item.IdStatus = (int)PedidoItemArteStatus.ITEM_CANCELAMENTOSOLICITADO;
                            }
                            else
                            {
                                if (pedido.PedidoArte.IdStatus == (int)PedidoArteStatus.PEDIDO_RASCUNHO)
                                    item.IdStatus = (int)PedidoItemArteStatus.ITEM_CANCELADO;
                                else
                                    item.IdStatus = (int)PedidoItemArteStatus.ITEM_CANCELAMENTOSOLICITADO;
                            }

                            status = await mediator.Send(new GetByStatusPedidoItemArteId() { Id = item.IdStatus }, cancellationToken);

                            if (status == null) throw new NotFoundException("Registro do status não encontrado.");

                            item.Status = status;

                            item.PedidoItem.JustificativaCancelamento = request.JustificativaCancelamento;

                            request.IdStatus = item.IdStatus;

                            await mediator.Send(new UpdatePedidoItemArte()
                            {
                                PedidoItemArte = item
                            }, cancellationToken);

                            if (item.IdStatus == (int)PedidoItemArteStatus.ITEM_CANCELAMENTOSOLICITADO)
                            {
                                await mediator.Publish(new OnCancelamentoItemSolicitado()
                                {
                                    PedidoItem = item.PedidoItem
                                });
                            }
                        }

                        break;
                    }
                case (int)PedidoItemArteStatus.ITEM_DEVOLUCAO:
                    {
                        if (!itens.Any(x => x.IdStatus == (int)PedidoItemArteStatus.ITEM_SOLICITACAODECOMPRACRIADA
                            || x.IdStatus == (int)PedidoItemArteStatus.ITEM_ATRIBUIDOAOCOMPRADOR))
                            throw new BadRequestException("Status atual do item não permite solicitação de devolução.");

                        if (userProvider.User.Login == pedido.PedidoArte.BaseLogin)
                        {
                            if (itens.Any(x => x.FlagDevolvidoBase))
                                throw new BadRequestException("Item já devolvido pela base.");

                            if (string.IsNullOrWhiteSpace(request.JustificativaDevolucao))
                                throw new BadRequestException("Justificativa de devolução é obrigatória.");

                            foreach (var item in itens)
                            {
                                item.FlagDevolvidoBase = true;

                                item.PedidoItem.JustificativaDevolucao = request.JustificativaDevolucao;

                                item.PedidoItem.DevolvidoPorLogin = userProvider.User.Login;

                                item.PedidoItem.DataDevolucao = DateTime.Now;

                                item.PedidoItem.PedidoItemArte.IdStatus = request.IdStatus;

                                item.PedidoItem.PedidoItemArte.Status = status;

                                request.IdStatus = item.IdStatus;

                                await mediator.Send(new UpdatePedidoItemArte()
                                {
                                    PedidoItemArte = item
                                }, cancellationToken);
                            }
                        }
                        else
                        {
                            if (!itens.Any(x => userProvider.User.Login == x.CompradoPorLogin))
                                throw new BadRequestException("Usuário não possui vinculo com o item para solicitar devolução.");

                            if (itens.Any(x => x.FlagDevolvidoComprador))
                                throw new BadRequestException("Item já devolvido pelo comprador.");

                            foreach (var item in itens)
                            {

                                if (string.IsNullOrWhiteSpace(request.JustificativaDevolucao))
                                    throw new BadRequestException("Justificativa de devolução é obrigatória.");

                                item.PedidoItem.PedidoItemArte.FlagDevolvidoComprador = true;

                                item.PedidoItem.JustificativaDevolucao = request.JustificativaDevolucao;

                                item.PedidoItem.DevolvidoPorLogin = userProvider.User.Login;

                                item.PedidoItem.DataDevolucao = DateTime.Now;

                                /*
                                Notificar o demanadante sobre a devolução
                                 */

                                request.IdStatus = item.IdStatus;

                                await mediator.Send(new UpdatePedidoItemArte()
                                {
                                    PedidoItemArte = item
                                }, cancellationToken);
                            }
                        }

                        break;
                    }
                case (int)PedidoItemArteStatus.ITEM_CANCELADO:
                    {
                        var login = userProvider.User.Login;

                        if (!itens.Any(x => x.Compras.Count() == 0))
                            throw new BadRequestException("Existem Compras associadas ao Item e não permitem cancelamento.");

                        if (login != pedido.PedidoArte.BaseLogin &&
                            itens.Any(x => login != x.CompradoPorLogin) &&
                            login != pedido.CriadoPorLogin)
                            throw new BadRequestException("Somente o usuário atribuído ao pedido poderá solicitar o cancelamento!");

                        foreach (var item in itens)
                        {
                            if (login == pedido.PedidoArte.BaseLogin)
                                item.PedidoItem.CanceladoPorLogin = pedido.PedidoArte.BaseLogin;

                            else if (login == item.CompradoPorLogin)
                                item.PedidoItem.CanceladoPorLogin = item.CompradoPorLogin;

                            else if (login == pedido.CriadoPorLogin)
                                item.PedidoItem.CanceladoPorLogin = pedido.CriadoPorLogin;

                            if (!string.IsNullOrWhiteSpace(item.PedidoItem.RCs.FirstOrDefault().Requisition))
                            {
                                var retorno = line.DeleteCancelLineAsync(item.PedidoItem.RCs.FirstOrDefault().HeadId, cancellationToken).Result;

                                if (!retorno)
                                    throw new BadRequestException("Não é possível cancelar o Item porque a RC foi aprovada no Oracle.");
                            }

                            item.PedidoItem.DataCancelamento = DateTime.Now;
                            item.IdStatus = request.IdStatus;
                            item.Status = status;

                            request.IdStatus = item.IdStatus;

                            await mediator.Send(new UpdatePedidoItemArte()
                            {
                                PedidoItemArte = item
                            }, cancellationToken);

                            await mediator.Publish(new OnCancelamentoItem()
                            {
                                PedidoItem = item.PedidoItem
                            });
                        }

                        break;
                    }
                case (int)PedidoItemArteStatus.ITEM_CANCELAMENTONEGADO:
                    {
                        foreach (var item in itens)
                        {
                            item.IdStatus = request.IdStatus;

                            item.Status = status;

                            request.IdStatus = item.IdStatus;

                            await mediator.Send(new UpdatePedidoItemArte()
                            {
                                PedidoItemArte = item
                            }, cancellationToken);

                            await mediator.Publish(new OnCancelamentoItemNegado()
                            {
                                PedidoItem = item.PedidoItem
                            });
                        }
                        break;
                    }
                case (int)PedidoItemArteStatus.ITEM_APROVADO:
                    {
                        //Caso este comando for acionado por um usuario do sistema
                        if (userProvider.User != null)
                        {
                            if (userProvider.User.UnidadeNegocio == null)
                                throw new BadRequestException("Enviar o pedido requer que o usuário tenha uma Unidade de Negócio previamente configurada.");

                            if (itens.Any(x => string.IsNullOrWhiteSpace(x.CompradoPorLogin)))
                                throw new BadRequestException("Para aprovar um item é necessário estar com permissão de comprador no item.");

                            if (itens.Any(x => userProvider.User.Login != x.CompradoPorLogin))
                                throw new BadRequestException("Somente o comprador vinculado poderá aprovar o item.");

                            if (itens.Any(x => x.PedidoItem.RCs.FirstOrDefault() == null))
                                throw new BadRequestException("Informações da Requisição de Compra não encontrada no item.");

                            if (itens.Any(x => !(string.IsNullOrWhiteSpace(x.PedidoItem.RCs.FirstOrDefault().Acordo) &&
                                             !x.PedidoItem.RCs.FirstOrDefault().AcordoId.HasValue &&
                                             !x.PedidoItem.RCs.FirstOrDefault().AcordoLinhaId.HasValue)))
                                throw new BadRequestException("Item de pedido com acordo não suportado por esta ação.");

                            await mediator.Send(new SendRC() { PedidoItens = itens.Select(x => x.PedidoItem).ToArray() }, cancellationToken);

                            foreach (var item in itens)
                            {
                                item.PedidoItem.AprovadoPorLogin = userProvider.User.Login;

                                item.PedidoItem.DataAprovacao = DateTime.Now;

                                item.PedidoItem.PedidoItemArte.IdStatus = (int)PedidoItemArteStatus.ITEM_SOLICITACAODECOMPRAENVIADA;

                                request.IdStatus = item.IdStatus;

                                await mediator.Send(new UpdatePedidoItemArte()
                                {
                                    PedidoItemArte = item
                                }, cancellationToken);
                            }
                            break;

                        }
                        //Caso este comando for acionado por um evento de notificação do Oracle
                        else
                        {
                            foreach (var item in itens)
                            {
                                item.PedidoItem.PedidoItemArte.IdStatus = (int)PedidoItemArteStatus.ITEM_APROVADO;

                                request.IdStatus = item.IdStatus;

                                await mediator.Send(new UpdatePedidoItemArte()
                                {
                                    PedidoItemArte = item
                                }, cancellationToken);

                                await mediator.Publish(new OnItemAprovado()
                                {
                                    PedidoItem = item.PedidoItem
                                });
                            }

                            break;
                        }
                    }
                default:
                    {
                        foreach (var item in itens)
                        {

                            item.IdStatus = request.IdStatus;

                            item.Status = status;

                            request.IdStatus = item.IdStatus;

                            return await mediator.Send(new UpdatePedidoItemArte()
                            {
                                PedidoItemArte = item
                            }, cancellationToken);
                        }

                        break;
                    }
            }

            return Unit.Value;
        }

        async Task<Unit> IRequestHandler<UpdatePedidoItemArteDevolucao, Unit>.Handle(UpdatePedidoItemArteDevolucao request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.PedidoItemDevolucao.Justificativa))
                    throw new BadRequestException("Justificativa da devolução é obrigatória!");

                if (request.PedidoItemDevolucao.idTipo == 0)
                    throw new BadRequestException("Somente pode ser devolvido para usuário demandante ou base!");

                var pedido = await pedidoRepository.GetByIdPedidoWithOutRoles(request.IdPedido, cancellationToken);

                if (pedido == null) throw new NotFoundException("Registro do pedido não encontrado.");

                var pedidoItem = await pedidoItemRepository.GetById(request.IdPedidoItem, cancellationToken);

                if (pedidoItem == null) throw new NotFoundException("Registro do pedido item não encontrado.");

                //var pedidoItemNew = await mediator.Send<PedidoItem>(new GetByIdPedidoItem() { Id = request.IdPedidoItem }, cancellationToken);

                var status = await statusItemRepository.GetById((int)PedidoItemArteStatus.ITEM_SOLICITACAODECOMPRACRIADA, cancellationToken);

                if (status == null) throw new BadRequestException("Registro do status não encontrado.");

                var comprador = pedidoItem.PedidoItemArte.CompradoPorLogin;

                if (pedidoItem.PedidoItemArte.QuantidadeDevolvida == 0)
                {
                    //var pedidoItemCompra = await mediator.Send<List<PedidoItemCompra>>(new ListItemCompraIdByIdPedidoItem() { IdPedido = id, IdPedidoItem = idItem }, cancellationToken);

                    if (pedidoItem.PedidoItemArte.QuantidadePendenteCompra != pedidoItem.Quantidade)
                    { //Item com compra
                      // Cria um novo pedido item para a quantidade devolvida.

                        if (request.PedidoItemDevolucao.idTipo == 1) throw new NotFoundException("Não é possível devolver o Item pois já tem compra.");

                        if (pedidoItem.PedidoItemArte.FlagDevolvidoComprador)
                            throw new BadRequestException("Item já foi devolvido pelo comprador.");

                        var arquivosPedidoItem = arquivosRepository.GetAll().Where(a => a.IdPedidoItem == pedidoItem.Id).ToList();

                        /*
                        if (arquivosPedidoItem.Count != 0)
                        {
                            foreach (var arquivos in arquivosPedidoItem)
                            {
                                arquivos.Id = 0;
                                arquivos.IdPedidoItem = 0;
                                arquivos.PedidoItem = null;
                            }
                        }
                        */

                        var rcsPedidoItem = rcRepository.GetAll().Where(a => a.IdPedidoItem == pedidoItem.Id).ToList();

                        /*
                        if (rcsPedidoItem.Count != 0)
                        {
                            foreach (var rc in rcsPedidoItem)
                            {
                                rc.Id = 0;
                                rc.IdPedidoItem = 0;
                                rc.PedidoItem = null;
                            }
                        }
                        */

                        var trackingsPedidoItem = trackingRepository.GetAll().Where(a => a.IdPedidoItemArte == pedidoItem.PedidoItemArte.Id &&
                                                                        a.StatusId <= 6).ToList();
                        /*
                        foreach (var trackings in trackingsPedidoItem)
                        {
                            if (trackings.StatusId <= 6)
                            {
                                trackings.Id = 0;
                                //trackings.ChangedBy = mediator.Send(new GetUsuarioLogin() { Login = trackings.ChangeById }, cancellationToken).Result;
                                //trackings.Status = mediator.Send<StatusPedidoItemArte>(new GetById() { Id = trackings.StatusId }, cancellationToken).Result;
                                trackings.PedidoItemArte = null;
                                trackings.IdPedidoItemArte = 0;
                            }
                        }
                        */
                        //TrackingArteRepository

                        var numeroItem = pedidoItem.Numero;
                        numeroItem += 0.01;

                        PedidoItem pedidoItemNew = new PedidoItem()
                        {
                            Id = 0,

                            /*PedidoItem*/
                            //pedidoItemNew.PedidoItem.Id = null;
                            IdPedido = pedidoItem.IdPedido,
                            Pedido = pedido,
                            IdPedidoItemPai = pedidoItem.IdPedidoItemPai,
                            IdItem = pedidoItem.IdItem,
                            Quantidade = pedidoItem.PedidoItemArte.QuantidadePendenteCompra,
                            Valor = pedidoItem.Valor.HasValue ? pedidoItem.Valor.Value : 0,
                            ValorItens = pedidoItem.Valor.HasValue ?
                            pedidoItem.PedidoItemArte.QuantidadePendenteCompra * pedidoItem.Valor.Value : 0,
                            ValorUnitario = pedidoItem.ValorUnitario,
                            DataNecessidade = pedidoItem.DataNecessidade,
                            LocalEntrega = pedidoItem.LocalEntrega,
                            DataEntrega = pedidoItem.DataEntrega,
                            NomeItem = pedidoItem.NomeItem,
                            Descricao = pedidoItem.Descricao,
                            UnidadeMedida = pedidoItem.UnidadeMedida,
                            Justificativa = pedidoItem.Justificativa,
                            JustificativaCancelamento = pedidoItem.JustificativaCancelamento,
                            JustificativaDevolucao = pedidoItem.JustificativaDevolucao,
                            CanceladoPorLogin = null,
                            DataCancelamento = null,
                            DevolvidoPorLogin = null,
                            DataDevolucao = null,
                            AprovadoPorLogin = null,
                            DataAprovacao = null,
                            ReprovadoPorLogin = null,
                            DataReprovacao = null,
                            Observacao = pedidoItem.Observacao,
                            Numero = numeroItem,
                            /*
                            Arquivos = pedidoItem.Arquivos.Select(x => {
                                x.Id = 0;
                                x.IdPedidoItem = 0;
                                x.PedidoItem = null;
                                return x;
                            }),
                            */
                            Arquivos = arquivosPedidoItem,
                            //Arquivos = null,
                            PedidoItemConversas = null,
                            /*
                            RCs = pedidoItem.RCs.Select(x => {
                                x.Id = 0;
                                x.IdPedidoItem = 0;
                                x.PedidoItem = null;
                                return x;
                            })
                            */
                            RCs = rcsPedidoItem
                            //RCs = null
                        };

                        PedidoItemArte pedidoItemArteNew = new PedidoItemArte()
                        {
                            /*PedidoItemarte*/
                            //pedidoItemNew.Id = 0;
                            //pedidoItemNew.IdPedidoItem = 0;
                            QuantidadePendenteCompra = pedidoItemNew.Quantidade,
                            QuantidadePendenteEntrega = 0,
                            QuantidadeEntregue = 0,
                            QuantidadeComprada = 0,
                            QuantidadeDevolvida = 0,
                            MarcacaoCena = pedidoItem.PedidoItemArte.MarcacaoCena,
                            IdStatus = (int)Domain.Enums.PedidoItemArteStatus.ITEM_SOLICITACAODECOMPRACRIADA,
                            Status = status,
                            Referencias = pedidoItem.PedidoItemArte.Referencias,
                            SugestaoFornecedor = pedidoItem.PedidoItemArte.SugestaoFornecedor,
                            SolicitacaoDirigida = pedidoItem.PedidoItemArte.SolicitacaoDirigida,
                            CompradoPorLogin = "",
                            DataVinculoComprador = null,
                            DataVisualizacaoComprador = null,
                            IdTipo = pedidoItem.PedidoItemArte.IdTipo,
                            DataReenvio = null,
                            DataEdicaoReenvio = null,
                            QuantidadeAprovacaoCompra = 0,
                            DataEntregaPrevista = pedidoItem.PedidoItemArte.DataEntregaPrevista,
                            ObservacaoAprovacaoCompra = pedidoItem.PedidoItemArte.ObservacaoAprovacaoCompra,
                            FlagDevolvidoBase = false,
                            FlagDevolvidoComprador = false,
                            //TrackingArte = null,
                            TrackingArte = trackingsPedidoItem,
                            /*TrackingArte = pedidoItem.PedidoItemArte.TrackingArte
                                .Where(a => a.StatusId < (int)PedidoItemArteStatus.ITEM_ATRIBUIDOAOCOMPRADOR)
                                .Select(x =>
                                {
                                    x.Id = 0;
                                    x.ChangedBy = mediator.Send(new GetUsuarioLogin() { Login = x.ChangeById }, cancellationToken).Result;
                                    x.Status = mediator.Send<StatusPedidoItemArte>(new GetById() { Id = x.StatusId }, cancellationToken).Result;
                                    x.PedidoItemArte = null;
                                    x.IdPedidoItemArte = 0;
                                    return x;
                                }),
                            */

                            Compras = null,
                            Entregas = null,
                            Comprador = null,
                            Devolucoes = null,
                            PedidoItem = pedidoItemNew
                        };

                        await mediator.Send(new CreatePedidoItemArte()
                        {
                            PedidoItemArte = pedidoItemArteNew
                        }, cancellationToken);

                        // Grava Tracking para o novo pedido item arte criado
                        // Grava os arquivos para o novo pedido item criado
                        // Grava RC para o novo pedido item criado

                        pedidoItem = await pedidoItemRepository.GetById(request.IdPedidoItem, cancellationToken);

                        pedidoItem.PedidoItemArte.QuantidadeDevolvida = pedidoItem.PedidoItemArte.QuantidadePendenteCompra;

                        pedidoItem.JustificativaDevolucao = request.PedidoItemDevolucao.Justificativa;

                        pedidoItem.DataDevolucao = DateTime.Now;

                        pedidoItem.DevolvidoPorLogin = userProvider.User.Login;

                        pedidoItem.PedidoItemArte.FlagDevolvidoComprador = true;

                        var userDevolucao = await userRepository.GetByLogin(pedidoItem.DevolvidoPorLogin, cancellationToken);

                        pedidoItem.DevolvidoPor = userDevolucao;

                        pedidoItem.PedidoItemArte.Compras = await mediator.Send<List<PedidoItemArteCompra>>(new ListByIdPedidoItemArteCompra() { IdPedidoItem = request.IdPedidoItem }, cancellationToken);

                        pedidoItem.PedidoItemArte.Entregas = await mediator.Send<List<PedidoItemArteEntrega>>(new ListByIdPedidoItemArteEntrega() { IdPedidoItem = request.IdPedidoItem }, cancellationToken);

                        pedidoItemRepository.AddOrUpdate(pedidoItem, cancellationToken);

                        var result = unitOfWork.SaveChanges();

                        if (!result) throw new ApplicationException("An error has occured.");

                        //request.PedidoItemDevolucao.PedidoItemArte.IdPedidoItem = pedidoItemNew.IdPedidoItem;
                        //request.PedidoItemDevolucao.IdPedidoItemArteOriginal = pedidoItem.PedidoItemArte.Id;
                        request.PedidoItemDevolucao.Quantidade = pedidoItemNew.Quantidade;
                        request.PedidoItemDevolucao.Comprador = pedidoItem.DevolvidoPorLogin;
                        request.PedidoItemDevolucao.UserComprador = userDevolucao;
                        request.PedidoItemDevolucao.DataDevolucao = DateTime.Now;
                        request.PedidoItemDevolucao.IdPedidoItemArte = pedidoItemArteNew.Id;
                        request.PedidoItemDevolucao.IdPedidoItemArteOriginal = pedidoItem.PedidoItemArte.Id;

                        pedidoItemDevolucaoRepository.AddOrUpdate(request.PedidoItemDevolucao, cancellationToken);

                        var resultDev = unitOfWork.SaveChanges();

                        if (!resultDev) throw new ApplicationException("An error has occured.");
                    }
                    else
                    {
                        //Atualiza pedidoItem
                        //Modifica Status para em Andamento
                        //Remover o comprador do PedidoItem
                        //Qtde devolvida do item original é zero(0)
                        if (pedidoItem.PedidoItemArte.FlagDevolvidoComprador) throw new BadRequestException("Item já foi devolvido pelo comprador.");

                        if (request.PedidoItemDevolucao.idTipo == 2)
                        {
                            if (userProvider.User.Login == pedidoItem.PedidoItemArte.CompradoPorLogin)
                            {
                                pedidoItem.PedidoItemArte.IdStatus = (int)PedidoItemArteStatus.ITEM_SOLICITACAODECOMPRACRIADA;

                                pedidoItem.PedidoItemArte.Status = status;

                                pedidoItem.PedidoItemArte.QuantidadeDevolvida = pedidoItem.PedidoItemArte.QuantidadePendenteCompra;

                                pedidoItem.PedidoItemArte.CompradoPorLogin = null;

                                pedidoItem.DevolvidoPorLogin = null;

                                pedidoItem.JustificativaDevolucao = null;

                                pedidoItem.DevolvidoPor = null;

                                pedidoItem.DataDevolucao = null;

                                pedidoItem.PedidoItemArte.FlagDevolvidoComprador = true;

                                pedidoItem.PedidoItemArte.FlagDevolvidoBase = false;
                            }
                            else
                                throw new BadRequestException("Somente o comprador pode devolver o pedido para a base.");
                        }
                        else
                        {
                            status = await statusItemRepository.GetById((int)PedidoItemArteStatus.ITEM_SOLICITACAODECOMPRACRIADA, cancellationToken);

                            if (status == null) throw new NotFoundException("Registro do status não encontrado.");

                            pedidoItem.PedidoItemArte.IdStatus = (int)PedidoItemArteStatus.ITEM_DEVOLUCAO;

                            pedidoItem.PedidoItemArte.Status = status;

                            pedidoItem.PedidoItemArte.QuantidadeDevolvida = 0;

                            pedidoItem.JustificativaDevolucao = request.PedidoItemDevolucao.Justificativa;

                            pedidoItem.DevolvidoPorLogin = userProvider.User.Login;

                            var userDevolucao = await mediator.Send(new GetUsuarioLogin() { Login = pedidoItem.DevolvidoPorLogin }, cancellationToken);

                            pedidoItem.DevolvidoPor = userDevolucao;

                            pedidoItem.DataDevolucao = DateTime.Now;

                            if (userProvider.User.Login == pedidoItem.PedidoItemArte.CompradoPorLogin)
                            {
                                pedidoItem.PedidoItemArte.FlagDevolvidoComprador = true;
                                pedidoItem.PedidoItemArte.FlagDevolvidoBase = false;
                            }
                            else
                            {
                                pedidoItem.PedidoItemArte.FlagDevolvidoComprador = false;
                                pedidoItem.PedidoItemArte.FlagDevolvidoBase = true;
                            }

                            await mediator.Publish(new OnDevolucaoPedidoItem() { PedidoItem = pedidoItem });

                        }

                        request.PedidoItemDevolucao.IdPedidoItemArte = pedidoItem.PedidoItemArte.Id;

                        request.PedidoItemDevolucao.IdPedidoItemArteOriginal = pedidoItem.PedidoItemArte.Id;

                        request.PedidoItemDevolucao.Quantidade = pedidoItem.PedidoItemArte.QuantidadePendenteCompra;

                        request.PedidoItemDevolucao.Comprador = comprador;

                        pedidoItem.PedidoItemArte.Compras = await mediator.Send<List<PedidoItemArteCompra>>(new ListByIdPedidoItemArteCompra() { IdPedidoItem = request.IdPedidoItem }, cancellationToken);

                        pedidoItem.PedidoItemArte.Entregas = await mediator.Send<List<PedidoItemArteEntrega>>(new ListByIdPedidoItemArteEntrega() { IdPedidoItem = request.IdPedidoItem }, cancellationToken);

                        pedidoItemRepository.AddOrUpdate(pedidoItem, cancellationToken);

                        var result = unitOfWork.SaveChanges();

                        if (!result) throw new ApplicationException("An error has occured.");

                        if (request.PedidoItemDevolucao.idTipo == 2)
                        {
                            pedidoItemDevolucaoRepository.AddOrUpdate(request.PedidoItemDevolucao, cancellationToken);

                            var resultDev = unitOfWork.SaveChanges();

                            if (!resultDev) throw new ApplicationException("An error has occured.");
                        }

                    }

                    //unitOfWork.CommitTransaction();

                    await mediator.Publish(new OnVerificarStatus() { Pedido = pedido });

                    await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = pedidoItem });

                }
                else throw new NotFoundException("Não é possível devolver o Item pois o mesmo já foi devolvido.");
            }
            catch (Exception error)
            {
                //unitOfWork.RollbackTransaction();

                throw new ApplicationException(error.Message);
            }

            return Unit.Value;
        }

        //async Task<Unit> IRequestHandler<UpdatePedidoItensDevolucao, Unit>.Handle(UpdatePedidoItensDevolucao request, CancellationToken cancellationToken)
        //{
        //	try
        //	{
        //		var pedido = await mediator.Send(new GetByIdPedidoWithOutRoles() { Id = request.IdPedido }, cancellationToken);

        //		if (pedido == null) throw new NotFoundException("Registro do pedido não encontrado.");

        //		var pedidoItens = pedidoItemRepository.GetAll().Where(a => a.IdPedido == pedido.Id).ToList();
        //		if (pedidoItens.Count > 0)
        //		{
        //			unitOfWork.BeginTransaction();

        //			foreach (var currentPedidoItem in pedidoItens)
        //			{
        //				var pedidoItem = await mediator.Send<PedidoItem>(new GetByIdPedidoItem() { Id = currentPedidoItem.Id }, cancellationToken);

        //				if (pedidoItem == null) throw new NotFoundException("Registro do pedido item não encontrado.");

        //				//var pedidoItemNew = await mediator.Send<PedidoItem>(new GetByIdPedidoItem() { Id = request.IdPedidoItem }, cancellationToken);

        //				var status = await mediator.Send(new GetByStatusItemId() { Id = (int)Domain.Enums.StatusItem.ITEM_EMANALISE }, cancellationToken);

        //				if (status == null) throw new NotFoundException("Registro do status não encontrado.");

        //				var comprador = pedidoItem.LoginComprador;
        //				//var pedidoItemCompra = await mediator.Send<List<PedidoItemCompra>>(new ListItemCompraIdByIdPedidoItem() { IdPedido = id, IdPedidoItem = idItem }, cancellationToken);
        //				if (pedidoItem.QuantidadeDevolvida == 0)
        //				{
        //					if (pedidoItem.QuantidadePendenteCompra != pedidoItem.Quantidade)
        //					{ //Item com compra
        //					  // Cria um novo pedido item para a quantidade devolvida.

        //						PedidoItem pedidoItemNew = new PedidoItem();

        //						pedidoItemNew.Id = 0;

        //						pedidoItemNew.LoginComprador = "";
        //						pedidoItemNew.Acordo = pedidoItem.Acordo;
        //						pedidoItemNew.BUId = pedidoItem.BUId;
        //						pedidoItemNew.Categoria = pedidoItem.Categoria;
        //						pedidoItemNew.DataAprovacao = pedidoItem.DataAprovacao;
        //						pedidoItemNew.DataCancelamento = pedidoItem.DataCancelamento;
        //						pedidoItemNew.DataDevolucao = pedidoItem.DataDevolucao;
        //						pedidoItemNew.DataEntregaPrevista = pedidoItem.DataEntregaPrevista;
        //						pedidoItemNew.DataEdicaoReenvio = pedidoItem.DataEdicaoReenvio;
        //						pedidoItemNew.DataEntrega = pedidoItem.DataEntrega;
        //						pedidoItemNew.DataEntregaPrevista = pedidoItem.DataEntregaPrevista;
        //						pedidoItemNew.DataNecessidade = pedidoItem.DataNecessidade;
        //						pedidoItemNew.DataReenvio = pedidoItem.DataReenvio;
        //						pedidoItemNew.DataReprovacao = pedidoItem.DataReprovacao;
        //						pedidoItemNew.DataVinculoComprador = null;
        //						pedidoItemNew.DataVisualizacaoComprador = null;
        //						pedidoItemNew.Descricao = pedidoItem.Descricao;
        //						pedidoItemNew.Fabricante = pedidoItem.Fabricante;
        //						pedidoItemNew.Fornecedor = pedidoItem.Fornecedor;
        //						pedidoItemNew.HeadId = pedidoItem.HeadId;
        //						pedidoItemNew.IdItemOracle = pedidoItem.IdItemOracle;
        //						pedidoItemNew.IdTipo = pedidoItem.IdTipo;
        //						pedidoItemNew.ItemFabricante = pedidoItem.ItemFabricante;
        //						pedidoItemNew.ItemKeyOracle = pedidoItem.ItemKeyOracle;
        //						pedidoItemNew.Justificativa = pedidoItem.Justificativa;
        //						pedidoItemNew.JustificativaCancelamento = pedidoItem.JustificativaCancelamento;
        //						pedidoItemNew.JustificativaDevolucao = pedidoItem.JustificativaDevolucao;
        //						pedidoItemNew.LineId = pedidoItem.LineId;
        //						pedidoItemNew.LocalEntrega = pedidoItem.LocalEntrega;
        //						pedidoItemNew.LoginAprovacao = "";
        //						pedidoItemNew.LoginCancelamento = pedidoItem.LoginCancelamento;
        //						pedidoItemNew.LoginComprador = "";
        //						pedidoItemNew.LoginDevolucao = "";
        //						pedidoItemNew.LoginReprovacao = "";
        //						pedidoItemNew.MarcacaoCena = pedidoItem.MarcacaoCena;
        //						pedidoItemNew.NomeItem = pedidoItem.NomeItem;
        //						pedidoItemNew.Observacao = pedidoItem.Observacao;
        //						pedidoItemNew.ObservacaoAprovacaoCompra = pedidoItem.ObservacaoAprovacaoCompra;
        //						pedidoItemNew.RcPedido = pedidoItem.RcPedido;
        //						pedidoItemNew.Referencias = pedidoItem.Referencias;
        //						pedidoItemNew.solicitacaoDirigida = pedidoItem.solicitacaoDirigida;
        //						pedidoItemNew.SugestaoFornecedor = pedidoItem.SugestaoFornecedor;
        //						pedidoItemNew.UnidadeMedida = pedidoItem.UnidadeMedida;
        //						pedidoItemNew.Valor = pedidoItem.Valor;
        //						pedidoItemNew.ValorItens = pedidoItem.ValorItens;
        //						pedidoItemNew.ValorUnitario = pedidoItem.ValorUnitario;


        //						pedidoItemNew.Quantidade = pedidoItem.QuantidadePendenteCompra;
        //						pedidoItemNew.QuantidadeAprovacaoCompra = 0;
        //						pedidoItemNew.QuantidadePendenteCompra = 0;
        //						pedidoItemNew.QuantidadeComprada = 0;
        //						pedidoItemNew.QuantidadeDevolvida = 0;
        //						pedidoItemNew.QuantidadeEntregue = 0;
        //						pedidoItemNew.QuantidadePendenteEntrega = 0;

        //						pedidoItemNew.ValorItens = pedidoItemNew.Quantidade * pedidoItemNew.Valor;
        //						pedidoItemNew.IdStatus = (int)Domain.Enums.StatusItem.ITEM_EMANALISE;
        //						pedidoItemNew.StatusPedidoItem = status;
        //						pedidoItemNew.IdPedido = pedidoItem.IdPedido;
        //						pedidoItemNew.Pedido = pedido;
        //						pedidoItemNew.Compras = null;
        //						pedidoItemNew.Entregas = null;
        //						pedidoItemNew.UserComprador = null;
        //						pedidoItemNew.Devolucao = null;
        //						pedidoItemNew.Arquivos = pedidoItem.Arquivos;
        //						pedidoItemNew.PedidoItemConversas = null;
        //						pedidoItemNew.Trackings = null;
        //						pedidoItemNew.UserDevolucao = null;
        //						pedidoItemNew.UserAprovacao = null;
        //						pedidoItemNew.UserCancelamento = null;
        //						pedidoItemNew.UserReprovacao = null;

        //						await mediator.Send(new SavePedidoItem()
        //						{
        //							PedidoItem = pedidoItemNew
        //						}, cancellationToken);

        //						// var result = mapper.Map<PedidoItemViewModel>(pedidoItemNew);

        //						pedidoItem = await mediator.Send<PedidoItem>(new GetByIdPedidoItem() { Id = currentPedidoItem.Id }, cancellationToken);

        //						pedidoItem.QuantidadeDevolvida = pedidoItem.QuantidadePendenteCompra;

        //						pedidoItem.JustificativaDevolucao = request.PedidoItemDevolucao.Justificativa;

        //						pedidoItem.DataDevolucao = DateTime.Now;

        //						pedidoItem.LoginDevolucao = userProvider.User.Login;

        //						var userDevolucao = await mediator.Send(new GetByLogin() { Login = pedidoItem.LoginDevolucao }, cancellationToken);

        //						pedidoItem.UserDevolucao = userDevolucao;

        //						pedidoItem.Compras = await mediator.Send<List<PedidoItemCompra>>(new ListByIdPedidoItemCompra() { IdPedidoItem = currentPedidoItem.Id }, cancellationToken);

        //						pedidoItem.Entregas = await mediator.Send<List<PedidoItemEntrega>>(new ListByIdPedidoItemEntrega() { IdPedidoItem = currentPedidoItem.Id }, cancellationToken);

        //						pedidoItemRepository.AddOrUpdate(pedidoItem, cancellationToken);

        //						var result = unitOfWork.SaveChanges();

        //						if (!result) throw new ApplicationException("An error has occured.");

        //						request.PedidoItemDevolucao.IdPedidoItem = pedidoItemNew.Id;
        //						request.PedidoItemDevolucao.IdPedidoItemOriginal = pedidoItem.Id;
        //						request.PedidoItemDevolucao.Quantidade = pedidoItem.QuantidadePendenteCompra;
        //						request.PedidoItemDevolucao.Comprador = comprador;

        //						pedidoItemDevolucaoRepository.AddOrUpdate(request.PedidoItemDevolucao, cancellationToken);

        //						var resultDev = unitOfWork.SaveChanges();

        //						if (!resultDev) throw new ApplicationException("An error has occured.");
        //					}
        //					else
        //					{
        //						//Atualiza pedidoItem
        //						//Modifica Status para em Andamento
        //						//Remover o comprador do PedidoItem
        //						//Qtde devolvida do item original é zero(0)

        //						request.PedidoItemDevolucao.IdPedidoItem = pedidoItem.Id;
        //						request.PedidoItemDevolucao.IdPedidoItemOriginal = pedidoItem.Id;
        //						request.PedidoItemDevolucao.Quantidade = pedidoItem.QuantidadePendenteCompra;
        //						request.PedidoItemDevolucao.Comprador = comprador;

        //						pedidoItem.IdStatus = (int)StatusItem.ITEM_EMANALISE;
        //						pedidoItem.StatusPedidoItem = status;
        //						pedidoItem.LoginComprador = "";
        //						pedidoItem.QuantidadeDevolvida = 0;

        //						pedidoItem.JustificativaDevolucao = request.PedidoItemDevolucao.Justificativa;

        //						pedidoItem.DataDevolucao = DateTime.Now;

        //						pedidoItem.LoginDevolucao = userProvider.User.Login;

        //						var userDevolucao = await mediator.Send(new GetByLogin() { Login = pedidoItem.LoginDevolucao }, cancellationToken);

        //						pedidoItem.UserDevolucao = userDevolucao;

        //						pedidoItem.Compras = await mediator.Send<List<PedidoItemCompra>>(new ListByIdPedidoItemCompra() { IdPedidoItem = currentPedidoItem.Id }, cancellationToken);

        //						pedidoItem.Entregas = await mediator.Send<List<PedidoItemEntrega>>(new ListByIdPedidoItemEntrega() { IdPedidoItem = currentPedidoItem.Id }, cancellationToken);

        //						pedidoItemRepository.AddOrUpdate(pedidoItem, cancellationToken);

        //						var result = unitOfWork.SaveChanges();

        //						if (!result) throw new ApplicationException("An error has occured.");


        //						pedidoItemDevolucaoRepository.AddOrUpdate(request.PedidoItemDevolucao, cancellationToken);

        //						var resultDev = unitOfWork.SaveChanges();

        //						if (!resultDev) throw new ApplicationException("An error has occured.");

        //					}
        //				}
        //			}

        //			pedido.LoginDevolucao = userProvider.User.Login;
        //			var userDev = await mediator.Send(new GetByLogin() { Login = userProvider.User.Login }, cancellationToken);
        //			pedido.UserDevolucao = userDev;
        //			pedido.DataDevolucao = DateTime.Now;
        //			pedido.JustificativaDevolucao = request.PedidoItemDevolucao.Justificativa;
        //			pedidoRepository.AddOrUpdate(pedido, cancellationToken);

        //			var resultPedido = unitOfWork.SaveChanges();

        //			if (!resultPedido) throw new ApplicationException("An error has occured.");


        //			unitOfWork.CommitTransaction();
        //		}
        //	}
        //	catch (Exception error)
        //	{
        //		unitOfWork.RollbackTransaction();
        //		throw new ApplicationException(error.Message);
        //	}

        //	return Unit.Value;
        //}

        async Task<Unit> IRequestHandler<DeletePedidoItemArte, Unit>.Handle(DeletePedidoItemArte request, CancellationToken cancellationToken)
        {
            var pedido = await pedidoRepository.GetByIdPedidoWithOutRoles(request.IdPedido, cancellationToken);

            if (pedido == null)
                throw new NotFoundException("pedido não encontrado!");

            var pedidoItem = await pedidoItemRepository.GetById(request.IdPedidoItem, cancellationToken);

            if (pedidoItem == null)
                throw new NotFoundException("id pedido item não encontrado!");

            var pedidoItemCompra = pedidoItemCompraRepository.GetAll().Where(a => a.PedidoItemArte.IdPedidoItem == request.IdPedidoItem).ToList();

            if (pedidoItemCompra.Count > 0)
                throw new NotFoundException(string.Format("Item {0} não pode ser editado, já existe compra vinculada.", request.IdPedidoItem));

            pedidoItemRepository.Remove(pedidoItem);

            //pedido.NroItens = pedido.Itens.Sum(a => a.Quantidade);

            var result = unitOfWork.SaveChanges();

            if (!result) throw new ApplicationException("An error has occured.");

            //await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = pedidoItem });

            return Unit.Value;
        }

        Task<Unit> IRequestHandler<DeleteAllPedidoItemArte, Unit>.Handle(DeleteAllPedidoItemArte request, CancellationToken cancellationToken)
        {

            var itens = pedidoItemRepository.GetAll().Where(a => a.IdPedido == request.IdPedido).ToList();
            if (itens == null)
                throw new NotFoundException(string.Format("Não existe itens do pedido {0} para serem excluídos.", request.IdPedido));

            pedidoItemRepository.Remove(itens);
            var result = unitOfWork.SaveChanges();

            if (!result) throw new ApplicationException("An error has occured.");

            foreach (var currentPedidoItem in itens)
            {
                mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = currentPedidoItem });
            }

            return Task.FromResult(Unit.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task<Unit> IRequestHandler<SendRC, Unit>.Handle(SendRC request, CancellationToken cancellationToken)
        {
            if (request.PedidoItens.Length <= 0) throw new BadRequestException("É necessário informar ao menos um item para enviar uma RC!");

            var pedido = request.PedidoItens[0].Pedido;

            List<AccountingSegmentRequisition> accountingSegments = new List<AccountingSegmentRequisition>();
            PurchaseRequisition purchaseRequisition = new PurchaseRequisition();
            List<Line> lines = new List<Line>();

            purchaseRequisition.activeRequisitionFlag = false;
            purchaseRequisition.auxiliaryAddress = $"{pedido.LocalEntrega} | {pedido.PedidoArte.LocalUtilizacao}";
            purchaseRequisition.functionalCurrencyCode = "BRL";
            purchaseRequisition.externallyManagedFlag = false;
            purchaseRequisition.interfaceSourceCode = "PIC";
            purchaseRequisition.preparerEmail = "roberto.c.campello@accenture.com";
            purchaseRequisition.requisitionBusinessUnit = userProvider.User.UnidadeNegocio.Codigo;
            purchaseRequisition.type = "Req.Compra";
            purchaseRequisition.documentSource = purchaseRequisition.sourceApplicationCode = pedido.Id.ToString();
            purchaseRequisition.additionalInformation =
                $"PIC" +
                $" | {pedido.Id}" +
                $" | {pedido.Titulo}" +
                $" | {pedido.Conteudo.Nome}" +
                $" | {(string.IsNullOrWhiteSpace(pedido.CriadoPor.Apelido) ? $"{pedido.CriadoPor.Name} {pedido.CriadoPor.LastName}" : pedido.CriadoPor.Apelido)}" +
                $" | {pedido.PedidoArte.DataNecessidade.ToCompleteDateTime()}";


            accountingSegments.Add(new AccountingSegmentRequisition
            {
                type = "centroDeCusto",
                value = pedido.CentroCusto
            });

            accountingSegments.Add(new AccountingSegmentRequisition
            {
                type = "finalidade",
                value = pedido.Finalidade
            });

            int number = 0;

            foreach (var item in request.PedidoItens)
            {
                number++;

                var requisicaoCompra = item.RCs.FirstOrDefault();

                if (item.DataNecessidade.Value.Date < DateTime.Now.Date)
                    throw new BadRequestException("Data necessidade está fora do período!");

                Line line = new Line();
                List<Distribution> distributions = new List<Distribution>();

                var expenditures = await expenditureProxy.GetExpenditures(requisicaoCompra.Categoria, cancellationToken);

                if (expenditures.Count < 1)
                    throw new NotFoundException(string.Format("Dispêndios não encontrados para a Categoria {0}.", requisicaoCompra.Categoria));

                var organization = expenditures.Where(a => a.OrganizationName == "Supplier Invoices").FirstOrDefault();

                if (organization == null)
                    throw new NotFoundException(string.Format("Supplier Invoices não encontrados para a Categoria {0}.", requisicaoCompra.Categoria));

                distributions.Add(new Distribution
                {
                    accountingSegments = accountingSegments,
                    expenditure = new Expenditure
                    {
                        date = item.DataNecessidade.Value.ToString("yyyy-MM-dd"),
                        organizationName = Environment.GetEnvironmentVariable("OIC_ORGANIZATIONID"),
                        typeName = organization.Id.ToString()
                    },
                    number = 1,
                    projectNumber = pedido.IdProjeto.ToString(),
                    quantity = item.Quantidade,
                    taskNumber = pedido.IdTarefa.ToString()
                });

                line.currencyCode = "BRL";
                line.deliverToLocationCode = item.Pedido.DeliverToLocationCode;
                line.destinationTypeCode = "EXPENSE";
                line.item = requisicaoCompra.ItemCodigo;
                line.number = number;
                line.quantity = item.Quantidade;

                line.unitPrice = string.IsNullOrWhiteSpace(requisicaoCompra.Acordo) ? item.ValorUnitario : requisicaoCompra.Valor;

                line.requestedDeliveryDate = item.DataNecessidade.Value.ToString("yyyy-MM-dd");
                line.requesterEmail = Environment.GetEnvironmentVariable("OIC_REQUESTEREMAIL"); //pedido.CriadoPor.Email;
                line.typeCode = "Goods";
                line.destinationOrganizationCode = pedido.DestinationOrganizationCode;
                line.negotiatedByPreparerFlag = true;
                line.distribution = distributions;
                line.sourceLineId = item.Numero;

                if (!string.IsNullOrWhiteSpace(requisicaoCompra.Acordo))
                {
                    line.agreementId = requisicaoCompra.AcordoId;
                    line.agreementLineId = requisicaoCompra.AcordoLinhaId;
                }
                else
                {
                    //ENPRJ-64059
                    //se não tiver acordo, preenche os dados de fornecedor informados pelo usuário e salvos na rc.
                    var rc = item.RCs.FirstOrDefault();
                    if (rc != null)
                        line.supplierId = rc.FornecedorId.ToString();
                }


                lines.Add(line);
            }

            purchaseRequisition.lines = lines;

            var requistion = await purchaseRequisitionProxy.PostPurchaseRequisitionAsync(purchaseRequisition, cancellationToken);

            if (requistion == null || string.IsNullOrWhiteSpace(requistion.Id)) throw new BadRequestException("A criação da Requisição de Compra não pode ser confirmada pelo ERP, por favor, tente novamente!");

            foreach (var item in request.PedidoItens)
                foreach (var rc in item.RCs)
                {
                    var reqLine = requistion.lines.Where(x => x.item.Equals(rc.ItemCodigo)).FirstOrDefault();

                    rc.HeadId = requistion.Id;
                    rc.BUId = requistion.requisitionBusinessUnit;
                    rc.Requisition = requistion.Code;
                    rc.Status = requistion.DocumentStatusCode;
                    rc.LineId = reqLine.id;

                    var statusCompraEnviada =
                        await statusItemRepository.GetById((int)PedidoItemArteStatus.ITEM_SOLICITACAODECOMPRAENVIADA, cancellationToken);
                    item.PedidoItemArte.IdStatus = (int)PedidoItemArteStatus.ITEM_SOLICITACAODECOMPRAENVIADA;
                    item.PedidoItemArte.Status = statusCompraEnviada;

                    pedidoItemRepository.AddOrUpdate(item, cancellationToken);

                    var result = unitOfWork.SaveChanges();

                    await mediator.Send(new AddTrackingArte()
                    {
                        PedidoItemArteTracking = new PedidoItemArteTracking()
                        {
                            IdPedidoItemArte = item.PedidoItemArte.Id,
                            StatusId = item.PedidoItemArte.IdStatus,
                            ChangeById = pedido.CriadoPorLogin
                        }
                    }, cancellationToken);
                }

            return await Unit.Task;
        }
    }
}
