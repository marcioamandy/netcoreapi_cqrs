using System;
using System.Linq;
using Globo.PIC.Domain.Entities;
using MediatR;
using System.Threading.Tasks;
using Globo.PIC.Domain.Types.Events;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Types.Commands;
using System.Threading;
using System.Collections.Generic;
using System.Globalization;
using Globo.PIC.Domain.Models;
using Globo.PIC.Domain.Exceptions;
using Globo.PIC.Domain.Enums;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.EventHandlers
{
	/// <summary>
	///
	/// </summary>
	public class PedidoVeiculoEventHandler
		:

		INotificationHandler<OnStatusPedidoVeiculoAlterado>

	{
		 
		/// <summary>
		/// 
		/// </summary>
		private readonly IUnitOfWork unitOfWork;

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<Notificacao> notificationRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItem> pedidoItemRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemVeiculo> pedidoItemVeiculoRepository;

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
		private readonly IRepository<StatusPedidoVeiculo> statusPedidoRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<StatusPedidoItemVeiculo> statusPedidoItemRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemVeiculoTracking> trackingRepository;

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
		private readonly IEmailSender emailSender;

		/// <summary>
		/// 
		/// </summary>
		private readonly IMediator mediator;

		private readonly IPurchaseRequisitionProxy proxyOIC;

		public PedidoVeiculoEventHandler(IPurchaseRequisitionProxy _proxyOIC)
		{
			proxyOIC = _proxyOIC;
		}

		private readonly IPurchaseRequisitionProxy purchaseRequisitionProxy;
		/// <summary>
		///
		/// </summary>
		public EmailSettings emailSettings { get; }

		public PedidoVeiculoEventHandler(
			IUnitOfWork _unitOfWork,
			IRepository<PedidoItem> _pedidoItemRepository,
			IRepository<PedidoItemVeiculo> _pedidoItemVeiculoRepository,
			IRepository<Notificacao> _notificationRepository,
			IRepository<StatusPedidoItemVeiculo> _statusPedidoItemRepository,
			IRepository<Usuario> _userRepository,
			IRepository<StatusPedidoVeiculo> _statusPedidoRepository,
			IRepository<Pedido> _pedidoRepository,
			IRepository<PedidoVeiculo> _pedidoVeiculoRepository,
			IRepository<PedidoItemVeiculoTracking> _trackingRepository,
			IUserProvider _userProvider,
			IMediator _mediator,
			IEmailSender _emailSender,
			EmailSettings _emailSettings,
			IPurchaseRequisitionProxy _purchaseRequisitionProxy)
		{
			unitOfWork = _unitOfWork;
			pedidoItemRepository = _pedidoItemRepository;
			pedidoItemVeiculoRepository = _pedidoItemVeiculoRepository;
			notificationRepository = _notificationRepository;
			statusPedidoItemRepository = _statusPedidoItemRepository;
			statusPedidoRepository = _statusPedidoRepository;
			userRepository = _userRepository;
			pedidoRepository = _pedidoRepository;
			pedidoVeiculoRepository = _pedidoVeiculoRepository;
			trackingRepository = _trackingRepository;
			userProvider = _userProvider;
			mediator = _mediator;
			emailSettings = _emailSettings;
			purchaseRequisitionProxy = _purchaseRequisitionProxy;
			emailSender = _emailSender; 
		}

		Task INotificationHandler<OnStatusPedidoVeiculoAlterado>.Handle(OnStatusPedidoVeiculoAlterado pedido, CancellationToken cancellationToken)
		{
			//Todo: Remover try quando bus de eventos estiver assincrono.
			//Enquanto este evendo estiver vindo da thread sincronda na query de notificações, o try evita que a consulta seja travada com alguma exception
			int idNewStatusItem = (int)Domain.Enums.PedidoItemVeiculoStatus.PEDIDOITEM_RASCUNHO;

			var existPedidoItem = pedidoItemRepository.GetAll().Where(a => a.IdPedido == pedido.Pedido.IdPedido).ToList();

			if (existPedidoItem.Count <= 0)
				throw new NotFoundException("Pedido item não encontrado");

			foreach (var pedidoItem in existPedidoItem)
			{
				var existPedidoItemVeiculo = pedidoItemVeiculoRepository.GetByIdPedidoItemVeiculo(pedidoItem.Id, cancellationToken).GetAwaiter().GetResult();

				if (existPedidoItemVeiculo != null)
				{

					switch (pedido.Pedido.IdStatus)
					{
						case (int)PedidoVeiculoStatus.PEDIDO_RASCUNHO:
							{
								idNewStatusItem = (int)PedidoItemVeiculoStatus.PEDIDOITEM_RASCUNHO;
								break;
							}
						case (int)PedidoVeiculoStatus.PEDIDO_ENVIADO:
							{
								idNewStatusItem = (int)PedidoItemVeiculoStatus.PEDIDOITEM_ENVIADO;
								break;
							}
						case (int)PedidoVeiculoStatus.PEDIDO_APROVADO:
							{
								idNewStatusItem = (int)PedidoItemVeiculoStatus.PEDIDOITEM_APROVADO;
								break;
							}
						case (int)PedidoVeiculoStatus.PEDIDO_CANCELADO:
							{
								idNewStatusItem = (int)PedidoItemVeiculoStatus.PEDIDOITEM_CANCELADO;
								break;
							}
						case (int)PedidoVeiculoStatus.PEDIDO_DEVOLVIDO:
							{
								idNewStatusItem = (int)PedidoItemVeiculoStatus.PEDIDOITEM_DEVOLUCAO;
								break;
							}
						case (int)PedidoVeiculoStatus.PEDIDO_EMANDAMENTO:
							{
								idNewStatusItem = (int)PedidoItemVeiculoStatus.PEDIDOITEM_EMANALISE;
								break;
							}
						case (int)PedidoVeiculoStatus.PEDIDO_FINALIZADO:
							{
								idNewStatusItem = 0;
								break;
							}
						default:
							break;
					}

					if (idNewStatusItem > 0)
					{
						/*
						mediator.Send(new MudarStatusPedidoItemVeiculo()
						{
							IdPedidoItem = pedidoItem.Id,
							IdStatus = idNewStatusItem
						}, cancellationToken);
						*/
						///
						/* Retirar depois de implementar HangFire Worker*/
						///

						bool statusModificado = false;


						pedidoItem.PedidoItemVeiculo = existPedidoItemVeiculo;

						var existStatus = statusPedidoItemRepository.GetById(idNewStatusItem, cancellationToken).GetAwaiter().GetResult();

						if (existStatus == null)
							throw new NotFoundException("Status não encontrado");

						if (existPedidoItemVeiculo.IdStatus != idNewStatusItem)
							statusModificado = true;

						if (statusModificado)
						{
							bool registraTracking = false;

							switch (existPedidoItemVeiculo.IdStatus)
							{
								case (int)PedidoItemVeiculoStatus.PEDIDOITEM_RASCUNHO:
									{
										registraTracking = false;

										if ((idNewStatusItem != (int)PedidoItemVeiculoStatus.PEDIDOITEM_ENVIADO))
											throw new ApplicationException("Mudança do status do pedido não permitido para o status atual");
										else
											registraTracking = true;

										break;
									}
								case (int)PedidoItemVeiculoStatus.PEDIDOITEM_ENVIADO:
									{
										registraTracking = true;

										if ((idNewStatusItem != (int)PedidoItemVeiculoStatus.PEDIDOITEM_EMANALISE))
											throw new ApplicationException("Mudança do status do pedido não permitido para o status atual");
										else
											registraTracking = true;

										break;
									}
								case (int)PedidoItemVeiculoStatus.PEDIDOITEM_EMANALISE:
									{
										registraTracking = true;

										if ((idNewStatusItem != (int)PedidoItemVeiculoStatus.PEDIDOITEM_OPCAOAPROVADA) &&
											(idNewStatusItem != (int)PedidoItemVeiculoStatus.PEDIDOITEM_SOLICITACAOEMPRESTIMOEXPIRADA) &&
											(idNewStatusItem != (int)PedidoItemVeiculoStatus.PEDIDOITEM_DEVOLUCAO))
											throw new ApplicationException("Mudança do status do pedido não permitido para o status atual");
										else
											registraTracking = true;

										break;
									}
								case (int)PedidoItemVeiculoStatus.PEDIDOITEM_OPCAOAPROVADA:
									{
										registraTracking = true;

										if ((idNewStatusItem != (int)PedidoItemVeiculoStatus.PEDIDOITEM_ACIONAMENTOSOLICITADO))
											throw new ApplicationException("Mudança do status do pedido não permitido para o status atual");
										else
											registraTracking = true;

										break;
									}
								case (int)PedidoItemVeiculoStatus.PEDIDOITEM_ACIONAMENTOSOLICITADO:
									{
										registraTracking = true;

										if ((idNewStatusItem != (int)PedidoItemVeiculoStatus.PEDIDOITEM_ACIONADO))
											throw new ApplicationException("Mudança do status do pedido não permitido para o status atual");
										else
											registraTracking = true;

										break;
									}
								case (int)PedidoItemVeiculoStatus.PEDIDOITEM_ACIONADO:
									{
										registraTracking = true;

										if ((idNewStatusItem == (int)PedidoItemVeiculoStatus.PEDIDOITEM_RCAPROVADA))
											throw new ApplicationException("Mudança do status do pedido não permitido para o status atual");
										else
											registraTracking = true;

										break;
									}
								default:
									break;
							}

							var existPedido = pedidoRepository.GetById(pedidoItem.IdPedido, cancellationToken).GetAwaiter().GetResult();
							if (existPedido == null)
								throw new NotFoundException("Pedido não encontrado");

							switch (idNewStatusItem)
							{
								case (int)PedidoItemVeiculoStatus.PEDIDOITEM_CANCELADO:
									{
										registraTracking = true;

										existPedidoItemVeiculo.PedidoItem.JustificativaCancelamento = existPedido.JustificativaCancelamento;
										existPedidoItemVeiculo.PedidoItem.DataCancelamento = DateTime.Now;
										existPedidoItemVeiculo.PedidoItem.CanceladoPor = existPedido.CanceladoPor;
										existPedidoItemVeiculo.PedidoItem.CanceladoPorLogin = existPedido.CanceladoPorLogin;

										break;
									}
								case (int)PedidoItemVeiculoStatus.PEDIDOITEM_DEVOLUCAO:
									{
										registraTracking = true;

										existPedidoItemVeiculo.PedidoItem.JustificativaDevolucao = existPedido.JustificativaDevolucao;
										existPedidoItemVeiculo.PedidoItem.DataDevolucao = DateTime.Now;
										existPedidoItemVeiculo.PedidoItem.DevolvidoPor = existPedido.DevolvidoPor;
										existPedidoItemVeiculo.PedidoItem.DevolvidoPorLogin = existPedido.DevolvidoPorLogin;

										break;
									}
								default:
									break;
							}

							existPedidoItemVeiculo.IdStatus = idNewStatusItem;
							existPedidoItemVeiculo.Status = existStatus;
							existPedidoItemVeiculo.PedidoItem = pedidoItem;
							existPedidoItemVeiculo.PedidoItem.Pedido = existPedido;

							//todo: remover persistência do EventHandle, persistir nos CommandHandlers
							pedidoItemVeiculoRepository.AddOrUpdate(existPedidoItemVeiculo, cancellationToken);

							var result = unitOfWork.SaveChanges();
							if (!result) throw new ApplicationException("An error has occured.");

							if (registraTracking)
							{
								/*
								mediator.Send(new AddTrackingVeiculo()
								{
									PedidoItemVeiculoTracking = new PedidoItemVeiculoTracking()
									{
										IdPedidoItem = existPedidoItemVeiculo.Id,
										StatusId = existPedidoItemVeiculo.IdStatus,
										ChangeById = existPedido.CriadoPorLogin
									}
								}, cancellationToken);
								*/
								var trackingList = trackingRepository.GetAll().Where(s => s.IdPedidoItem == existPedidoItemVeiculo.Id).ToList();

								//Verifica se o status solicitado já é o status atual
								if (trackingList.Count() > 0 && trackingList.Last().StatusId == existPedidoItemVeiculo.IdStatus)
									return Task.FromResult(Unit.Value);

								var tracking = new PedidoItemVeiculoTracking()
								{
									StatusId = existPedidoItemVeiculo.IdStatus,
									IdPedidoItem = existPedidoItemVeiculo.Id,
									ChangeById = userProvider.User.Login,

								};

								if (trackingList.FirstOrDefault() != null)
									tracking.StatusPosition = trackingList.Last().StatusPosition + 1;
								else
									tracking.StatusPosition = 1;

								tracking.TrackingDate = DateTime.Now;

								var existUsuario = userRepository.GetByLogin(tracking.ChangeById, cancellationToken).GetAwaiter().GetResult();
								if (existUsuario == null)
									throw new NotFoundException("Usuário não encontrado");

								tracking.ChangedBy = existUsuario;

								trackingRepository.Add(tracking, cancellationToken);

								var resultTracking = unitOfWork.SaveChanges();

								if (!resultTracking) throw new ApplicationException("An error has occured.");
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

					}
				}

			}

			return Task.FromResult(true);
		}


	}
}

