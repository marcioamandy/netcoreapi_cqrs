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
	public class ExpenditureQueryHandler :
       IRequestHandler<GetByTypeName, List<Expenditures>>
    {
         
        private readonly IExpenditureProxy expenditureProxy;

        public ExpenditureQueryHandler(IExpenditureProxy _expenditureProxy)
		{
            expenditureProxy = _expenditureProxy;
		}

        async Task<List<Expenditures>> IRequestHandler<GetByTypeName, List<Expenditures>>.Handle(GetByTypeName request, CancellationToken cancellationToken)
        {
            return await expenditureProxy.GetExpenditures(request.TypeName, cancellationToken);
        }
    }
}
