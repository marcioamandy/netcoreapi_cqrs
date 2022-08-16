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

    public class StatusVeiculoItemRepository : Repository<StatusPedidoItemVeiculo>
    {
        public StatusVeiculoItemRepository(PicDbContext context) : base(context) { }

		public override ValueTask<StatusPedidoItemVeiculo> GetById(long id, CancellationToken cancellationToken)
		{
			return new ValueTask<StatusPedidoItemVeiculo>(_db.StatusPedidoItemVeiculo
				.FirstOrDefaultAsync(p => p.Id.Equals(id)));
		}

		public override IQueryable<StatusPedidoItemVeiculo> GetAll()
		{
			return this._db.StatusPedidoItemVeiculo
				.OrderBy(o => o.Id);
		}
	}
}
