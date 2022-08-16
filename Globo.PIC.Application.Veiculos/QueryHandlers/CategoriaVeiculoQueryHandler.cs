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

namespace Globo.PIC.Application.Veiculo.QueryHandlers
{
    public class CategoriaVeiculoQueryHandler :
		IRequestHandler<GetById, Categoria>,
		//IRequestHandler<GetAll, Categoria>,
		IRequestHandler<ListCategoria, List<Categoria>>,
		IRequestHandler<GetByIdCategoriaVeiculos, Categoria>,
		IRequestHandler<ListSubCategoriaIdByIdTipoVeiculo, List<Categoria>>
	{
		/// <summary>
		/// 
		/// </summary>
		private readonly CategoriaRepository categoriaRepository;

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
		private readonly IMapper mapper;

		public CategoriaVeiculoQueryHandler(
			IRepository<Categoria> _categoriaRepository,
			IUserProvider _userProvider,
			IMediator _mediator,
			IMapper _mapper
			)
		{
			categoriaRepository = _categoriaRepository as CategoriaRepository;
			userProvider = _userProvider;
			mediator = _mediator;
			mapper = _mapper;
		}
	 
        Task<Categoria> IRequestHandler<GetById, Categoria>.Handle(GetById request, CancellationToken cancellationToken)
		{
			var query = categoriaRepository.GetById(request.Id, cancellationToken);
			return query.AsTask();
		}

		Task<Categoria> IRequestHandler<GetByIdCategoriaVeiculos, Categoria>.Handle(GetByIdCategoriaVeiculos request, CancellationToken cancellationToken)
		{
			var query = categoriaRepository.GetByIdTipoVeiculos(request.IdCategoria, cancellationToken);
			return query.AsTask();
		}

		Task<List<Categoria>> IRequestHandler<ListSubCategoriaIdByIdTipoVeiculo, List<Categoria>>.Handle(ListSubCategoriaIdByIdTipoVeiculo request, CancellationToken cancellationToken)
        {
			var query = categoriaRepository.ListSubCategoriaIdByIdTipoVeiculo(request.IdTipoVeiculo, cancellationToken);
			return query;
        }

		Task<List<Categoria>> IRequestHandler<ListCategoria, List<Categoria>>.Handle(ListCategoria request, CancellationToken cancellationToken)
		{
			var query = categoriaRepository.ListCategoria(cancellationToken);
			return query;
		}
		/*
		Task<List<Categoria>> IRequestHandler<GetAll, List<Categoria>>.Handle(CancellationToken cancellationToken)
		{
			var query = categoriaRepository.GetAll().ToList();
			return query;
		}
		*/
		//async Task<PedidoItemModel> IRequestHandler<GetByIdPedidoItemModel, PedidoItemModel>.Handle(GetByIdPedidoItemModel request, CancellationToken cancellationToken)
		//{
		//	var pedidoItem = mapper.Map<PedidoItemModel>(await pedidoItemRepository.GetByIdPedidoItem(request.Id, cancellationToken));

		//	pedidoItem.PedidoItemConversas.ForEach(x => x.Arquivos.ForEach(a => {

		//		try
		//		{
		//			var taskUrl = mediator.Send(new GetPreSignedUrl()
		//			{
		//				Key = a.NomeArquivo,
		//				FileName = a.NomeOriginal,
		//				Verb = "GET",
		//				Expires = DateTime.Now.AddDays(1)
		//			}, cancellationToken); taskUrl.Wait();

		//			a.CaminhoArquivo = taskUrl.Result;
		//		}
		//		catch (Exception) { }

		//	}));

		//	return pedidoItem;
		//}
	}
}
