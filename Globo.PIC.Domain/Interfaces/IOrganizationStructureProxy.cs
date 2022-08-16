 
using Globo.PIC.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Globo.PIC.Domain.Interfaces
{
    public interface IOrganizationStructureProxy : IDisposable
    { 
        Task<List<OrganizationStructure>> GetOrganizationStructures(long businessUnityId,CancellationToken cancellationToken);
    }
}
