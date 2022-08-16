using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Infra.Data.Context;

namespace Globo.PIC.Infra.Data.Repositories
{
    public class StatusArteRepository : Repository<StatusPedidoArte>
    {
        public StatusArteRepository(PicDbContext context) : base(context) { }

		public override ValueTask<StatusPedidoArte> GetById(long id, CancellationToken cancellationToken)
		{
			return new ValueTask<StatusPedidoArte>(_db.StatusPedidoArte
				.FirstOrDefaultAsync(p => p.Id.Equals(id)));
		}

		public override IQueryable<StatusPedidoArte> GetAll()
		{
			return this._db.StatusPedidoArte
				.OrderBy(o => o.Id);
		} 
    }
}
