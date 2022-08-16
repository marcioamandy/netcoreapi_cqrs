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
    public class PedidoItemCompraDocumentosRepository : Repository<PedidoItemArteCompraDocumento>
    {
        public PedidoItemCompraDocumentosRepository(PicDbContext context) : base(context) { }

        public override IQueryable<PedidoItemArteCompraDocumento> GetAll()
        {
            return this._db.PedidoItemArteCompraDocumento
                .Include(c => c.Arquivos)
                .Include(c => c.User)
                .OrderBy(o => o.Id);
        }

        public override ValueTask<PedidoItemArteCompraDocumento> GetById(long id, CancellationToken cancellationToken)
        {
            var pedidoItemCompraDocumentos = _db.PedidoItemArteCompraDocumento
                .Include(c => c.Arquivos)
                .Include(c => c.User)
                .Where(a => a.Id == id).FirstOrDefault();
            var retorno = new ValueTask<PedidoItemArteCompraDocumento>(pedidoItemCompraDocumentos);
            return retorno;
        }

        public override Task<List<PedidoItemArteCompraDocumento>> ListByIdPedidoItemCompraDocumentos(long idPedidoItemCompra, CancellationToken cancellationToken)
        {
            var pedidoItemCompraDocumentos = _db.PedidoItemArteCompraDocumento
                .Include(c => c.Arquivos)
                .Include(c => c.User)
                .Where(a => a.IdCompra == idPedidoItemCompra).ToList();

            var retorno = Task.FromResult(pedidoItemCompraDocumentos);
            return retorno;
        }

        public override ValueTask<PedidoItemArteCompraDocumento> GetByIdPedidoItemCompraDocumentos(long idPedidoItemCompraDocumentos, CancellationToken cancellationToken)
        {
            var pedidoItemCompraDocumentos = _db.PedidoItemArteCompraDocumento
                .Include(c => c.Arquivos)
                .Include(c => c.User)
                .Where(a => a.Id == idPedidoItemCompraDocumentos).FirstOrDefault();
            var retorno = new ValueTask<PedidoItemArteCompraDocumento>(pedidoItemCompraDocumentos);
            return retorno;
        }

        public override void AddOrUpdate(PedidoItemArteCompraDocumento obj, CancellationToken cancellationToken)
        {

            var pedidoItemCompraDocumentosFromDb = _db.PedidoItemArteCompraDocumento.Find(obj.Id);

            if (pedidoItemCompraDocumentosFromDb != null)
            {
                var typeObj = pedidoItemCompraDocumentosFromDb.GetType();

                foreach (PropertyInfo propertyInfo in typeObj.GetProperties())
                {
                    if (propertyInfo.CanRead)
                    {
                        object firstValue = propertyInfo.GetValue(pedidoItemCompraDocumentosFromDb, null);
                        object secondValue = propertyInfo.GetValue(obj, null);
                        if (!Equals(firstValue, secondValue))
                            propertyInfo.SetValue(pedidoItemCompraDocumentosFromDb, secondValue);
                    }
                }

                obj = pedidoItemCompraDocumentosFromDb;
                Update(obj);
            }
            else
            {
                Add(obj, cancellationToken);
            }
        }
    }
}
