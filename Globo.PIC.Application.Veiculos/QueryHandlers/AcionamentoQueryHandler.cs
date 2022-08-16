using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Models;
using Microsoft.EntityFrameworkCore;
using MediatR;
using System.Threading;
using Globo.PIC.Domain.Types.Queries;
using Globo.PIC.Domain.Types.Events;
using AutoMapper;
using Globo.PIC.Infra.Data.Repositories;
using Globo.PIC.Domain.Exceptions;

namespace Globo.PIC.Application.Veiculo.QueryHandlers
{
    public class AcionamentoQueryHandler :
        IRequestHandler<GetByIdAcionamento, Acionamento>,
        IRequestHandler<ListByAcionamentoFilter, List<Acionamento>>,
        IRequestHandler<GetByIdAcionamentoItem, AcionamentoItem>,
        IRequestHandler<ListByAcionamentoItemFilter, List<AcionamentoItem>>
    {
        /// <summary>
        ///
        /// </summary>
        private readonly IRepository<Acionamento> acionamentoRepository;

        /// <summary>
        ///
        /// </summary>
        private readonly IRepository<AcionamentoItem> acionamentoItemRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IProjectProxy project;

        /// <summary>
        /// 
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_talentoRepository"></param>
        public AcionamentoQueryHandler(
            IRepository<Acionamento> _acionamentoRepository,
            IRepository<AcionamentoItem> _acionamentoItemRepository,
            IProjectProxy _project,
            IMapper _mapper
            )
        {
            acionamentoRepository = _acionamentoRepository;
            acionamentoItemRepository = _acionamentoItemRepository;
            project = _project;
            mapper = _mapper;
        }

        Task<List<Acionamento>> IRequestHandler<ListByAcionamentoFilter, List<Acionamento>>.Handle(ListByAcionamentoFilter request, CancellationToken cancellationToken)
        {
            var retorno = acionamentoRepository.GetAll().Where(a => a.IdPedido == request.Filter.IdPedido);

            int.TryParse(request.Filter.Page.ToString(), out int page);
            page = page <= 0 ? 1 : page;

            return Task.FromResult(retorno.ToList());
        }

        Task<Acionamento> IRequestHandler<GetByIdAcionamento, Acionamento>.Handle(GetByIdAcionamento request, CancellationToken cancellationToken)
        {
            var acionamento = acionamentoRepository.GetById(request.Id, cancellationToken);

            if (acionamento == null) throw new NotFoundException("Acionamento não encontrado!");

            return acionamento.AsTask();
        }

        Task<List<AcionamentoItem>> IRequestHandler<ListByAcionamentoItemFilter, List<AcionamentoItem>>.Handle(ListByAcionamentoItemFilter request, CancellationToken cancellationToken)
        {
            var retorno = acionamentoItemRepository.GetAll().Where(a => a.IdAcionamento == request.Filter.IdAcionamento && a.IdPedidoItem == request.Filter.IdPedidoItem);

            int.TryParse(request.Filter.Page.ToString(), out int page);
            page = page <= 0 ? 1 : page;

            return Task.FromResult(retorno.ToList());
        }

        Task<AcionamentoItem> IRequestHandler<GetByIdAcionamentoItem, AcionamentoItem>.Handle(GetByIdAcionamentoItem request, CancellationToken cancellationToken)
        {
            var acionamentoItem = acionamentoItemRepository.GetById(request.Id, cancellationToken);

            if (acionamentoItem == null) throw new NotFoundException("Acionamento Item não encontrado!");

            return acionamentoItem.AsTask();
        }
    }
}
