using Globo.PIC.Domain.Entities;
using Globo.PIC.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;

namespace Globo.PIC.Infra.Data.Repositories
{
    public class PedidoItemArteRepository : Repository<PedidoItemArte>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public PedidoItemArteRepository(PicDbContext context) : base(context) { }

        public override IQueryable<PedidoItemArte> GetAll()
        {
            var pedidoItemArte = _db.PedidoItemArte.AsQueryable();

            pedidoItemArte = SetAllIncludes(pedidoItemArte);

            pedidoItemArte.OrderBy(o => o.Id);

            return pedidoItemArte;
        }

        public override ValueTask<PedidoItemArte> GetById(long id, CancellationToken cancellationToken)
        {
            var pedidoItemArte = _db.PedidoItemArte.AsQueryable();

            pedidoItemArte = SetAllIncludes(pedidoItemArte);

            return new ValueTask<PedidoItemArte>(pedidoItemArte.FirstOrDefaultAsync(x => x.Id == id, cancellationToken));            
        }

        public ValueTask<PedidoItemArte> GetByIdPedidoItem(long idPedidoItem, CancellationToken cancellationToken)
        {
            var pedidoItemArte = _db.PedidoItemArte.AsQueryable();

            pedidoItemArte = SetAllIncludes(pedidoItemArte);

            return new ValueTask<PedidoItemArte>(
                pedidoItemArte.Where(a => a.IdPedidoItem == idPedidoItem).FirstOrDefaultAsync(cancellationToken));
        }

        public override void AddOrUpdate(PedidoItemArte obj, CancellationToken cancellationToken)
        {
            var pedidoItemFromDb = _db.PedidoItemArte.Find(obj.Id);

            if (pedidoItemFromDb != null)
            {
                var typeObj = pedidoItemFromDb.GetType();

                foreach (PropertyInfo propertyInfo in typeObj.GetProperties())
                {
                    if (propertyInfo.CanRead)
                    {
                        object firstValue = propertyInfo.GetValue(pedidoItemFromDb, null);
                        object secondValue = propertyInfo.GetValue(obj, null);
                        if (!Equals(firstValue, secondValue))
                            propertyInfo.SetValue(pedidoItemFromDb, secondValue);
                    }
                }

                obj = pedidoItemFromDb;
                Update(obj);
            }
            else
            {
                Add(obj, cancellationToken);
            }
        }

        IQueryable<PedidoItemArte> SetAllIncludes(IQueryable<PedidoItemArte> dbItemArte)
        {
            return dbItemArte
                .Include(c => c.PedidoItem).ThenInclude(t => t.Arquivos)
                .Include(c => c.PedidoItem).ThenInclude(t => t.RCs)
                .Include(a => a.PedidoItem).ThenInclude(c => c.PedidoItemConversas).ThenInclude(i => i.Arquivos)
                .Include(a => a.PedidoItem).ThenInclude(c => c.PedidoItemConversas).ThenInclude(i=>i.Usuario)
                .Include(x => x.TrackingArte)
                .Include(a => a.Status)
                .Include(i => i.Compras).ThenInclude(d => d.Documentos).ThenInclude(i => i.Arquivos)
                .Include(d => d.Devolucoes)
                .Include(d => d.Atribuicoes)
                .Include(e => e.Entregas)
                .Include(u1 => u1.Comprador);
        }
    }
}
