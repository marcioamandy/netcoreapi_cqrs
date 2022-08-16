using Globo.PIC.Domain.Entities;
using Globo.PIC.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Generic;

namespace Globo.PIC.Infra.Data.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public class TrackingVeiculoRepository : Repository<PedidoItemVeiculoTracking>
    {

        private readonly DbContext trackingVeiculosRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public TrackingVeiculoRepository(PicDbContext context) : base(context) {
            trackingVeiculosRepository = context;
        }

        public override Task<List<PedidoItemVeiculoTracking>> ListTrackingVeiculos(long idPedidoItem, CancellationToken cancellationToken)
        {
            var tracking = _db.PedidoItemVeiculoTracking
                .Include(s => s.Status)
                .Include(u3 => u3.ChangedBy)
                .Include(p => p.PedidoItemVeiculo)
                .Where(a => a.PedidoItemVeiculo.IdPedidoItem == idPedidoItem
                && a.Ativo == true)
                .OrderBy(o => o.StatusPosition).ToList();

            var retorno = Task.FromResult(tracking);
            return retorno;
        }

        public override IQueryable<PedidoItemVeiculoTracking> GetAll()
        {
            return this._db.PedidoItemVeiculoTracking
                .Include(s => s.Status)
                .Include(u3 => u3.ChangedBy)
                .Include(p => p.PedidoItemVeiculo)
                .Where(a => a.Ativo == true)
                .OrderBy(o => o.StatusPosition);				
                //.Include(c => c.Order);
        }

    }
}