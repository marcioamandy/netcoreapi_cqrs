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
    public class TrackingArteRepository : Repository<PedidoItemArteTracking>
    {

        private readonly DbContext trackingArteRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public TrackingArteRepository(PicDbContext context) : base(context) {
            trackingArteRepository = context;
        }

        public override Task<List<PedidoItemArteTracking>> ListTrackingByIdItemPedido(long idPedidoItem, CancellationToken cancellationToken)
        {
            var tracking = _db.PedidoItemArteTracking
                .Include(s => s.Status)
                .Include(u3 => u3.ChangedBy)
                .Include(p => p.PedidoItemArte)
                .Where(a => a.PedidoItemArte.IdPedidoItem == idPedidoItem
                && a.Ativo == true)
                .OrderBy(o => o.StatusPosition).ToList();

            var retorno = Task.FromResult(tracking);
            return retorno;
        }

        public override IQueryable<PedidoItemArteTracking> GetAll()
        {
            return this._db.PedidoItemArteTracking
                .Include(s => s.Status)
                //.Include(s => s.PedidoItemArte).ThenInclude(a => a.Status)
                .Include(u3 => u3.ChangedBy)
                .Include(p => p.PedidoItemArte)
                .Where(a => a.Ativo == true)
                .OrderBy(o => o.StatusPosition);
            //.Include(c => c.Order);
        }

    }
}