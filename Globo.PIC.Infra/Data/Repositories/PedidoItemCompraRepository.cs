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

    public class PedidoItemCompraRepository : Repository<PedidoItemArteCompra>
    {

        public PedidoItemCompraRepository(PicDbContext context) : base(context) { }

        public override IQueryable<PedidoItemArteCompra> GetAll()
        {
            return this._db.PedidoItemArteCompra
                .Include(c => c.PedidoItemArte)
                .Include(c => c.Documentos).ThenInclude(i => i.Arquivos)
                .Include(c => c.Usuario)
                .OrderBy(o => o.Id);
        }

        public override ValueTask<PedidoItemArteCompra> GetById(long id, CancellationToken cancellationToken)
        {
            var pedidoItemCompra = _db.PedidoItemArteCompra
                .Include(c => c.PedidoItemArte)
                .Include(c => c.Documentos).ThenInclude(i => i.Arquivos)
                .Include(c => c.Usuario)
                .Where(a => a.Id == id).FirstOrDefault();

            var retorno = new ValueTask<PedidoItemArteCompra>(pedidoItemCompra);

            return retorno;
        }

        public override Task<List<PedidoItemArteCompra>> ListByIdPedidoItemCompra(long idPedidoItem, CancellationToken cancellationToken)
        {
            var pedidoItemCompra = _db.PedidoItemArteCompra
                 .Include(c => c.PedidoItemArte)
                .Include(c => c.Documentos).ThenInclude(i => i.Arquivos)
                .Include(c => c.Usuario)
                .Where(a => a.PedidoItemArte.IdPedidoItem == idPedidoItem).ToList();

            var retorno = Task.FromResult(pedidoItemCompra);

            return retorno;
        }

        public override ValueTask<PedidoItemArteCompra> GetByIdPedidoItemCompra(long idPedidoItemCompra, CancellationToken cancellationToken)
        {
            var pedidoItemCompra = _db.PedidoItemArteCompra
                .Include(c => c.PedidoItemArte)
                .Include(c => c.Documentos).ThenInclude(i => i.Arquivos)
                .Include(c => c.Usuario)
                .Where(a => a.Id == idPedidoItemCompra).FirstOrDefault();

            var retorno = new ValueTask<PedidoItemArteCompra>(pedidoItemCompra);

            return retorno;
        }

        public override void AddOrUpdate(PedidoItemArteCompra obj, CancellationToken cancellationToken)
        {
            var pedidoItemCompraFromDb = _db.PedidoItemArteCompra.Find(obj.Id);

            if (pedidoItemCompraFromDb != null)
            {
                var typeObj = pedidoItemCompraFromDb.GetType();

                foreach (PropertyInfo propertyInfo in typeObj.GetProperties())
                {
                    if (propertyInfo.CanRead)
                    {
                        object firstValue = propertyInfo.GetValue(pedidoItemCompraFromDb, null);
                        object secondValue = propertyInfo.GetValue(obj, null);
                        if (!Equals(firstValue, secondValue))
                            propertyInfo.SetValue(pedidoItemCompraFromDb, secondValue);
                    }
                }

                obj = pedidoItemCompraFromDb;

                Update(obj);
            }
            else
            {
                Add(obj, cancellationToken);
            }
        }
    }
}
