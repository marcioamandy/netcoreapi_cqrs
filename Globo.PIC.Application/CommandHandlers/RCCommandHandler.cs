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
using Globo.PIC.Domain.ViewModels;
using Globo.PIC.Domain.Models;
using Globo.PIC.Infra.Data.Repositories;

namespace Globo.PIC.Application.Services.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class RCCommandHandler :
        IRequestHandler<UpdateRCLineStatus>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly RCRepository rCRepository;

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
        public RCCommandHandler(
            IUnitOfWork _unitOfWork,
            IMediator _mediator,
            IRepository<RC> _rCRepository)
        {
            unitOfWork = _unitOfWork;
            mediator = _mediator;
            rCRepository = _rCRepository as RCRepository;
        }        

        async Task<Unit> IRequestHandler<UpdateRCLineStatus, Unit>.Handle(UpdateRCLineStatus request, CancellationToken cancellationToken)
        {
            var st = request.StatusLineOCRC;

            var rc = rCRepository.GetAll().FirstOrDefault(x =>
                x.Requisition != null &&
                st.Requisitions[0].Number.ToUpper().Equals(x.Requisition.ToUpper()));

            //Avaliar por a verificação de só atualizar quando tiver divergencia com a base
            if (rc != null)
            {
                rc.LinhaStatus = st.Status;
                rc.Status = st.Requisitions[0].Status;

                if (st.Purchases != null && st.Purchases.Count > 0)
                    rc.OrdemCompra = st.Purchases[0].Number;
            } else
                return Unit.Value;

            rCRepository.AddOrUpdate(rc, cancellationToken);

            var result = unitOfWork.SaveChanges();

            if (!result) throw new ApplicationException("Erro ao atualizar RC com notificação de mudança do status da linha.");

            await mediator.Publish(new OnStatusRCAlterada() { RC = rc }, cancellationToken);

            return Unit.Value;
        }
    }
}
