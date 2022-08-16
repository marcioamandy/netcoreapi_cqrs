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
    public class StatusVeiculoRepository : Repository<StatusPedidoVeiculo>
    {
        public StatusVeiculoRepository(PicDbContext context) : base(context) { }

		public override ValueTask<StatusPedidoVeiculo> GetById(long id, CancellationToken cancellationToken)
		{
			return new ValueTask<StatusPedidoVeiculo>(_db.StatusPedidoVeiculo
				.FirstOrDefaultAsync(p => p.Id.Equals(id)));
		}

		public override IQueryable<StatusPedidoVeiculo> GetAll()
		{
			return this._db.StatusPedidoVeiculo
				.OrderBy(o => o.Id);
		}
	}
}
