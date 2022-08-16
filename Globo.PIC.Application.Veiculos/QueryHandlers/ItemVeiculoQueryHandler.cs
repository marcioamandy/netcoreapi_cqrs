using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
using System.Threading;
using Globo.PIC.Domain.Types.Queries;
using Globo.PIC.Infra.Data.Repositories;

namespace Globo.PIC.Application.Veiculo.QueryHandlers
{
	public class ItemVeiculoQueryHandler :		
		IRequestHandler<GetItemVeiculoById, ItemVeiculo>,
		IRequestHandler<ListItemVeiculoByPedidoItemVeiculo, List<ItemVeiculo>>
	{	

		/// <summary>
		/// 
		/// </summary>
		private readonly ItemVeiculoRepository itemVeiculoRepository;

		public ItemVeiculoQueryHandler(
			IRepository<ItemVeiculo> _itemVeiculoRepository			
		)
		{
			itemVeiculoRepository = _itemVeiculoRepository as ItemVeiculoRepository;			
		}

        Task<List<ItemVeiculo>> IRequestHandler<ListItemVeiculoByPedidoItemVeiculo, List<ItemVeiculo>>.Handle(ListItemVeiculoByPedidoItemVeiculo request, CancellationToken cancellationToken)
        {
			var query = itemVeiculoRepository.GetAll();

			query = query.Where(x => x.PedidoItemVeiculo.IdPedidoItem == request.PedidoItemId);

			return query.ToListAsync(cancellationToken);
		}

        Task<ItemVeiculo> IRequestHandler<GetItemVeiculoById, ItemVeiculo>.Handle(GetItemVeiculoById request, CancellationToken cancellationToken)
        {
			return itemVeiculoRepository.GetById(request.Id, cancellationToken).AsTask();
		}
    }
}
