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
using Globo.PIC.Domain.Types.Queries.Filters;
using Globo.PIC.Domain.Exceptions;
using Globo.PIC.Domain.ViewModels;
using Globo.PIC.Domain.Models;

namespace Globo.PIC.Application.Services.Commands
{
	/// <summary>
	/// 
	/// </summary>
	public class PedidoItemCommandHandler :

		IRequestHandler<DeletePedidoItem>,
		IRequestHandler<DeleteAllPedidoItem>,
		IRequestHandler<UpdatePedidoItem>,
		IRequestHandler<UpdatePedidoItens>,
		IRequestHandler<SavePedidoItem>,
		IRequestHandler<SavePedidoItens>,
		IRequestHandler<UpdatePedidoItemDevolucao>,
		IRequestHandler<UpdatePedidoItensDevolucao>,
		IRequestHandler<SaveRequisitionItem>
	{

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItem> pedidoItemRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemDevolucao> pedidoItemDevolucaoRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemCompra> pedidoItemCompraRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<StatusPedidoItem> statusItemRepository;
		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<Pedido> pedidoRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemAnexos> pedidoItemAnexosRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<Tracking> trackingRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<User> userRepository;

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

		private readonly IPurchaseRequisitionProxy purchaseRequisitionProxy;

		/// <summary>
		/// 
		/// </summary>  
		public PedidoItemCommandHandler(
			IUnitOfWork _unitOfWork,
			IMediator _mediator,
			IRepository<PedidoItem> _pedidoItemRepository,
			IRepository<PedidoItemDevolucao> _pedidoItemDevolucaoRepository,
			IRepository<PedidoItemCompra> _pedidoItemCompraRepository,
			IRepository<StatusPedidoItem> _statusItemRepository,
			IRepository<Pedido> _pedidoRepository,
			IRepository<PedidoItemAnexos> _pedidoItemAnexosRepository,
			IRepository<Tracking> _trackingRepository,
			IRepository<User> _userRepository,
			IUserProvider _userProvider, 
			IPurchaseRequisitionProxy _purchaseRequisitionProxy)
		{
			unitOfWork = _unitOfWork;
			mediator = _mediator;
			pedidoItemRepository = _pedidoItemRepository;
			pedidoItemDevolucaoRepository = _pedidoItemDevolucaoRepository;
			pedidoItemCompraRepository = _pedidoItemCompraRepository;
			statusItemRepository = _statusItemRepository;
			pedidoRepository = _pedidoRepository;
			pedidoItemAnexosRepository = _pedidoItemAnexosRepository;
			trackingRepository = _trackingRepository;
			userRepository = _userRepository;
			userProvider = _userProvider;
			purchaseRequisitionProxy = _purchaseRequisitionProxy;
		}

		protected void RunEntityValidation(PedidoItem pedido)
		{
			if (pedido == null)
				throw new ApplicationException("O Pedido Item está vazio!");

			if (pedido.LocalEntrega == "")
				throw new ApplicationException("O local de entrega é obrigatório!");

			if (pedido.NomeItem == "")
				throw new ApplicationException("O Nome do item é obrigatório!");
		}

		public void RunEntityValidationList(List<PedidoItem> pedidoItemViewModels)
		{
			if (pedidoItemViewModels.Count() == 0)
				throw new ApplicationException("O Pedido Item está vazio!");

			foreach (var pedido in pedidoItemViewModels)
				RunEntityValidation(pedido);
		}

		/// <summary>
		/// atualiza um pedido item
		/// </summary>
		/// <param name="request">PedidoItem</param>
		/// <param name="cancellationToken">cancellation token</param>
		/// <returns></returns>
		async Task<Unit> IRequestHandler<UpdatePedidoItem, Unit>.Handle(UpdatePedidoItem request, CancellationToken cancellationToken)
		{
			try
			{
				bool statusChange = false;

				RunEntityValidation(request.PedidoItem);

				var pedidoItemCompra = pedidoItemCompraRepository.GetAll().Where(a => a.IdPedidoItem == request.PedidoItem.Id).ToList();
				if (pedidoItemCompra.Count > 0)
					throw new NotFoundException(string.Format("Item {0} não pode ser editado, já existe compra vinculada.", request.PedidoItem.Id));

				var existPedidoItem = await pedidoItemRepository.GetById(request.PedidoItem.Id, cancellationToken);
				if (existPedidoItem == null)
					throw new BadRequestException("id pedido item não encontrado!");

				var existPedido = await pedidoRepository.GetByIdPedidoWithOutRoles(request.PedidoItem.IdPedido, cancellationToken);
				if (existPedido == null)
					throw new BadRequestException("id pedido não encontrado!");

				request.PedidoItem.Pedido = existPedido;

				if (request.PedidoItem.IdStatus == 0)
				{
					if (existPedidoItem.IdStatus == 0)
						if (string.IsNullOrWhiteSpace(existPedidoItem.Acordo))
						{
							request.PedidoItem.IdStatus = (int)StatusItem.ITEM_SOLICITACAODECOMPRACRIADA;
							statusChange = true;
						}
						else
						{
							request.PedidoItem.IdStatus = (int)StatusItem.ITEM_SOLICITACAODECOMPRAENVIADA;
							statusChange = true;
						}
					else
						request.PedidoItem.IdStatus = existPedidoItem.IdStatus;

				}
				else
					statusChange = true;

				if (request.PedidoItem.DataNecessidade == null)
				{
					if (existPedidoItem.DataNecessidade != null)
						request.PedidoItem.DataNecessidade = existPedidoItem.DataNecessidade;
					else
						if (request.PedidoItem.Pedido.DataNecessidade != null)
						request.PedidoItem.DataNecessidade = request.PedidoItem.Pedido.DataNecessidade;
				}

				if (!string.IsNullOrWhiteSpace(existPedidoItem.LoginComprador) && existPedidoItem.UserComprador != null)
				{
					request.PedidoItem.LoginComprador = existPedidoItem.LoginComprador;
					request.PedidoItem.UserComprador = existPedidoItem.UserComprador;
				}
				else if (userProvider.IsRole(Role.PERFIL_BASE_SUPRIMENTOS) && !string.IsNullOrWhiteSpace(request.PedidoItem.LoginComprador))
				{
					request.PedidoItem.IdStatus = (int)StatusItem.ITEM_ATRIBUIDOAOCOMPRADOR;
					statusChange = true;
					request.PedidoItem.UserComprador = await userRepository.GetByLogin(request.PedidoItem.LoginComprador, cancellationToken);
					if (!DBNull.Value.Equals(request.PedidoItem.DataVinculoComprador) || (request.PedidoItem.DataVinculoComprador != existPedidoItem.DataVinculoComprador))
						request.PedidoItem.DataVinculoComprador = DateTime.Now;

					await mediator.Publish(new OnItemAtribuidoComprador() { Pedido = existPedido });
				}
				else if (!userProvider.IsRole(Role.PERFIL_BASE_SUPRIMENTOS) && !string.IsNullOrWhiteSpace(request.PedidoItem.LoginComprador))
					throw new BadRequestException("Somente usuário da base de suprimentos pode vincular um comprador.");

				var status = await statusItemRepository.GetById(request.PedidoItem.IdStatus, cancellationToken);
				if (status == null)
					throw new NotFoundException("status não encontrado!");
				else
					request.PedidoItem.StatusPedidoItem = status;

				var vltotal = request.PedidoItem.Quantidade * request.PedidoItem.Valor;
				request.PedidoItem.ValorItens = vltotal;

				if (!string.IsNullOrWhiteSpace(request.PedidoItem.LoginDevolucao))
					request.PedidoItem.UserDevolucao = await userRepository.GetByLogin(request.PedidoItem.LoginDevolucao, cancellationToken);

				if (!string.IsNullOrWhiteSpace(request.PedidoItem.LoginAprovacao))
					request.PedidoItem.UserAprovacao = await userRepository.GetByLogin(request.PedidoItem.LoginAprovacao, cancellationToken);

				if (!string.IsNullOrWhiteSpace(request.PedidoItem.LoginReprovacao))
					request.PedidoItem.UserReprovacao = await userRepository.GetByLogin(request.PedidoItem.LoginReprovacao, cancellationToken);

				if (!string.IsNullOrWhiteSpace(request.PedidoItem.LoginComprador))
				{
					if (!string.IsNullOrWhiteSpace(existPedidoItem.Acordo))
						throw new BadRequestException("Não é permitido atribuir comprador para itens com acordo.");

					request.PedidoItem.UserComprador = await userRepository.GetByLogin(request.PedidoItem.LoginComprador, cancellationToken);
				}

				if (!string.IsNullOrWhiteSpace(request.PedidoItem.LoginReprovacao))
					request.PedidoItem.UserReprovacao = await userRepository.GetByLogin(request.PedidoItem.LoginReprovacao, cancellationToken);

				request.PedidoItem.Trackings = trackingRepository.GetAll().Where(a => a.IdPedidoItem == request.PedidoItem.Id).ToList();

				pedidoItemRepository.AddOrUpdate(request.PedidoItem, cancellationToken);
				var result = unitOfWork.SaveChanges();
				if (!result) throw new ApplicationException("An error has occured.");

				if (statusChange)
				{
					await mediator.Send(new AddTracking()
					{
						Tracking = new Tracking()
						{
							IdPedidoItem = request.PedidoItem.Id,
							StatusId = request.PedidoItem.IdStatus,
							ChangeById = request.PedidoItem.Pedido.LoginSolicitante
						}
					}, cancellationToken);
				}

				//var pedidoWithOutRoles = pedidoRepository.GetByIdPedidoWithOutRoles(request.PedidoItem.Pedido.Id, cancellationToken).Result;

				await mediator.Publish(new OnVerificarStatus() { Pedido = existPedido });

				await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = request.PedidoItem });
			}
			catch (Exception error)
			{
				throw new ApplicationException(error.Message);
			}
			return Unit.Value;
		}

		async Task<Unit> IRequestHandler<UpdatePedidoItemDevolucao, Unit>.Handle(UpdatePedidoItemDevolucao request, CancellationToken cancellationToken)
		{
			try
			{
				var pedido = await mediator.Send(new GetByIdPedidoWithOutRoles() { Id = request.IdPedido }, cancellationToken);

				if (pedido == null) throw new NotFoundException("Registro do pedido não encontrado.");

				var pedidoItem = await mediator.Send<PedidoItem>(new GetByIdPedidoItem() { Id = request.IdPedidoItem }, cancellationToken);

				if (pedidoItem == null) throw new NotFoundException("Registro do pedido item não encontrado.");

				//var pedidoItemNew = await mediator.Send<PedidoItem>(new GetByIdPedidoItem() { Id = request.IdPedidoItem }, cancellationToken);

				var status = await mediator.Send(new GetByStatusItemId() { Id = (int)Domain.Enums.StatusItem.ITEM_EMANALISE }, cancellationToken);

				if (status == null) throw new NotFoundException("Registro do status não encontrado.");

				if (pedidoItem.QuantidadeDevolvida == 0)
				{
					unitOfWork.BeginTransaction();
					var comprador = pedidoItem.LoginComprador;
					//var pedidoItemCompra = await mediator.Send<List<PedidoItemCompra>>(new ListItemCompraIdByIdPedidoItem() { IdPedido = id, IdPedidoItem = idItem }, cancellationToken);


					if (pedidoItem.QuantidadePendenteCompra != pedidoItem.Quantidade)
					{ //Item com compra
					  // Cria um novo pedido item para a quantidade devolvida.

						if (request.PedidoItemDevolucao.idTipo == 1) throw new NotFoundException("Não é possível devolver o Item pois já tem compra.");

						if (pedidoItem.FlagDevolvidoComprador) throw new BadRequestException("Item já foi devolvido pelo comprador.");

						PedidoItem pedidoItemNew = new PedidoItem();

						pedidoItemNew.Id = 0;

						pedidoItemNew.LoginComprador = "";
						pedidoItemNew.Acordo = pedidoItem.Acordo;
						pedidoItemNew.BUId = pedidoItem.BUId;
						pedidoItemNew.Categoria = pedidoItem.Categoria;
						pedidoItemNew.DataAprovacao = pedidoItem.DataAprovacao;
						pedidoItemNew.DataCancelamento = pedidoItem.DataCancelamento;
						pedidoItemNew.DataDevolucao = pedidoItem.DataDevolucao;
						pedidoItemNew.DataEntregaPrevista = pedidoItem.DataEntregaPrevista;
						pedidoItemNew.DataEdicaoReenvio = pedidoItem.DataEdicaoReenvio;
						pedidoItemNew.DataEntrega = pedidoItem.DataEntrega;
						pedidoItemNew.DataEntregaPrevista = pedidoItem.DataEntregaPrevista;
						pedidoItemNew.DataNecessidade = pedidoItem.DataNecessidade;
						pedidoItemNew.DataReenvio = pedidoItem.DataReenvio;
						pedidoItemNew.DataReprovacao = pedidoItem.DataReprovacao;
						pedidoItemNew.DataVinculoComprador = null;
						pedidoItemNew.DataVisualizacaoComprador = null;
						pedidoItemNew.Descricao = pedidoItem.Descricao;
						pedidoItemNew.Fabricante = pedidoItem.Fabricante;
						pedidoItemNew.Fornecedor = pedidoItem.Fornecedor;
						pedidoItemNew.HeadId = pedidoItem.HeadId;
						pedidoItemNew.IdItemOracle = pedidoItem.IdItemOracle;
						pedidoItemNew.IdTipo = pedidoItem.IdTipo;
						pedidoItemNew.ItemFabricante = pedidoItem.ItemFabricante;
						pedidoItemNew.ItemKeyOracle = pedidoItem.ItemKeyOracle;
						pedidoItemNew.Justificativa = pedidoItem.Justificativa;
						pedidoItemNew.JustificativaCancelamento = pedidoItem.JustificativaCancelamento;
						pedidoItemNew.JustificativaDevolucao = pedidoItem.JustificativaDevolucao;
						pedidoItemNew.LineId = pedidoItem.LineId;
						pedidoItemNew.LocalEntrega = pedidoItem.LocalEntrega;
						pedidoItemNew.LoginAprovacao = "";
						pedidoItemNew.LoginCancelamento = pedidoItem.LoginCancelamento;
						pedidoItemNew.LoginComprador = "";
						pedidoItemNew.LoginDevolucao = "";
						pedidoItemNew.LoginReprovacao = "";
						pedidoItemNew.MarcacaoCena = pedidoItem.MarcacaoCena;
						pedidoItemNew.NomeItem = pedidoItem.NomeItem;
						pedidoItemNew.Observacao = pedidoItem.Observacao;
						pedidoItemNew.ObservacaoAprovacaoCompra = pedidoItem.ObservacaoAprovacaoCompra;
						pedidoItemNew.RcPedido = pedidoItem.RcPedido;
						pedidoItemNew.Referencias = pedidoItem.Referencias;
						pedidoItemNew.solicitacaoDirigida = pedidoItem.solicitacaoDirigida;
						pedidoItemNew.SugestaoFornecedor = pedidoItem.SugestaoFornecedor;
						pedidoItemNew.UnidadeMedida = pedidoItem.UnidadeMedida;
						pedidoItemNew.Valor = pedidoItem.Valor;
						pedidoItemNew.ValorItens = pedidoItem.ValorItens;
						pedidoItemNew.ValorUnitario = pedidoItem.ValorUnitario;

						pedidoItemNew.Quantidade = pedidoItem.QuantidadePendenteCompra;
						pedidoItemNew.QuantidadeAprovacaoCompra = 0;
						pedidoItemNew.QuantidadePendenteCompra = 0;
						pedidoItemNew.QuantidadeComprada = 0;
						pedidoItemNew.QuantidadeDevolvida = 0;
						pedidoItemNew.QuantidadeEntregue = 0;
						pedidoItemNew.QuantidadePendenteEntrega = 0;

						pedidoItemNew.ValorItens = pedidoItemNew.Quantidade * pedidoItemNew.Valor;
						pedidoItemNew.IdStatus = (int)Domain.Enums.StatusItem.ITEM_EMANALISE;
						pedidoItemNew.StatusPedidoItem = status;
						pedidoItemNew.IdPedido = pedidoItem.IdPedido;
						pedidoItemNew.Pedido = pedido;
						pedidoItemNew.Compras = null;
						pedidoItemNew.Entregas = null;
						pedidoItemNew.UserComprador = null;
						pedidoItemNew.Devolucao = null;
						pedidoItemNew.Arquivos = pedidoItem.Arquivos;
						pedidoItemNew.PedidoItemConversas = null;
						pedidoItemNew.Trackings = null;
						pedidoItemNew.UserDevolucao = null;
						pedidoItemNew.UserAprovacao = null;
						pedidoItemNew.UserCancelamento = null;
						pedidoItemNew.UserReprovacao = null;

						await mediator.Send(new SavePedidoItem()
						{
							PedidoItem = pedidoItemNew
						}, cancellationToken);

						// var result = mapper.Map<PedidoItemViewModel>(pedidoItemNew);

						pedidoItem = await mediator.Send<PedidoItem>(new GetByIdPedidoItem() { Id = request.IdPedidoItem }, cancellationToken);

						pedidoItem.DataDevolucao = DateTime.Now;

						//pedidoItem.LoginDevolucao = userProvider.User.Login;

						//pedidoItem.JustificativaDevolucao = request.PedidoItemDevolucao.Justificativa;

						pedidoItem.FlagDevolvidoComprador = true;

						//var userDevolucao = await mediator.Send(new GetByLogin() { Login = pedidoItem.LoginDevolucao }, cancellationToken);

						//pedidoItem.UserDevolucao = userDevolucao;

						pedidoItem.QuantidadeDevolvida = pedidoItem.QuantidadePendenteCompra;

						pedidoItem.Compras = await mediator.Send<List<PedidoItemCompra>>(new ListByIdPedidoItemCompra() { IdPedidoItem = request.IdPedidoItem }, cancellationToken);

						pedidoItem.Entregas = await mediator.Send<List<PedidoItemEntrega>>(new ListByIdPedidoItemEntrega() { IdPedidoItem = request.IdPedidoItem }, cancellationToken);

						pedidoItemRepository.AddOrUpdate(pedidoItem, cancellationToken);

						var result = unitOfWork.SaveChanges();

						if (!result) throw new ApplicationException("An error has occured.");

						request.PedidoItemDevolucao.IdPedidoItem = pedidoItemNew.Id;
						request.PedidoItemDevolucao.IdPedidoItemOriginal = pedidoItem.Id;
						request.PedidoItemDevolucao.Quantidade = pedidoItem.QuantidadePendenteCompra;
						request.PedidoItemDevolucao.Comprador = comprador;

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
						if (pedidoItem.FlagDevolvidoComprador) throw new BadRequestException("Item já foi devolvido pelo comprador.");

						if (request.PedidoItemDevolucao.idTipo == 2)
						{
							if (userProvider.User.Login == pedidoItem.LoginComprador)
							{
								pedidoItem.IdStatus = (int)StatusItem.ITEM_EMANALISE;

								pedidoItem.StatusPedidoItem = status;

								pedidoItem.LoginComprador = "";

								pedidoItem.QuantidadeDevolvida = pedidoItem.QuantidadePendenteCompra;

								pedidoItem.JustificativaDevolucao = "";

								pedidoItem.LoginDevolucao = null;

								pedidoItem.UserDevolucao = null;

								pedidoItem.DataDevolucao = null;

								pedidoItem.FlagDevolvidoComprador = true;

								pedidoItem.FlagDevolvidoBase = false;
							}
							else
								throw new BadRequestException("Somente o comprador pode devolver o pedido para a base.");
						}
						else
						{
							status = await mediator.Send(new GetByStatusItemId() { Id = (int)Domain.Enums.StatusItem.ITEM_DEVOLUCAO }, cancellationToken);

							if (status == null) throw new NotFoundException("Registro do status não encontrado.");

							pedidoItem.IdStatus = (int)StatusItem.ITEM_DEVOLUCAO;

							pedidoItem.StatusPedidoItem = status;

							pedidoItem.QuantidadeDevolvida = 0;

							pedidoItem.JustificativaDevolucao = request.PedidoItemDevolucao.Justificativa;

							pedidoItem.LoginDevolucao = userProvider.User.Login;

							var userDevolucao = await mediator.Send(new GetByLogin() { Login = pedidoItem.LoginDevolucao }, cancellationToken);

							pedidoItem.UserDevolucao = userDevolucao;

							pedidoItem.DataDevolucao = DateTime.Now;

							if (userProvider.User.Login == pedidoItem.LoginComprador)
							{
								pedidoItem.FlagDevolvidoComprador = true;
								pedidoItem.FlagDevolvidoBase = false;
							}
							else
							{
								pedidoItem.FlagDevolvidoComprador = false;
								pedidoItem.FlagDevolvidoBase = true;
							}

							await mediator.Publish(new OnDevolucaoPedidoItem() { PedidoItem = pedidoItem });

						}

						request.PedidoItemDevolucao.IdPedidoItem = pedidoItem.Id;

						request.PedidoItemDevolucao.IdPedidoItemOriginal = pedidoItem.Id;

						request.PedidoItemDevolucao.Quantidade = pedidoItem.QuantidadePendenteCompra;

						request.PedidoItemDevolucao.Comprador = comprador;

						pedidoItem.Compras = await mediator.Send<List<PedidoItemCompra>>(new ListByIdPedidoItemCompra() { IdPedidoItem = request.IdPedidoItem }, cancellationToken);

						pedidoItem.Entregas = await mediator.Send<List<PedidoItemEntrega>>(new ListByIdPedidoItemEntrega() { IdPedidoItem = request.IdPedidoItem }, cancellationToken);


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

					unitOfWork.CommitTransaction();

					await mediator.Publish(new OnVerificarStatus() { Pedido = pedido });

					await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = pedidoItem });

				}
				else throw new NotFoundException("Não é possível devolver o Item pois o mesmo já foi devolvido.");
			}
			catch (Exception error)
			{
				unitOfWork.RollbackTransaction();
				throw new ApplicationException(error.Message);
			}

			return Unit.Value;
		}

		async Task<Unit> IRequestHandler<UpdatePedidoItensDevolucao, Unit>.Handle(UpdatePedidoItensDevolucao request, CancellationToken cancellationToken)
		{
			try
			{
				var pedido = await mediator.Send(new GetByIdPedidoWithOutRoles() { Id = request.IdPedido }, cancellationToken);

				if (pedido == null) throw new NotFoundException("Registro do pedido não encontrado.");

				var pedidoItens = pedidoItemRepository.GetAll().Where(a => a.IdPedido == pedido.Id).ToList();
				if (pedidoItens.Count > 0)
				{
					unitOfWork.BeginTransaction();

					foreach (var currentPedidoItem in pedidoItens)
					{
						var pedidoItem = await mediator.Send<PedidoItem>(new GetByIdPedidoItem() { Id = currentPedidoItem.Id }, cancellationToken);

						if (pedidoItem == null) throw new NotFoundException("Registro do pedido item não encontrado.");

						//var pedidoItemNew = await mediator.Send<PedidoItem>(new GetByIdPedidoItem() { Id = request.IdPedidoItem }, cancellationToken);

						var status = await mediator.Send(new GetByStatusItemId() { Id = (int)Domain.Enums.StatusItem.ITEM_EMANALISE }, cancellationToken);

						if (status == null) throw new NotFoundException("Registro do status não encontrado.");

						var comprador = pedidoItem.LoginComprador;
						//var pedidoItemCompra = await mediator.Send<List<PedidoItemCompra>>(new ListItemCompraIdByIdPedidoItem() { IdPedido = id, IdPedidoItem = idItem }, cancellationToken);
						if (pedidoItem.QuantidadeDevolvida == 0)
						{
							if (pedidoItem.QuantidadePendenteCompra != pedidoItem.Quantidade)
							{ //Item com compra
							  // Cria um novo pedido item para a quantidade devolvida.

								PedidoItem pedidoItemNew = new PedidoItem();

								pedidoItemNew.Id = 0;

								pedidoItemNew.LoginComprador = "";
								pedidoItemNew.Acordo = pedidoItem.Acordo;
								pedidoItemNew.BUId = pedidoItem.BUId;
								pedidoItemNew.Categoria = pedidoItem.Categoria;
								pedidoItemNew.DataAprovacao = pedidoItem.DataAprovacao;
								pedidoItemNew.DataCancelamento = pedidoItem.DataCancelamento;
								pedidoItemNew.DataDevolucao = pedidoItem.DataDevolucao;
								pedidoItemNew.DataEntregaPrevista = pedidoItem.DataEntregaPrevista;
								pedidoItemNew.DataEdicaoReenvio = pedidoItem.DataEdicaoReenvio;
								pedidoItemNew.DataEntrega = pedidoItem.DataEntrega;
								pedidoItemNew.DataEntregaPrevista = pedidoItem.DataEntregaPrevista;
								pedidoItemNew.DataNecessidade = pedidoItem.DataNecessidade;
								pedidoItemNew.DataReenvio = pedidoItem.DataReenvio;
								pedidoItemNew.DataReprovacao = pedidoItem.DataReprovacao;
								pedidoItemNew.DataVinculoComprador = null;
								pedidoItemNew.DataVisualizacaoComprador = null;
								pedidoItemNew.Descricao = pedidoItem.Descricao;
								pedidoItemNew.Fabricante = pedidoItem.Fabricante;
								pedidoItemNew.Fornecedor = pedidoItem.Fornecedor;
								pedidoItemNew.HeadId = pedidoItem.HeadId;
								pedidoItemNew.IdItemOracle = pedidoItem.IdItemOracle;
								pedidoItemNew.IdTipo = pedidoItem.IdTipo;
								pedidoItemNew.ItemFabricante = pedidoItem.ItemFabricante;
								pedidoItemNew.ItemKeyOracle = pedidoItem.ItemKeyOracle;
								pedidoItemNew.Justificativa = pedidoItem.Justificativa;
								pedidoItemNew.JustificativaCancelamento = pedidoItem.JustificativaCancelamento;
								pedidoItemNew.JustificativaDevolucao = pedidoItem.JustificativaDevolucao;
								pedidoItemNew.LineId = pedidoItem.LineId;
								pedidoItemNew.LocalEntrega = pedidoItem.LocalEntrega;
								pedidoItemNew.LoginAprovacao = "";
								pedidoItemNew.LoginCancelamento = pedidoItem.LoginCancelamento;
								pedidoItemNew.LoginComprador = "";
								pedidoItemNew.LoginDevolucao = "";
								pedidoItemNew.LoginReprovacao = "";
								pedidoItemNew.MarcacaoCena = pedidoItem.MarcacaoCena;
								pedidoItemNew.NomeItem = pedidoItem.NomeItem;
								pedidoItemNew.Observacao = pedidoItem.Observacao;
								pedidoItemNew.ObservacaoAprovacaoCompra = pedidoItem.ObservacaoAprovacaoCompra;
								pedidoItemNew.RcPedido = pedidoItem.RcPedido;
								pedidoItemNew.Referencias = pedidoItem.Referencias;
								pedidoItemNew.solicitacaoDirigida = pedidoItem.solicitacaoDirigida;
								pedidoItemNew.SugestaoFornecedor = pedidoItem.SugestaoFornecedor;
								pedidoItemNew.UnidadeMedida = pedidoItem.UnidadeMedida;
								pedidoItemNew.Valor = pedidoItem.Valor;
								pedidoItemNew.ValorItens = pedidoItem.ValorItens;
								pedidoItemNew.ValorUnitario = pedidoItem.ValorUnitario;


								pedidoItemNew.Quantidade = pedidoItem.QuantidadePendenteCompra;
								pedidoItemNew.QuantidadeAprovacaoCompra = 0;
								pedidoItemNew.QuantidadePendenteCompra = 0;
								pedidoItemNew.QuantidadeComprada = 0;
								pedidoItemNew.QuantidadeDevolvida = 0;
								pedidoItemNew.QuantidadeEntregue = 0;
								pedidoItemNew.QuantidadePendenteEntrega = 0;

								pedidoItemNew.ValorItens = pedidoItemNew.Quantidade * pedidoItemNew.Valor;
								pedidoItemNew.IdStatus = (int)Domain.Enums.StatusItem.ITEM_EMANALISE;
								pedidoItemNew.StatusPedidoItem = status;
								pedidoItemNew.IdPedido = pedidoItem.IdPedido;
								pedidoItemNew.Pedido = pedido;
								pedidoItemNew.Compras = null;
								pedidoItemNew.Entregas = null;
								pedidoItemNew.UserComprador = null;
								pedidoItemNew.Devolucao = null;
								pedidoItemNew.Arquivos = pedidoItem.Arquivos;
								pedidoItemNew.PedidoItemConversas = null;
								pedidoItemNew.Trackings = null;
								pedidoItemNew.UserDevolucao = null;
								pedidoItemNew.UserAprovacao = null;
								pedidoItemNew.UserCancelamento = null;
								pedidoItemNew.UserReprovacao = null;

								await mediator.Send(new SavePedidoItem()
								{
									PedidoItem = pedidoItemNew
								}, cancellationToken);

								// var result = mapper.Map<PedidoItemViewModel>(pedidoItemNew);

								pedidoItem = await mediator.Send<PedidoItem>(new GetByIdPedidoItem() { Id = currentPedidoItem.Id }, cancellationToken);

								pedidoItem.QuantidadeDevolvida = pedidoItem.QuantidadePendenteCompra;

								pedidoItem.JustificativaDevolucao = request.PedidoItemDevolucao.Justificativa;

								pedidoItem.DataDevolucao = DateTime.Now;

								pedidoItem.LoginDevolucao = userProvider.User.Login;

								var userDevolucao = await mediator.Send(new GetByLogin() { Login = pedidoItem.LoginDevolucao }, cancellationToken);

								pedidoItem.UserDevolucao = userDevolucao;

								pedidoItem.Compras = await mediator.Send<List<PedidoItemCompra>>(new ListByIdPedidoItemCompra() { IdPedidoItem = currentPedidoItem.Id }, cancellationToken);

								pedidoItem.Entregas = await mediator.Send<List<PedidoItemEntrega>>(new ListByIdPedidoItemEntrega() { IdPedidoItem = currentPedidoItem.Id }, cancellationToken);

								pedidoItemRepository.AddOrUpdate(pedidoItem, cancellationToken);

								var result = unitOfWork.SaveChanges();

								if (!result) throw new ApplicationException("An error has occured.");

								request.PedidoItemDevolucao.IdPedidoItem = pedidoItemNew.Id;
								request.PedidoItemDevolucao.IdPedidoItemOriginal = pedidoItem.Id;
								request.PedidoItemDevolucao.Quantidade = pedidoItem.QuantidadePendenteCompra;
								request.PedidoItemDevolucao.Comprador = comprador;

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

								request.PedidoItemDevolucao.IdPedidoItem = pedidoItem.Id;
								request.PedidoItemDevolucao.IdPedidoItemOriginal = pedidoItem.Id;
								request.PedidoItemDevolucao.Quantidade = pedidoItem.QuantidadePendenteCompra;
								request.PedidoItemDevolucao.Comprador = comprador;

								pedidoItem.IdStatus = (int)StatusItem.ITEM_EMANALISE;
								pedidoItem.StatusPedidoItem = status;
								pedidoItem.LoginComprador = "";
								pedidoItem.QuantidadeDevolvida = 0;

								pedidoItem.JustificativaDevolucao = request.PedidoItemDevolucao.Justificativa;

								pedidoItem.DataDevolucao = DateTime.Now;

								pedidoItem.LoginDevolucao = userProvider.User.Login;

								var userDevolucao = await mediator.Send(new GetByLogin() { Login = pedidoItem.LoginDevolucao }, cancellationToken);

								pedidoItem.UserDevolucao = userDevolucao;

								pedidoItem.Compras = await mediator.Send<List<PedidoItemCompra>>(new ListByIdPedidoItemCompra() { IdPedidoItem = currentPedidoItem.Id }, cancellationToken);

								pedidoItem.Entregas = await mediator.Send<List<PedidoItemEntrega>>(new ListByIdPedidoItemEntrega() { IdPedidoItem = currentPedidoItem.Id }, cancellationToken);

								pedidoItemRepository.AddOrUpdate(pedidoItem, cancellationToken);

								var result = unitOfWork.SaveChanges();

								if (!result) throw new ApplicationException("An error has occured.");


								pedidoItemDevolucaoRepository.AddOrUpdate(request.PedidoItemDevolucao, cancellationToken);

								var resultDev = unitOfWork.SaveChanges();

								if (!resultDev) throw new ApplicationException("An error has occured.");

							}
						}
					}

					pedido.LoginDevolucao = userProvider.User.Login;
					var userDev = await mediator.Send(new GetByLogin() { Login = userProvider.User.Login }, cancellationToken);
					pedido.UserDevolucao = userDev;
					pedido.DataDevolucao = DateTime.Now;
					pedido.JustificativaDevolucao = request.PedidoItemDevolucao.Justificativa;
					pedidoRepository.AddOrUpdate(pedido, cancellationToken);

					var resultPedido = unitOfWork.SaveChanges();

					if (!resultPedido) throw new ApplicationException("An error has occured.");


					unitOfWork.CommitTransaction();
				}
			}
			catch (Exception error)
			{
				unitOfWork.RollbackTransaction();
				throw new ApplicationException(error.Message);
			}

			return Unit.Value;
		}

		async Task<Unit> IRequestHandler<UpdatePedidoItens, Unit>.Handle(UpdatePedidoItens request, CancellationToken cancellationToken)
		{
			try
			{
				RunEntityValidationList(request.PedidoItens);

				unitOfWork.BeginTransaction();

				bool statusChange = false;

				foreach (var currentPedidoItem in request.PedidoItens)
				{
					var pedidoItemCompra = pedidoItemCompraRepository.GetAll().Where(a => a.IdPedidoItem == currentPedidoItem.Id).ToList();
					if (pedidoItemCompra.Count > 0)
						throw new NotFoundException(string.Format("Item {0} não pode ser editado, já existe compra vinculada.", currentPedidoItem.Id));

					statusChange = false;

					var pedidoItem = await pedidoItemRepository.GetById(currentPedidoItem.Id, cancellationToken);

					if (pedidoItem == null)
						throw new NotFoundException("id pedido item não encontrado!");

					if (pedidoItem.IdPedido > 0)
						currentPedidoItem.Pedido = await pedidoRepository.GetByIdPedidoWithOutRoles(pedidoItem.IdPedido, cancellationToken);

					if (currentPedidoItem.IdStatus == 0)
					{
						if (pedidoItem.IdStatus == 0)
							if (string.IsNullOrWhiteSpace(pedidoItem.Acordo))
							{
								currentPedidoItem.IdStatus = (int)StatusItem.ITEM_SOLICITACAODECOMPRACRIADA;
								statusChange = true;
							}
							else
							{
								currentPedidoItem.IdStatus = (int)StatusItem.ITEM_SOLICITACAODECOMPRAENVIADA;
								statusChange = true;
							}
						else
							currentPedidoItem.IdStatus = pedidoItem.IdStatus;
					}
					else
						statusChange = true;


					if (currentPedidoItem.DataNecessidade == null)
					{
						if (pedidoItem.DataNecessidade != null)
							currentPedidoItem.DataNecessidade = pedidoItem.DataNecessidade;
						else
							if (currentPedidoItem.Pedido.DataNecessidade != null)
							currentPedidoItem.DataNecessidade = currentPedidoItem.Pedido.DataNecessidade;
					}


					if (!string.IsNullOrWhiteSpace(currentPedidoItem.LoginComprador) && (currentPedidoItem.LoginComprador != pedidoItem.LoginComprador))
					{
						if (userProvider.User.Login == currentPedidoItem.Pedido.LoginBase)
						{
							if (currentPedidoItem.IdTipo == 0)
								throw new NotFoundException("O Tipo do pedido não pode ser nulo quando o comprador é atribuído");

							currentPedidoItem.IdStatus = (int)StatusItem.ITEM_ATRIBUIDOAOCOMPRADOR;

							statusChange = true;

							currentPedidoItem.UserComprador = await userRepository.GetByLogin(currentPedidoItem.LoginComprador, cancellationToken);

							if (!DBNull.Value.Equals(currentPedidoItem.DataVinculoComprador) && (currentPedidoItem.DataVinculoComprador != pedidoItem.DataVinculoComprador))
								currentPedidoItem.DataVinculoComprador = DateTime.Now;

							if (currentPedidoItem.IdTipo == 0 || currentPedidoItem.IdTipo > 2)
								throw new BadRequestException("É necessário informar o tipo para atribuir um comprador.");


							await mediator.Publish(new OnItemAtribuidoComprador() { Pedido = pedidoItem.Pedido });

						}
						else
							throw new BadRequestException("Somente usuário da base de suprimentos pode vincular um comprador.");
					}

					var status = await statusItemRepository.GetById(currentPedidoItem.IdStatus, cancellationToken);
					if (status == null)
						throw new NotFoundException("status não encontrado!");
					else
						currentPedidoItem.StatusPedidoItem = status;


					var vltotal = currentPedidoItem.Quantidade * currentPedidoItem.Valor;
					currentPedidoItem.ValorItens = vltotal;

					if (!string.IsNullOrWhiteSpace(currentPedidoItem.LoginDevolucao))
						currentPedidoItem.UserDevolucao = await userRepository.GetByLogin(currentPedidoItem.LoginDevolucao, cancellationToken);

					if (!string.IsNullOrWhiteSpace(currentPedidoItem.LoginAprovacao))
						currentPedidoItem.UserAprovacao = await userRepository.GetByLogin(currentPedidoItem.LoginAprovacao, cancellationToken);

					if (!string.IsNullOrWhiteSpace(currentPedidoItem.LoginReprovacao))
						currentPedidoItem.UserReprovacao = await userRepository.GetByLogin(currentPedidoItem.LoginReprovacao, cancellationToken);

					var arquivos = pedidoItemAnexosRepository
						.GetAll().Where(a => a.IdPedidoItem == pedidoItem.Id).ToList();

					pedidoItemAnexosRepository.Remove(arquivos);

					pedidoItemRepository.AddOrUpdate(currentPedidoItem, cancellationToken);

					var result = unitOfWork.SaveChanges();

					if (!result) throw new ApplicationException("An error has occured.");

					if (statusChange)
					{
						await mediator.Send(new AddTracking()
						{
							Tracking = new Tracking()
							{
								IdPedidoItem = currentPedidoItem.Id,
								StatusId = currentPedidoItem.IdStatus,
								ChangeById = currentPedidoItem.Pedido.LoginSolicitante
							}
						}, cancellationToken);
					}

					await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = currentPedidoItem });
				}
				unitOfWork.CommitTransaction();

				var pedidoWithOutRoles = pedidoRepository.GetByIdPedidoWithOutRoles(request.PedidoItens[0].Pedido.Id, cancellationToken).Result;

				await mediator.Publish(new OnVerificarStatus() { Pedido = pedidoWithOutRoles });

			}
			catch (Exception error)
			{
				unitOfWork.RollbackTransaction();
				throw new ApplicationException(error.Message);
			}
			return await Unit.Task;
		}

		async Task<Unit> IRequestHandler<DeletePedidoItem, Unit>.Handle(DeletePedidoItem request, CancellationToken cancellationToken)
		{
			var pedidoItemCompra = pedidoItemCompraRepository.GetAll().Where(a => a.IdPedidoItem == request.IdPedidoItem).ToList();
			if (pedidoItemCompra.Count > 0)
				throw new NotFoundException(string.Format("Item {0} não pode ser editado, já existe compra vinculada.", request.IdPedidoItem));

			var pedido = await pedidoRepository.GetByIdPedidoWithOutRoles(request.IdPedido, cancellationToken);
			if (pedido == null)
				throw new NotFoundException("pedido não encontrado!");

			var pedidoItem = await pedidoItemRepository.GetById(request.IdPedidoItem, cancellationToken);
			if (pedidoItem == null)
				throw new NotFoundException("id pedido item não encontrado!");

			pedidoItemRepository.Remove(pedidoItem);

			//pedido.NroItens = pedido.Itens.Sum(a => a.Quantidade);

			var result = unitOfWork.SaveChanges();
			if (!result) throw new ApplicationException("An error has occured.");

			//await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = pedidoItem });

			return Unit.Value;
		}

		Task<Unit> IRequestHandler<DeleteAllPedidoItem, Unit>.Handle(DeleteAllPedidoItem request, CancellationToken cancellationToken)
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

		async Task<Unit> IRequestHandler<SavePedidoItem, Unit>.Handle(SavePedidoItem request, CancellationToken cancellationToken)
		{
			try
			{
				RunEntityValidation(request.PedidoItem);
				/*
				 * 
				 * Verifica a existência do pedido
				 *
				 */
				bool statusChange = false;

				var pedido = await pedidoRepository.GetByIdPedidoWithOutRoles(request.PedidoItem.IdPedido, cancellationToken);
				if (pedido == null)
					throw new NotFoundException("id pedido não encontrado!");
				else
					request.PedidoItem.Pedido = pedido;


				/*
				 * 
				 * Bloco para verificação de pedido item e caso existe aplica update senão insert
				 * Se o status não foi passado como parametro atribui o que está na base de dados caso o item existe, caso contrário inserir o status inicial de acordo com o acordo
				 *
				 */
				if (request.PedidoItem.Id > 0)
				{
					var existPedidoItem = await pedidoItemRepository.GetById(request.PedidoItem.Id, cancellationToken);

					if (existPedidoItem == null)
						throw new NotFoundException("id pedido item não encontrado!");

					if (request.PedidoItem.IdStatus == 0)
					{
						if (existPedidoItem.IdStatus > 0)
							request.PedidoItem.IdStatus = existPedidoItem.IdStatus;
						else
						{
							if (string.IsNullOrWhiteSpace(existPedidoItem.Acordo))
							{
								request.PedidoItem.IdStatus = (int)StatusItem.ITEM_SOLICITACAODECOMPRACRIADA;
								statusChange = true;

								if (!string.IsNullOrWhiteSpace(pedido.LoginBase))
									request.PedidoItem.IdStatus = (int)StatusItem.ITEM_EMANALISE;

								if (string.IsNullOrWhiteSpace(existPedidoItem.LoginComprador))
								{
									request.PedidoItem.IdStatus = (int)StatusItem.ITEM_ATRIBUIDOAOCOMPRADOR;
								}
							}
							else
							{
								request.PedidoItem.IdStatus = (int)StatusItem.ITEM_SOLICITACAODECOMPRAENVIADA;
								statusChange = true;
							}
						}
					}
					else
						statusChange = true;
				}
				else
				{
					if (request.PedidoItem.Quantidade == 0)
						throw new ApplicationException("A quantidade do Item está zerado!");

					//if (request.PedidoItem.Valor == 0)
					//	throw new ApplicationException("O valor do Item está zerado!");

					//BUG 60948: adicionar o local de entrega do pedido no item.
					request.PedidoItem.LocalEntrega = pedido.LocalEntrega;

					request.PedidoItem.QuantidadePendenteCompra = request.PedidoItem.Quantidade;

					if (request.PedidoItem.IdStatus == 0)
					{
						if (string.IsNullOrWhiteSpace(request.PedidoItem.Acordo))
						{
							request.PedidoItem.IdStatus = (int)StatusItem.ITEM_SOLICITACAODECOMPRACRIADA;
							statusChange = true;

							if (!string.IsNullOrWhiteSpace(pedido.LoginBase))
							{
								request.PedidoItem.IdStatus = (int)StatusItem.ITEM_EMANALISE;
							}
						}
						else
						{
							request.PedidoItem.IdStatus = (int)StatusItem.ITEM_SOLICITACAODECOMPRAENVIADA;
							statusChange = true;
						}
					}
					else
						statusChange = true;
				}


				if (!string.IsNullOrWhiteSpace(request.PedidoItem.LoginComprador))
				{
					if (request.PedidoItem.IdTipo == 0)
						throw new NotFoundException("O Tipo do pedido não pode ser nulo quando o comprador é atribuído");

					if (request.PedidoItem.Id > 0)
					{
						var existPedidoItem = await pedidoItemRepository.GetById(request.PedidoItem.Id, cancellationToken);

						if (existPedidoItem == null)
							throw new NotFoundException("id pedido item não encontrado!");

						if (request.PedidoItem.LoginComprador != existPedidoItem.LoginComprador)
						{
							request.PedidoItem.IdStatus = (int)StatusItem.ITEM_ATRIBUIDOAOCOMPRADOR;

							statusChange = true;
						}

						if (!DBNull.Value.Equals(request.PedidoItem.DataVinculoComprador) || (request.PedidoItem.DataVinculoComprador != existPedidoItem.DataVinculoComprador))
							request.PedidoItem.DataVinculoComprador = DateTime.Now;
					}
					else
					{
						request.PedidoItem.IdStatus = (int)StatusItem.ITEM_ATRIBUIDOAOCOMPRADOR;

						statusChange = true;

						if (!DBNull.Value.Equals(request.PedidoItem.DataVinculoComprador))
							request.PedidoItem.DataVinculoComprador = DateTime.Now;
					}

					request.PedidoItem.UserComprador = await userRepository.GetByLogin(request.PedidoItem.LoginComprador, cancellationToken);

					await mediator.Publish(new OnItemAtribuidoComprador() { Pedido = pedido });
				}


				if (request.PedidoItem.DataNecessidade == null)
				{
					if (pedido.DataNecessidade != null)
						request.PedidoItem.DataNecessidade = pedido.DataNecessidade;
				}

				var status = await statusItemRepository.GetById(request.PedidoItem.IdStatus, cancellationToken);
				if (status == null)
					throw new NotFoundException("status não encontrado!");
				else
					request.PedidoItem.StatusPedidoItem = status;

				var vltotal = request.PedidoItem.Quantidade * request.PedidoItem.Valor;
				request.PedidoItem.ValorItens = vltotal;


				pedidoItemRepository.AddOrUpdate(request.PedidoItem, cancellationToken);
				var result = unitOfWork.SaveChanges();
				if (!result) throw new ApplicationException("An error has occured.");

				if (request.PedidoItem.Arquivos.Count() > 0)
				{
					foreach (var anexos in request.PedidoItem.Arquivos)
					{
						anexos.IdPedidoItem = request.PedidoItem.Id;
						anexos.PedidoItem = request.PedidoItem;
						pedidoItemAnexosRepository.AddOrUpdate(anexos, cancellationToken);
						result = unitOfWork.SaveChanges();
					}
				}

				if (statusChange)
				{
					if (request.PedidoItem.IdStatus == (int)StatusItem.ITEM_EMANALISE || request.PedidoItem.IdStatus == (int)StatusItem.ITEM_ATRIBUIDOAOCOMPRADOR)
					{
						var existTracking = trackingRepository.GetAll().Where(i => i.IdPedidoItem == request.PedidoItem.Id &&
						i.StatusId == (int)StatusItem.ITEM_SOLICITACAODECOMPRACRIADA).ToList();

						if (existTracking.Count == 0)
						{
							await mediator.Send(new AddTracking()
							{
								Tracking = new Tracking()
								{
									IdPedidoItem = request.PedidoItem.Id,
									StatusId = (int)StatusItem.ITEM_SOLICITACAODECOMPRACRIADA,
									ChangeById = request.PedidoItem.Pedido.LoginSolicitante
								}
							}, cancellationToken);
						}
					}

					await mediator.Send(new AddTracking()
					{
						Tracking = new Tracking()
						{
							IdPedidoItem = request.PedidoItem.Id,
							StatusId = request.PedidoItem.IdStatus,
							ChangeById = request.PedidoItem.Pedido.LoginSolicitante
						}
					}, cancellationToken);
				}

				await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = request.PedidoItem });
			}
			catch (Exception error)
			{
				throw new Exception(error.Message);
			}
			return Unit.Value;
		}

		async Task<Unit> IRequestHandler<SavePedidoItens, Unit>.Handle(SavePedidoItens request, CancellationToken cancellationToken)
		{
			try
			{
				RunEntityValidationList(request.PedidoItens);

				unitOfWork.BeginTransaction();

				bool statusChange = false;

				foreach (var currentPedidoItem in request.PedidoItens)
				{

					if (currentPedidoItem.IdPedido > 0)
						currentPedidoItem.Pedido
							= await pedidoRepository.GetByIdPedidoWithOutRoles(currentPedidoItem.IdPedido, cancellationToken);

					if (currentPedidoItem.Id > 0)
					{
						var existPedidoItem = await pedidoItemRepository.GetById(currentPedidoItem.Id, cancellationToken);

						if (existPedidoItem == null)
							throw new NotFoundException("id pedido item não encontrado!");

						if (currentPedidoItem.IdStatus == 0)
						{
							if (existPedidoItem.IdStatus > 0)
								currentPedidoItem.IdStatus = existPedidoItem.IdStatus;
							else
							{
								if (string.IsNullOrWhiteSpace(existPedidoItem.Acordo))
								{
									currentPedidoItem.IdStatus = (int)StatusItem.ITEM_SOLICITACAODECOMPRACRIADA;
									statusChange = true;

									if (!string.IsNullOrWhiteSpace(currentPedidoItem.Pedido.LoginBase))
										currentPedidoItem.IdStatus = (int)StatusItem.ITEM_EMANALISE;

									if (string.IsNullOrWhiteSpace(existPedidoItem.LoginComprador))
										currentPedidoItem.IdStatus = (int)StatusItem.ITEM_ATRIBUIDOAOCOMPRADOR;
								}
								else
								{
									currentPedidoItem.IdStatus = (int)StatusItem.ITEM_SOLICITACAODECOMPRAENVIADA;
									statusChange = true;
								}
							}
						}
						else
							statusChange = true;
					}
					else
					{
						if (currentPedidoItem.Quantidade == 0)
							throw new ApplicationException("A quantidade do Item está zerado!");

						//if (currentPedidoItem.Valor == 0)
						//	throw new ApplicationException("O valor do Item está zerado!");

						currentPedidoItem.QuantidadePendenteCompra = currentPedidoItem.Quantidade;

						if (currentPedidoItem.IdStatus == 0)
						{
							if (string.IsNullOrWhiteSpace(currentPedidoItem.Acordo))
							{
								currentPedidoItem.IdStatus = (int)StatusItem.ITEM_SOLICITACAODECOMPRACRIADA;
								statusChange = true;

								if (!string.IsNullOrWhiteSpace(currentPedidoItem.Pedido.LoginBase))
								{
									currentPedidoItem.IdStatus = (int)StatusItem.ITEM_EMANALISE;
								}
							}
							else
							{
								currentPedidoItem.IdStatus = (int)StatusItem.ITEM_SOLICITACAODECOMPRAENVIADA;
								statusChange = true;
							}
						}
						else
							statusChange = true;
					}


					if (!string.IsNullOrWhiteSpace(currentPedidoItem.LoginComprador))
					{
						if (currentPedidoItem.IdTipo == 0)
							throw new NotFoundException("O Tipo do pedido não pode ser nulo quando o comprador é atribuído");

						if (currentPedidoItem.Id > 0)
						{
							var existPedidoItem = await pedidoItemRepository.GetById(currentPedidoItem.Id, cancellationToken);

							if (existPedidoItem == null)
								throw new NotFoundException("id pedido item não encontrado!");

							if (currentPedidoItem.LoginComprador != existPedidoItem.LoginComprador)
							{
								currentPedidoItem.IdStatus = (int)StatusItem.ITEM_ATRIBUIDOAOCOMPRADOR;

								statusChange = true;
							}

							if (!DBNull.Value.Equals(currentPedidoItem.DataVinculoComprador) || (currentPedidoItem.DataVinculoComprador != existPedidoItem.DataVinculoComprador))
								currentPedidoItem.DataVinculoComprador = DateTime.Now;
						}
						else
						{
							currentPedidoItem.IdStatus = (int)StatusItem.ITEM_ATRIBUIDOAOCOMPRADOR;

							statusChange = true;

							if (!DBNull.Value.Equals(currentPedidoItem.DataVinculoComprador))
								currentPedidoItem.DataVinculoComprador = DateTime.Now;
						}

						currentPedidoItem.UserComprador = await userRepository.GetByLogin(currentPedidoItem.LoginComprador, cancellationToken);

					}


					if (currentPedidoItem.DataNecessidade == null)
					{
						if (currentPedidoItem.Pedido.DataNecessidade != null)
							currentPedidoItem.DataNecessidade = currentPedidoItem.Pedido.DataNecessidade;
					}

					if (!string.IsNullOrWhiteSpace(currentPedidoItem.LoginComprador))
					{
						currentPedidoItem.UserComprador = userRepository.GetByLogin(currentPedidoItem.LoginComprador, cancellationToken).Result;

						if (!DBNull.Value.Equals(currentPedidoItem.DataVinculoComprador))
							currentPedidoItem.DataVinculoComprador = DateTime.Now;
					}

					var status = await statusItemRepository.GetById(currentPedidoItem.IdStatus, cancellationToken);
					if (status == null)
						throw new NotFoundException("status não encontrado!");
					else
						currentPedidoItem.StatusPedidoItem = status;


					var vltotal = currentPedidoItem.Quantidade * currentPedidoItem.Valor;
					currentPedidoItem.ValorItens = vltotal;


					pedidoItemRepository.AddOrUpdate(currentPedidoItem, cancellationToken);
					var result = unitOfWork.SaveChanges();

					if (currentPedidoItem.Arquivos.Count() > 0)
					{
						foreach (var anexos in currentPedidoItem.Arquivos)
						{
							anexos.IdPedidoItem = currentPedidoItem.Id;
							anexos.PedidoItem = currentPedidoItem;
							pedidoItemAnexosRepository.AddOrUpdate(anexos, cancellationToken);
							result = unitOfWork.SaveChanges();
						}
					}

					if (!result) throw new ApplicationException("An error has occured.");

					if (statusChange)
					{
						if (currentPedidoItem.IdStatus == (int)StatusItem.ITEM_EMANALISE || currentPedidoItem.IdStatus == (int)StatusItem.ITEM_ATRIBUIDOAOCOMPRADOR)
						{
							var existTracking = trackingRepository.GetAll().Where(i => i.IdPedidoItem == currentPedidoItem.Id &&
							i.StatusId == (int)StatusItem.ITEM_SOLICITACAODECOMPRACRIADA).ToList();

							if (existTracking.Count == 0)
							{
								await mediator.Send(new AddTracking()
								{
									Tracking = new Tracking()
									{
										IdPedidoItem = currentPedidoItem.Id,
										StatusId = (int)StatusItem.ITEM_SOLICITACAODECOMPRACRIADA,
										ChangeById = currentPedidoItem.Pedido.LoginSolicitante
									}
								}, cancellationToken);
							}
						}

						await mediator.Send(new AddTracking()
						{
							Tracking = new Tracking()
							{
								IdPedidoItem = currentPedidoItem.Id,
								StatusId = currentPedidoItem.IdStatus,
								ChangeById = currentPedidoItem.Pedido.LoginSolicitante
							}
						}, cancellationToken);
					}

					await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = currentPedidoItem });
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

		async Task<Unit> IRequestHandler<SaveRequisitionItem, Unit>.Handle(SaveRequisitionItem request, CancellationToken cancellationToken)
		{
			try
			{
				var pedidoItem = request.PedidoItem;
				if (pedidoItem != null)
				{
					var requisitioningBUIdEnvironmentVariable = Environment.GetEnvironmentVariable("OIC_REQUISITIONINGBUID");
					long requisitioningBUId = long.Parse(requisitioningBUIdEnvironmentVariable);
					var DestinationOrganizationIdEnvironmentVariable = Environment.GetEnvironmentVariable("OIC_DESTINATIONORGANIZATIONID");
					long destinationOrganizationId = long.Parse(DestinationOrganizationIdEnvironmentVariable);
					var deliverToLocationCode = Environment.GetEnvironmentVariable("OIC_DELIVERTOLOCATIONCODE");
					//Todo: apontar para o valor do email do usuáruio logado, quando o usuário estiver no oracle
					//vamos deixar hardcoded em principio
					var requesterEmail = "marco.santiago@g.globo";
					var finalidade = Environment.GetEnvironmentVariable("OIC_FINALIDADE");
					var preparerEmail = Environment.GetEnvironmentVariable("OIC_USEREMAIL");

					RequisitionHeaderDFFClass requisitionHeaderDFFClass = new RequisitionHeaderDFFClass();
					requisitionHeaderDFFClass.enderecoComplementar = deliverToLocationCode;

					var bodyHeader = SerializeEntityObject(new
					{
						RequisitioningBUId = requisitioningBUId,
						PreparerEmail = preparerEmail,
						Description = "Item de Material de Escritório",
						ExternallyManagedFlag = false,
						DFF = new[] { requisitionHeaderDFFClass }
					});

					var returnHeader = await purchaseRequisitionProxy.PostRequisitionHeaderAsync(bodyHeader, cancellationToken);
					string requisitionHeaderId = returnHeader.RequisitionHeaderId.ToString();
					//contador++;  
					if (pedidoItem.DataNecessidade.Value.Date < DateTime.Now.Date)
						throw new BadRequestException("Data necessidade está fora do período!");

					long idItemOracle = 0;
					if (pedidoItem.IdItemOracle != string.Empty)
						idItemOracle = long.Parse(pedidoItem.IdItemOracle);
					else
						throw new BadRequestException("item sem id item oracle");

					var bodyLine = SerializeEntityObject(new
					{
						LineNumber = 1,//Todo: acertar a numeração automática
						LineTypeId = 1,
						LineTypeCode = "Goods",
						CurrencyCode = "BRL",
						DestinationTypeCode = "EXPENSE",
						UOM = "Unidade", //Todo: Pegar a UOM do catalogo;
						DestinationType = "Expense",
						RequesterEmail = requesterEmail,
						RequestedDeliveryDate = pedidoItem.DataNecessidade.Value.Date.ToString("yyyy-MM-dd"),
						DestinationOrganizationId = destinationOrganizationId,//hard coded
						Quantity = pedidoItem.Quantidade,
						Price = pedidoItem.Valor,

						DeliverToLocationCode = deliverToLocationCode,
						ItemId = idItemOracle
					}); ;

					if (requisitionHeaderId == string.Empty)
						throw new BadRequestException("requisitionHeaderId inválido!");

					var returnLine = await purchaseRequisitionProxy.PostRequisitionLineAsync(requisitionHeaderId, bodyLine, cancellationToken);

					if (returnLine != null)
					{
						//3 - distributions
						string requisitionLineId = returnLine.RequisitionLineId.ToString();
						DistributionDFFClass distributionDFFClass = new DistributionDFFClass();
						distributionDFFClass.centroDeCusto = Environment.GetEnvironmentVariable("OIC_CENTRODECUSTO");

						if (requisitionLineId == string.Empty)
							throw new BadRequestException("requisitionLineId inválido!");

						distributionDFFClass.finalidade = finalidade;

						var bodyDistribution = SerializeEntityObject(new
						{
							DistributionNumber = 1,//contador,// é o mesmo que LineNumber=1
							Quantity = pedidoItem.Quantidade,//é o mesmo que quantidade de itens.
							DFF = new[] { distributionDFFClass }
						});

						var returnDistribution = await purchaseRequisitionProxy.PostRequisitionDistributionWithLineAsync(requisitionHeaderId, requisitionLineId, bodyDistribution, cancellationToken);
						if (returnDistribution.RequisitionLineId == string.Empty)
							throw new BadRequestException("DistributionId inválido!");

						var bodySubmitRequisition = SerializeEntityObject(new
						{
							name = "submitRequisition",
							parameters = new string[] { }
						});

						var returnDistributionItem = await purchaseRequisitionProxy.PostRequisitionAsync(requisitionHeaderId, bodySubmitRequisition, cancellationToken);
						if (returnDistributionItem.Result == "SUCCESS")
						{
							pedidoItem.RcPedido = returnHeader.Requisition;
							pedidoItem.IdStatus = (int)StatusItem.ITEM_APROVADO;
							pedidoItemRepository.AddOrUpdate(pedidoItem, cancellationToken); 
							var result = unitOfWork.SaveChanges(); 

							if (result)
                            {
								pedidoItem.RcPedido = returnHeader.Requisition;
								pedidoItem.IdStatus = (int)StatusItem.ITEM_APROVADO;
								pedidoItemRepository.AddOrUpdate(pedidoItem, cancellationToken);

								await mediator.Send(new AddTracking()
								{
									Tracking = new Tracking()
									{
										IdPedidoItem = pedidoItem.Id,
										StatusId = pedidoItem.IdStatus,
										ChangeById = userProvider.User.Login
									}
								}, cancellationToken);
							}
						}
						else
						{
							throw new BadRequestException("Não foi possível aprovar a RC do Item HeaderID: " + requisitionHeaderId);
						}

					}
				}
			}
			catch (Exception error)
			{
				throw new Exception(error.Message);
			}
			return await Unit.Task;
		}

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
		 