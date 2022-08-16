using System;
using System.Collections.Generic;
using Globo.PIC.Domain.Entities;
using MediatR;

namespace Globo.PIC.Domain.Types.Queries
{        
    public class ListItemVeiculoByPedidoItemVeiculo : IRequest<List<ItemVeiculo>>
    {
        public long PedidoId { get; set; }

        public long PedidoItemId { get; set; }
    }
}
