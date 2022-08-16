
using Globo.PIC.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Globo.PIC.Domain.Interfaces
{
    public interface ISupplierProxy : IDisposable
    {
        Task<List<Supplier>> GetSuppliers(string name, CancellationToken cancellationToken);

        Task<Supplier> GetSuppliersByCode(long code, CancellationToken cancellationToken);

        Task<List<Agreements>> GetAgreementBySuppliers(long code, CancellationToken cancellationToken);
    }
}
