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

    public class StatusArteItemRepository : Repository<StatusPedidoItemArte>
    {
        public StatusArteItemRepository(PicDbContext context) : base(context) { }

		public override ValueTask<StatusPedidoItemArte> GetById(long id, CancellationToken cancellationToken)
		{
			return new ValueTask<StatusPedidoItemArte>(_db.StatusPedidoItemArte
				.FirstOrDefaultAsync(p => p.Id.Equals(id)));
		}

		public override IQueryable<StatusPedidoItemArte> GetAll()
		{
			return this._db.StatusPedidoItemArte
				.OrderBy(o => o.Id);
		}
	}
}
