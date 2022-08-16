using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Globo.PIC.Domain.ViewModels;
using Globo.PIC.Domain.Types.Queries;
using Globo.PIC.Domain.Exceptions;

namespace Globo.PIC.API.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("tarefas")]
    [Authorize(Policy = "MatchProfile")]
    public class TarefasController : ControllerBase
    {

        /// <summary>
        /// 
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// 
        /// </summary>
        private readonly IMediator mediator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_mapper"></param>
        /// <param name="_mediator"></param>
        /// <param name="_userProvider"></param>
        public TarefasController(IMapper _mapper,
                                IMediator _mediator)
        {
            mapper = _mapper;
            mediator = _mediator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<List<TarefaViewModel>>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTarefaAsync([FromQuery] TarefaFilterViewModel filter, CancellationToken cancellationToken)
        {
            var tarefaList = await mediator.Send(new GetByTarefaFilter() { Filter = filter }, cancellationToken);

            if (tarefaList.Count == 0) throw new NotFoundException("Tarefa não encontrada.");

            var tarefas = mapper.Map<List<TarefaViewModel>>(tarefaList);

            return Ok(tarefas);
        }
    }
}
