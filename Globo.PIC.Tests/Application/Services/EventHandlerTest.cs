using Kledex;
using System;
using System.Collections.Generic;
using TVGlobo.R2P.Parceria.Domain.Entities;
using TVGlobo.R2P.Parceria.Domain.Enums;
using TVGlobo.R2P.Parceria.Domain.Events;
using TVGlobo.R2P.Parceria.Domain.Interfaces;
using TVGlobo.R2P.Parceria.Domain.Models;
using Xunit;

namespace TVGlobo.R2P.Parceria.Tests.Application.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public class EventHandlerTest
    {
        private readonly IEmailSender _emailSender;        
        private readonly EmailSettings _emailSettings;          
        private readonly IRepository<Tracking> _trakingRepository;
        private readonly IRepository<Notification> _notificationRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Programme> _programmeRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Audit> _auditRepository;
		private readonly IUserProvider _userProvider;
		private readonly IDispatcher _dispatcher;
        private readonly IUnitOfWork _unitOfWork;
        
        public EventHandlerTest()
        {
            
        }

        private Domain.EventHandlers.EventHandler GetEventHandler()
        {
            return new Domain.EventHandlers.EventHandler(
				_auditRepository,
				_orderRepository,
				_trakingRepository,
                _notificationRepository,
				_userRepository,
                _programmeRepository,
				_emailSender,
				_emailSettings,
				_userProvider,
				_dispatcher,
				_unitOfWork
			);
        }

        private EventAudit GetEventAudit()
        {
			return new EventAudit()
            {
                EntityId = 1,
                EntityName = Constants.None,
                Action = AuditActionEnum.None,
                Object = new Object()
            };
        }
    }
}
