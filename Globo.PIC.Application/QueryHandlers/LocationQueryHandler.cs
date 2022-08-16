using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Models;
using Globo.PIC.Domain.Types.Queries;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Globo.PIC.Application.QueryHandlers
{
    public class LocationQueryHandler :
        IRequestHandler<GetByLocationsFilter, Location>
    {
        private readonly IHCMProxy HCM;

        public LocationQueryHandler(IHCMProxy _HCM)
        {
            HCM = _HCM;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task<Location> IRequestHandler<GetByLocationsFilter, Location>.Handle(GetByLocationsFilter request, CancellationToken cancellationToken)
        {
            string param = "";

            if (!string.IsNullOrWhiteSpace(request.OrganizationName))
                param = "InventoryOrganizationName=" + request.OrganizationName.ToUpper() ;
             

            var locationResult = await HCM.GetResultLocationsAsync(param, cancellationToken);

            Location location = new Location();
            location.items = new List<items>();
            ///filtrando apenas para GCP_OI_RJ e GCP_OI_SP
            foreach (var loc in locationResult.items.Where(a=>a.InventoryOrganizationName!=null).OrderBy(a=>a.InventoryOrganizationName))
            {
                if (loc.InventoryOrganizationName.Equals("GCP_OI_RJ") ||
                    loc.InventoryOrganizationName.Equals("GCP_OI_SP"))
                {
                    location.items.Add(loc);
                }
            }

            return location;
        }
    }
}
