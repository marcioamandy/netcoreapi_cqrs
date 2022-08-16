using System;
using Globo.PIC.Domain.Entities;
using MediatR;

namespace Globo.PIC.Domain.Types.Queries
{
    public class GetItemCatalogoById : IRequest<ItemCatalogo>
    {
        public long Id { get; set; }
    }
}
