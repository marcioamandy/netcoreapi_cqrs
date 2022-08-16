using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Globo.PIC.API.Configurations.Attributes;
using Globo.PIC.Domain.ViewModels;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Enums;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Types.Queries;
using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Globo.PIC.API.Controllers
{

	/// <summary>
	/// 
	/// </summary>
	[ApiController]
	[Route("usuarios")]
	[Authorize(Policy = "MatchProfile")]
	public class UsuariosController : ControllerBase
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
		private readonly IUserProvider userProvider;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="_mapper"></param>
		/// <param name="_mediator"></param>
		/// <param name="_userProvider"></param>
		public UsuariosController(
								IMapper _mapper,
								IMediator _mediator,
								IUserProvider _userProvider)
		{
			mapper = _mapper;
			mediator = _mediator;
			userProvider = _userProvider;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="login"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpGet("{login}")]
		[ProducesResponseType(200, Type = typeof(UserFilterViewModel))]
		[ProducesResponseType(400, Type = typeof(string))]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		[IsInRole(Role.GRANT_ADM_USUARIOS)]
		public async Task<IActionResult> GetByLoginAsync([FromRoute] string login, CancellationToken cancellationToken)
		{
			if (login == "") throw new BadRequestException("O parametro login é obrigatório.");

			var user = await mediator.Send(new GetUsuarioLogin()
			{
				Login = login				
			}, cancellationToken);

			if (user == null) return NotFound();

			var result = ToViewModel(user);

			return Ok(result);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filter"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<UsuarioViewModel>))]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> GetUsuariosAsync([FromQuery] UserFilterViewModel filter, CancellationToken cancellationToken)
		{
			var count = await mediator.Send<int>(new GetByUserFilterCount() { Filter = filter }, cancellationToken);

			if (count == 0) throw new NotFoundException("Nenhum registro encontrado.");

			var users = await mediator.Send(new GetByUserFilter() { Filter = filter }, cancellationToken);

			List<UsuarioViewModel> userList = new List<UsuarioViewModel>();
            
			foreach (var item in users)
			{
				var supplierVM = ToViewModel(item);

				userList.Add(supplierVM);
			}
		
			var pagging = new GenericPaggingViewModel<UsuarioViewModel>(userList, filter, count);

			return Ok(pagging);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpGet("me")]
		[ProducesResponseType(200, Type = typeof(IdentityUserViewModel))]
		[ProducesResponseType(400, Type = typeof(string))]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		public ActionResult GetMe()
		{
			var usuario = userProvider.User;
			var userViewModel = mapper.Map<IdentityUserViewModel>(usuario);
			return Ok(userViewModel);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="UserVM"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpPost]
		[ProducesResponseType(201, Type = typeof(UsuarioViewModel))]
		[ProducesResponseType(400, Type = typeof(string))]
		[ProducesResponseType(422, Type = typeof(string))]
		[ProducesResponseType(500)]
		[IsInRole(Role.GRANT_ADM_USUARIOS)]
		public async Task<IActionResult> PostUsuarios([FromBody] UsuarioViewModel UserVM, CancellationToken cancellationToken)
		{
			if (UserVM == null)
				throw new BadRequestException("The parameter supplierVM is required.");

			var user = mapper.Map<Usuario>(UserVM);

			await mediator.Send(new CreateUsuario() { Usuario = user }, cancellationToken);

			UserVM = ToViewModel(user);

			return StatusCode(StatusCodes.Status201Created, mapper.Map<UsuarioViewModel>(UserVM));
		}

		/// <summary>
		/// public async Task<IActionResult> PutPedidoItemVeiculo(
		/// </summary>
		/// <param name="login"></param>
		/// <param name="UserVM"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpPut("{login}")]
		[ProducesResponseType(200, Type = typeof(UsuarioViewModel))]
		[ProducesResponseType(400, Type = typeof(string))]
		[ProducesResponseType(422, Type = typeof(string))]
		[ProducesResponseType(500)]
		[IsInRole(Role.GRANT_ADM_USUARIOS)]
		public async Task<IActionResult> PutUsuarios([FromRoute] string login, [FromBody] UsuarioViewModel UserVM, CancellationToken cancellationToken)
		{
			if (UserVM == null)
				throw new BadRequestException("The parameter UserVM is required.");

			if (string.IsNullOrWhiteSpace(login))
				throw new BadRequestException("The parameter login is required.");

			if (UserVM.Login != login)
				throw new BadRequestException("Chave do usuário não correspondente.");

			var user = mapper.Map<Usuario>(UserVM);

			await mediator.Send(new UpdateUsuario() { Usuario = user }, cancellationToken);

			var result = ToViewModel(user);

			return Ok(result);
		}

		private UsuarioViewModel ToViewModel(Usuario user)
		{
			var userVM = mapper.Map<UsuarioViewModel>(user);

			return userVM;
		}
	}
}