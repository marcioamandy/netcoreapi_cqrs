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
    public class PedidoItemEntregaRepository : Repository<PedidoItemArteEntrega>
    {
        public PedidoItemEntregaRepository(PicDbContext context) : base(context) { }

        public override IQueryable<PedidoItemArteEntrega> GetAll()
        {
            return this._db.PedidoItemArteEntrega
                .Include(c => c.Usuario)
                .Include(c => c.PedidoItemArte)
                .OrderBy(o => o.Id);
        }

        public override ValueTask<PedidoItemArteEntrega> GetById(long id, CancellationToken cancellationToken)
        {
            var pedidoItemEntrega = _db.PedidoItemArteEntrega
                .Include(c => c.Usuario)
                .Include(c => c.PedidoItemArte)
                .Where(a => a.Id == id).FirstOrDefault();
            var retorno = new ValueTask<PedidoItemArteEntrega>(pedidoItemEntrega);
            return retorno;
        }

        public override Task<List<PedidoItemArteEntrega>> ListByIdPedidoItemEntrega(long idPedidoItem, CancellationToken cancellationToken)
        {
            var pedidoItemEntrega = _db.PedidoItemArteEntrega
                .Include(c => c.Usuario)
                .Include(c => c.PedidoItemArte)
                .Where(a => a.PedidoItemArte.IdPedidoItem == idPedidoItem).ToList();

            var retorno = Task.FromResult(pedidoItemEntrega);
            return retorno;
        }

        public override ValueTask<PedidoItemArteEntrega> GetByIdPedidoItemEntrega(long idPedidoItemEntrega, CancellationToken cancellationToken)
        {
            var pedidoItemEntrega = _db.PedidoItemArteEntrega
                .Include(c => c.Usuario)
                .Include(c => c.PedidoItemArte)
                .Where(a => a.Id == idPedidoItemEntrega).FirstOrDefault();
            var retorno = new ValueTask<PedidoItemArteEntrega>(pedidoItemEntrega);
            return retorno;
        }

        public override void AddOrUpdate(PedidoItemArteEntrega obj, CancellationToken cancellationToken)
        {

            var pedidoItemEntregaFromDb = _db.PedidoItemArteEntrega.Find(obj.Id);

            if (pedidoItemEntregaFromDb != null)
            {
                var typeObj = pedidoItemEntregaFromDb.GetType();

                foreach (PropertyInfo propertyInfo in typeObj.GetProperties())
                {
                    if (propertyInfo.CanRead)
                    {
                        object firstValue = propertyInfo.GetValue(pedidoItemEntregaFromDb, null);
                        object secondValue = propertyInfo.GetValue(obj, null);
                        if (!Equals(firstValue, secondValue))
                            propertyInfo.SetValue(pedidoItemEntregaFromDb, secondValue);
                    }
                }

                obj = pedidoItemEntregaFromDb;
                Update(obj);
            }
            else
            {
                Add(obj, cancellationToken);
            }
        }
    }
}
