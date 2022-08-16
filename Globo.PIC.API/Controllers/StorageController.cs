using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Globo.PIC.Domain.ViewModels;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Types.Queries;
using Globo.PIC.Domain.Exceptions;
using Globo.PIC.Application.Filters;
using System.IO;
using Globo.PIC.Domain.Extensions;

namespace Globo.PIC.API.Controllers
{
    /// <summary>
	/// 
	/// </summary>
    [ApiController]
    [Route("storage")]
    [Authorize(Policy = "MatchProfile")]
    public class StorageController : ControllerBase
    {
        /// <summary>
		/// 
		/// </summary>
		private readonly ILogger<StorageController> logger;

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
        private readonly IUserProvider userProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_logger"></param>
        /// <param name="_mapper"></param>
        /// <param name="_mediator"></param>
        /// <param name="_userProvider"></param>
        public StorageController(ILogger<StorageController> _logger,
                                IMapper _mapper,
                                IMediator _mediator,
                                IUserProvider _userProvider)
        {
            logger = _logger;
            mapper = _mapper;
            mediator = _mediator;
            userProvider = _userProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("urlpresigned")]
        [ProducesResponseType(200, Type = typeof(PreSignedUrlViewModel))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUpload([FromQuery] PreSignedUrlUploadFilter filter, CancellationToken cancellationToken)
        {
                if (filter == null) throw new BadRequestException("O parametro filter é obrigatório.");

                if (!string.IsNullOrEmpty(filter.ContentType) && string.IsNullOrEmpty(filter.FileName)) throw new BadRequestException("O parametro FileName é obrigatório.");

                if (!string.IsNullOrEmpty(filter.FileName) && string.IsNullOrEmpty(filter.ContentType)) throw new BadRequestException("O parametro ContentType é obrigatório.");

                var tempFileName = $"{Guid.NewGuid()}{Path.GetExtension(filter.FileName)}";

                var url = await mediator.Send(new GetPreSignedUrl()
                {   
                    Key = tempFileName,
                    ContentType = filter.ContentType,
                    Verb = "PUT",
                    Expires = DateTime.Now.AddMinutes(5)
                }, cancellationToken);

                var result = new PreSignedUrlViewModel()
                {
                    Url = url,
                    TempFileName = tempFileName
                };

                return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("download")]
        [ProducesResponseType(200, Type = typeof(PreSignedUrlViewModel))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetDownloadAsync([FromQuery] string name, CancellationToken cancellationToken)
        {
                if (name == null) throw new BadRequestException("O parametro name é obrigatório.");

                var url = await mediator.Send(new GetPreSignedUrl()
                {
                    Key = $"{name}",
                    FileName = name,
                    ContentType = string.Empty,
                    Verb = "GET",
                    Expires = DateTime.Now.AddDays(1)
                }, cancellationToken);

                var result = new PreSignedUrlViewModel
                {
                    Url = url,
                    TempFileName = name
                };            

                return Ok(result);
        }
    }
}
