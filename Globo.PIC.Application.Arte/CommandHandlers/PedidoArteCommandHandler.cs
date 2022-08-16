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

namespace Globo.PIC.Application.Services.Commands
{
    public class PedidoArteCommandHandler :
        IRequestHandler<CreatePedidoArte>,
        IRequestHandler<UpdatePedidoArte>,
        IRequestHandler<SaveNotificacao>

    //IRequestHandler<UpdatePedidoStatus>,
    //IRequestHandler<UpdatePedidoDevolucao>
    {
        private readonly IUserProvider userProvider;


        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<Notificacao> notificacaoRepository;

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
        private readonly IRepository<StatusPedidoArte> statusRepository;

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
        private readonly IRepository<PedidoArte> pedidoArteRepository;

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
        public PedidoArteCommandHandler(
            IUnitOfWork _unitOfWork,
            IMediator _mediator,
            IRepository<Pedido> _pedidoRepository,
            IRepository<PedidoArte> _pedidoArteRepository,
            IRepository<StatusPedidoArte> _statusRepository,
            IRepository<Usuario> _userRepository,
            IRepository<Equipe> _pedidoEquipeRepository,
            IRepository<PedidoItem> _pedidoItemRepository,
            IUserProvider _userProvider,
            IPurchaseRequisitionProxy _purchaseRequisitionProxy,
            IRepository<Notificacao> _notificacaoRepository,
            ITasksProxy _projectTask,
            IMapper _mapper
            )
        {
            unitOfWork = _unitOfWork;
            mediator = _mediator;
            pedidoRepository = _pedidoRepository;
            pedidoArteRepository = _pedidoArteRepository;
            statusRepository = _statusRepository;
            userRepository = _userRepository;
            pedidoEquipeRepository = _pedidoEquipeRepository;
            pedidoItemRepository = _pedidoItemRepository;
            userProvider = _userProvider;
            purchaseRequisitionProxy = _purchaseRequisitionProxy;
            notificacaoRepository = _notificacaoRepository;
            projectTask = _projectTask;
            mapper = _mapper;
        }

        void RunEntityValidation(PedidoArte pedidoArte)
        {
            if (pedidoArte.Pedido.IdConteudo <= 0)
                throw new ValidationException("O Conteúdo não pode ser nulo.");

            if (pedidoArte.Pedido.IdProjeto <= 0)
                throw new ValidationException("O Projeto não pode ser nulo.");

            if (pedidoArte.Pedido.IdTarefa <= 0)
                throw new ValidationException("A Tarefa não pode ser nulo.");

            if (string.IsNullOrWhiteSpace(pedidoArte.LocalUtilizacao))
                throw new ValidationException("O Local de Utilização não pode ser nulo.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        Task<Unit> IRequestHandler<SaveNotificacao, Unit>.Handle(SaveNotificacao request, CancellationToken cancellationToken)
        {
            notificacaoRepository.AddOrUpdate(request.Notificacao, cancellationToken);

            var result = unitOfWork.Commit();

            if (!result) throw new ApplicationException("An error has occured.");

            return Task.FromResult(Unit.Value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        async Task<Unit> IRequestHandler<CreatePedidoArte, Unit>.Handle(CreatePedidoArte request, CancellationToken cancellationToken)
        {

            RunEntityValidation(request.PedidoArte);

            foreach (var equipe in request.PedidoArte.Pedido.Equipe)
            {
                var usuario = await userRepository.GetByLogin(equipe.Login, cancellationToken);

                if (usuario == null)
                    throw new NotFoundException("usuário equipe não encontrado: " + equipe.Login);
                else
                {
                    equipe.Usuario = usuario;
                }
            }

            var tarefa = await projectTask.GetResultTaskAsync(request.PedidoArte.Pedido.IdProjeto, request.PedidoArte.Pedido.IdTarefa, cancellationToken);

            if (tarefa != null)
            {
                request.PedidoArte.Pedido.IdTarefa = tarefa.Id;
                request.PedidoArte.Pedido.DescricaoTarefa = tarefa.Description;
            }

            pedidoArteRepository.AddOrUpdate(request.PedidoArte, cancellationToken);

            var result = unitOfWork.SaveChanges();

            if (!result) throw new ApplicationException("An error has occured.");

            return await Unit.Task;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task<Unit> IRequestHandler<UpdatePedidoArte, Unit>.Handle(UpdatePedidoArte request, CancellationToken cancellationToken)
        {

            RunEntityValidation(request.PedidoArte);

            bool
                dtNecessidadeModificada = false,
                dtReenvioModificada = false,
                statusModificado = false;

            var existPedido = await pedidoRepository.GetByIdPedidoWithOutRoles(request.PedidoArte.IdPedido, cancellationToken);

            if (existPedido == null)
                throw new NotFoundException("Pedido não encontrado");

            var existPedidoArte = await pedidoArteRepository.GetById(existPedido.PedidoArte.Id, cancellationToken);

            if (existPedidoArte == null)
                throw new NotFoundException("Pedido não encontrado");

            request.PedidoArte.Id = existPedido.PedidoArte.Id;

            //todo: persistir aqui a data necessidades
            dtNecessidadeModificada = !(
                (existPedidoArte.DataNecessidade.HasValue ? existPedidoArte.DataNecessidade.Value.ToShortDateString() : string.Empty)
                ==
                (request.PedidoArte.DataNecessidade.HasValue ? request.PedidoArte.DataNecessidade.Value.ToShortDateString() : string.Empty)
            );

            dtReenvioModificada = !(
                (existPedidoArte.DataReenvio.HasValue ? existPedidoArte.DataReenvio.Value.ToShortDateString() : string.Empty)
                ==
                (request.PedidoArte.DataReenvio.HasValue ? request.PedidoArte.DataReenvio.Value.ToShortDateString() : string.Empty)
            );

            statusModificado = existPedidoArte.IdStatus != request.PedidoArte.IdStatus;

            if (request.PedidoArte.Pedido.Equipe != null)
            {
                List<Equipe> newEquipe = new List<Equipe>();

                var equipes = pedidoEquipeRepository.GetAll().Where(a => a.IdPedido == existPedido.Id).ToList();

                pedidoEquipeRepository.Remove(equipes);

                foreach (var equipe in request.PedidoArte.Pedido.Equipe)
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

            existPedido.DestinationOrganizationId = request.PedidoArte.Pedido.DestinationOrganizationId;
            existPedido.DestinationOrganizationCode = request.PedidoArte.Pedido.DestinationOrganizationCode;
            existPedido.DeliverToLocationId = request.PedidoArte.Pedido.DeliverToLocationId;
            existPedido.DeliverToLocationCode = request.PedidoArte.Pedido.DeliverToLocationCode;

            existPedido.Titulo = request.PedidoArte.Pedido.Titulo;
            existPedido.IdConteudo = request.PedidoArte.Pedido.IdConteudo;
            existPedido.IdProjeto = request.PedidoArte.Pedido.IdProjeto;
            existPedido.LocalEntrega = request.PedidoArte.Pedido.LocalEntrega;
            existPedido.Finalidade = request.PedidoArte.Pedido.Finalidade;
            existPedido.CentroCusto = request.PedidoArte.Pedido.CentroCusto;
            existPedido.Conta = request.PedidoArte.Pedido.Conta;

            if (request.PedidoArte.Pedido.IdTarefa != existPedido.IdTarefa)
            {
                var tarefa = await projectTask.GetResultTaskAsync(request.PedidoArte.Pedido.IdProjeto, request.PedidoArte.Pedido.IdTarefa, cancellationToken);

                if (tarefa != null)
                {
                    existPedido.IdTarefa = tarefa.Id;
                    existPedido.DescricaoTarefa = tarefa.Description;
                }
            }


            await mediator.Send(new UpdatePedido() { Pedido = existPedido }, cancellationToken);

            existPedidoArte.DataNecessidade = request.PedidoArte.DataNecessidade;
            existPedidoArte.LocalUtilizacao = request.PedidoArte.LocalUtilizacao;
            existPedidoArte.DescricaoCena = request.PedidoArte.DescricaoCena;
            existPedidoArte.FlagFastPass = request.PedidoArte.FlagFastPass;
            existPedidoArte.FlagPedidoAlimentos = request.PedidoArte.FlagPedidoAlimentos;
                
            if (request.PedidoArte.DataEdicaoReenvio.HasValue)
                existPedidoArte.DataEdicaoReenvio = request.PedidoArte.DataEdicaoReenvio.Value;

            if (request.PedidoArte.DataReenvio.HasValue)
                existPedidoArte.DataReenvio = request.PedidoArte.DataReenvio.Value;

            pedidoArteRepository.AddOrUpdate(existPedidoArte, cancellationToken);

            var result = unitOfWork.SaveChanges();

            if (!result) throw new ApplicationException("An error has occured.");

            if (dtNecessidadeModificada)
                await mediator.Publish(new OnDataNecessidadeAlterada() { Pedido = existPedido });

            if (dtReenvioModificada)
                await mediator.Publish(new OnDataReenvioAlterada() { Pedido = existPedido });

            if (!string.IsNullOrWhiteSpace(request.LoginComprador) && (existPedidoArte.IdStatus == (int)PedidoArteStatus.PEDIDO_EMANDAMENTO))
            {
                if (request.PedidoArte.Pedido.IdTipo > 0)
                    existPedido.IdTipo = request.PedidoArte.Pedido.IdTipo;
                else
                    throw new NotFoundException("O Tipo do pedido não pode ser nulo quando o comprador é atribuído");

                if (!string.IsNullOrWhiteSpace(request.PedidoArte.Pedido.Observacao))
                    existPedido.Observacao = request.PedidoArte.Pedido.Observacao;

                await mediator.Publish(new OnCompradorAlterado()
                {
                    Pedido = existPedido,
                    CompradoPorLogin = request.LoginComprador
                });
            }

            //if (!string.IsNullOrWhiteSpace(request.PedidoArte.BaseLogin) && (existPedidoArte.IdStatus == (int)PedidoArteStatus.PEDIDO_ENVIADO))
            //{
            //    existPedidoArte.BaseLogin = request.PedidoArte.BaseLogin;

            //    await mediator.Publish(new OnBaseAlterada() { Pedido = existPedido });

            //    existPedidoArte.IdStatus = (int)PedidoArteStatus.PEDIDO_EMANDAMENTO;

            //    await mediator.Publish(new OnNovoPedidoArte() { Pedido = existPedido });
            //}

            if (statusModificado)
            {
                await mediator.Publish(new OnStatusPedidoArteAlterado()
                {
                    PedidoArte = request.PedidoArte
                });
            }

            return await Unit.Task;
        }

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