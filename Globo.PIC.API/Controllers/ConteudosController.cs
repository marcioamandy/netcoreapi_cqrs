using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Globo.PIC.Domain.ViewModels;
using Globo.PIC.Domain.Types.Queries;
using Globo.PIC.Domain.Exceptions;
using Globo.PIC.Domain.Models;

namespace Globo.PIC.API.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("conteudos")]
    [Authorize(Policy = "MatchProfile")]
    public class ConteudosController : ControllerBase
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
        /// <param name="_logger"></param>
        /// <param name="_mapper"></param>
        /// <param name="_mediator"></param>
        /// <param name="_userProvider"></param>
        public ConteudosController(
                                IMapper _mapper,
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
        [ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<ConteudoViewModel>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetConteudosByFilterAsync([FromQuery] ConteudoFilterViewModel filter, CancellationToken cancellationToken)
        {
            var conteudos = await mediator.Send(new GetByConteudoFilter()
            {
                Filter = filter
            }, cancellationToken);

            int contador = conteudos.Count;

            int.TryParse(filter.Page.ToString(), out int page);

            page = page <= 0 ? 1 : page;

            conteudos = conteudos.Skip((page - 1) * filter.PerPage).Take(filter.PerPage).ToList();

            if (conteudos.Count() == 0) throw new NotFoundException("Registro não encontrado.");

            var conteudoViewModel = mapper.Map<List<ConteudoViewModel>>(conteudos);

            return Ok(new GenericPaggingViewModel<ConteudoViewModel>(conteudoViewModel, filter, contador));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id:long}")]
        [ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<ConteudoViewModel>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetConteudoByIdAsync([FromRoute] long id, CancellationToken cancellationToken)
        {
            var projeto = await mediator.Send<ProjetoModel>(new GetByProjetoFilter()
            {
                ProjectId = id
            }, cancellationToken);

            if (projeto == null) throw new NotFoundException("conteúdo não encontrado.");

            var conteudoViewModel = mapper.Map<ConteudoViewModel>(projeto);

            return Ok(conteudoViewModel);
        }
    }
}
