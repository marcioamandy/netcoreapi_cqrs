using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Models;
using Globo.PIC.Domain.Types.Queries;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Globo.PIC.Application.QueryHandlers
{
    public class SupplierQueryHandler :
       IRequestHandler<GetByFornecedorFilter, List<Supplier>>,
       IRequestHandler<GetByCnpj, Supplier>,
       IRequestHandler<GetAgrementsByCNPJ, List<Agreements>>

    {

        private readonly ISupplierProxy supplier;

        private readonly IMediator mediator;
        public SupplierQueryHandler(ISupplierProxy _supplier, IMediator _mediator)
        {
            supplier = _supplier;
            mediator = _mediator;
        }

        async Task<List<Supplier>> IRequestHandler<GetByFornecedorFilter, List<Supplier>>.Handle(GetByFornecedorFilter request, CancellationToken cancellationToken)
        {
            var retorno = await supplier.GetSuppliers(request.Filter.Search, cancellationToken);
            return retorno;
        }

        async Task<Supplier> IRequestHandler<GetByCnpj, Supplier>.Handle(GetByCnpj request, CancellationToken cancellationToken)
        {
            var retorno = await supplier.GetSuppliersByCode(request.Cnpj, cancellationToken);
            return retorno;
        }

        async Task<List<Agreements>> IRequestHandler<GetAgrementsByCNPJ, List<Agreements>>.Handle(GetAgrementsByCNPJ request, CancellationToken cancellationToken)
        {
            var retorno = await supplier.GetAgreementBySuppliers(request.Cnpj, cancellationToken);
            return retorno;
        }
    }
}
