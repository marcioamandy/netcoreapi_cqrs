using System;
using Globo.PIC.Domain.Entities;
using MediatR;

namespace Globo.PIC.Domain.Types.Queries
{
    public class GetItemVeiculoById : IRequest<ItemVeiculo>
    {
        public long Id { get; set; }
    }
}
