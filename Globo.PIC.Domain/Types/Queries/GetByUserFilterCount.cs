using MediatR;

namespace Globo.PIC.Domain.Types.Queries
{
    public class GetByUserFilterCount : GetByUserFilter, IRequest<int> { }

}
