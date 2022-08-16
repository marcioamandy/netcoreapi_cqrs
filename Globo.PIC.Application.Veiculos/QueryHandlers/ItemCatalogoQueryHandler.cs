using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Interfaces;
using MediatR;
using System.Threading;
using Globo.PIC.Domain.Types.Queries;
using Globo.PIC.Infra.Data.Repositories;


namespace Globo.PIC.Application.Veiculo.QueryHandlers
{
	public class ItemCatalogoQueryHandler :		
		IRequestHandler<GetItemCatalogoById, ItemCatalogo>,
		IRequestHandler<ListByItemCatalogoFilter, List<ItemCatalogo>>
	{	

		/// <summary>
		/// 
		/// </summary>
		private readonly ItemCatalogoRepository itemCatalogoRepository;

		public ItemCatalogoQueryHandler(
			IRepository<ItemCatalogo> _itemCatalogoRepository			
		)
		{
			itemCatalogoRepository = _itemCatalogoRepository as ItemCatalogoRepository;			
		}

        Task<ItemCatalogo> IRequestHandler<GetItemCatalogoById, ItemCatalogo>.Handle(GetItemCatalogoById request, CancellationToken cancellationToken)
        {
			return itemCatalogoRepository.GetById(request.Id, cancellationToken).AsTask();
		}

		Task<List<ItemCatalogo>> IRequestHandler<ListByItemCatalogoFilter, List<ItemCatalogo>>.Handle(ListByItemCatalogoFilter request, CancellationToken cancellationToken)
		{
			var retorno = itemCatalogoRepository.GetAll();

			int.TryParse(request.Filter.Page.ToString(), out int page);
			page = page <= 0 ? 1 : page;

			var catalogos = retorno.Where(x =>
				   (!request.Filter.DataInicio.HasValue || x.DataInicio.Value.Date >= request.Filter.DataInicio.Value.Date) &&
				   (!request.Filter.DataFim.HasValue || x.DataFim.Value.Date <= request.Filter.DataFim.Value.Date) &&
				   (request.Filter.Conteudos == null || request.Filter.Conteudos.Contains(x.IdConteudo)) &&
				   (x.BloqueadoOutrosConteudos == request.Filter.BloqueadoEmprestimo) &&
				   (request.Filter.NomeItem == null || x.Item.ItemVeiculo.PedidoItemVeiculo.PedidoItem.NomeItem.Contains(request.Filter.NomeItem)) &&
				   (request.Filter.Descricao == null || x.Item.ItemVeiculo.PedidoItemVeiculo.PedidoItem.Descricao.Contains(request.Filter.Descricao)))
				   .Skip((page - 1) * request.Filter.PerPage).Take(request.Filter.PerPage);

			return Task.FromResult(catalogos.ToList());
		}
	}
}
