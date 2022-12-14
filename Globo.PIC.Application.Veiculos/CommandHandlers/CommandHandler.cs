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

namespace Globo.PIC.Application.Services.Commands
{
	/// <summary>
	/// 
	/// </summary>
	public class CommandHandler :
        IRequestHandler<SaveNotificacao>,
        //IRequestHandler<SaveViewer>,
        IRequestHandler<SetNotificacaoLida>,
		IRequestHandler<AddTrackingVeiculo>,
		IRequestHandler<DeleteTrackingVeiculo>
		//IRequestHandler<CheckStatusChanged>
	{

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<Usuario> userRepository;


		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<Notificacao> notificacaoRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<Viewer> viewerRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<Reader> readerRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemVeiculoTracking> trackingRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<StatusPedidoVeiculo> statusRepository;

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
		public CommandHandler(
			IUnitOfWork _unitOfWork,
			IMediator _mediator,
			IUserProvider _userProvider,
			IRepository<Usuario> _userRepository, 
			IRepository<Notificacao> _notificacaoRepository,
			IRepository<Viewer> _viewerRepository,
			IRepository<Reader> _readerRepository,
			IRepository<PedidoItemVeiculoTracking> _trackingRepository,
			IRepository<StatusPedidoVeiculo> _statusRepository
			)
		{
			unitOfWork = _unitOfWork;
			mediator = _mediator;
			userRepository = _userRepository;
			notificacaoRepository = _notificacaoRepository;
			viewerRepository = _viewerRepository;
			readerRepository = _readerRepository;
			trackingRepository = _trackingRepository;
			statusRepository = _statusRepository;
			userProvider = _userProvider;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		Task<Unit> IRequestHandler<AddTrackingVeiculo, Unit>.Handle(AddTrackingVeiculo request, CancellationToken cancellationToken)
		{
			var trackingList = trackingRepository.GetAll().Where(s => s.IdPedidoItem == request.PedidoItemVeiculoTracking.IdPedidoItem && s.Ativo == true).ToList();

			//Verifica se o status solicitado já é o status atual
			if (trackingList.Count() > 0 && trackingList.Last().StatusId == request.PedidoItemVeiculoTracking.StatusId)
				return Task.FromResult(Unit.Value); 

			var tracking = new PedidoItemVeiculoTracking()
			{
				ChangedBy = request.PedidoItemVeiculoTracking.ChangedBy,
				Status = request.PedidoItemVeiculoTracking.Status,
				StatusId = request.PedidoItemVeiculoTracking.StatusId,
				IdPedidoItem = request.PedidoItemVeiculoTracking.IdPedidoItem,
				PedidoItemVeiculo = request.PedidoItemVeiculoTracking.PedidoItemVeiculo,
				ChangeById = userProvider.User.Login,
			};

			if (trackingList.FirstOrDefault() != null)
				tracking.StatusPosition = trackingList.Last().StatusPosition + 1;
			else
				tracking.StatusPosition = 1;

			tracking.Ativo = true;

			tracking.TrackingDate = DateTime.Now;

			trackingRepository.Add(tracking, cancellationToken);

			var result = unitOfWork.SaveChanges();

			if (!result) throw new ApplicationException("An error has occured.");

			return Task.FromResult(Unit.Value);
		}

		Task<Unit> IRequestHandler<DeleteTrackingVeiculo, Unit>.Handle(DeleteTrackingVeiculo request, CancellationToken cancellationToken)
		{
			var existTracking = trackingRepository.GetAll()
				.Where(i => i.IdPedidoItem == request.TrackingVeiculo.IdPedidoItem &&
						i.StatusId == request.TrackingVeiculo.StatusId &&
						i.Ativo == true).ToList();

			if (existTracking.Count > 0)
			{

				//trackingRepository.Update(existTracking)
				trackingRepository.Remove(existTracking);

				var result = unitOfWork.SaveChanges();

				if (!result) throw new ApplicationException("An error has occured.");
			}

			return Task.FromResult(Unit.Value);
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

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="request"></param>
        //Task<Unit> IRequestHandler<SaveViewer, Unit>.Handle(SaveViewer request, CancellationToken cancellationToken)
        //{
        //	viewerRepository.AddOrUpdate(request.Viewer, cancellationToken);

        //	var result = unitOfWork.Commit();

        //	if (!result) throw new ApplicationException("An error has occured.");

        //	return Task.FromResult(Unit.Value);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        Task<Unit> IRequestHandler<SetNotificacaoLida, Unit>.Handle(SetNotificacaoLida request, CancellationToken cancellationToken)
		{
			if (
				!request.Notification.Assigns.Any() ||
				!request.Notification.Assigns.Where(a =>
					userProvider.User.Authorization.Roles.Contains(a.Role) ||
					userProvider.User.Login.Equals(a.Login)).Any()
			) throw new ApplicationException("Desculpe, somente é possível ler suas próprias mensagens.");

			//Retorna da função caso a notificação já esteja marcado como lida.
			if (request.Notification.Readers.Where(r => r.Login.Equals(userProvider.User.Login)).Any())
				Task.FromResult(Unit.Value); 

			readerRepository.Add(new Reader()
			{
				NotificationId = request.Notification.Id,
				Login = userProvider.User.Login
			}, cancellationToken);

			var result = unitOfWork.Commit();

			if (!result) throw new ApplicationException("An error has occured.");

			return Task.FromResult(Unit.Value);
		}
 

		///// <summary>
		///// 
		///// </summary>
		///// <param name="request"></param>
		//Task<Unit> IRequestHandler<CheckStatusChanged, Unit>.Handle(CheckStatusChanged request, CancellationToken cancellationToken)
  //      {
  //          var talento = (Talento)mediator.Send(new GetTalentoById()
  //          {
  //              Id = request.Medicao.ActorId,
  //              CancellationToken = cancellationToken
  //          }).Result;

  //          if (request.StatusId == 2)
  //              mediator.Publish(new OnMedicaoRealizada()
  //              {
  //                  Medicao = request.Medicao,
  //                  StatusId = request.StatusId
  //              });

  //          else if (request.StatusId == 3)
  //              mediator.Publish(new OnMedicaoValidada()
  //              {
  //                  Talento = talento,
  //                  Medicao = request.Medicao,
  //                  StatusId = request.StatusId
  //              });

  //          else if (request.StatusId == 4)
  //              mediator.Publish(new OnMedicaoExpirada()
  //              {
  //                  Medicao = request.Medicao,
  //                  StatusId = request.StatusId
  //              });

  //          return Task.FromResult(Unit.Value);
		//}

	}
}