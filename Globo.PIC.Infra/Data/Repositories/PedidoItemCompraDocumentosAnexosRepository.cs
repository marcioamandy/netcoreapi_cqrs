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
    public class PedidoItemCompraDocumentosAnexosRepository : Repository<PedidoItemArteCompraDocumentoAnexo>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public PedidoItemCompraDocumentosAnexosRepository(PicDbContext context) : base(context) { }

        public override ValueTask<PedidoItemArteCompraDocumentoAnexo> GetById(long id, CancellationToken cancellationToken)
        {
            var pedidoItemCompraDocumentosAnexo = _db.PedidoItemArteCompraDocumentoAnexo.Where(a => a.Id == id).FirstOrDefault();
            var retorno = new ValueTask<PedidoItemArteCompraDocumentoAnexo>(pedidoItemCompraDocumentosAnexo);
            return retorno;
        }

        public override ValueTask<PedidoItemArteCompraDocumentoAnexo> GetByIdPedidoItemCompraDocumentosAnexos(long idPedidoItemCompraDocumentosAnexo, CancellationToken cancellationToken)
        {
            var pedidoItemCompraDocumentosAnexo = _db.PedidoItemArteCompraDocumentoAnexo.Where(a => a.Id == idPedidoItemCompraDocumentosAnexo).FirstOrDefault();
            var retorno = new ValueTask<PedidoItemArteCompraDocumentoAnexo>(pedidoItemCompraDocumentosAnexo);
            return retorno;
        }

        public override Task<List<PedidoItemArteCompraDocumentoAnexo>> ListByIdPedidoItemCompraDocumentosAnexos(long idPedidoItemCompraDocumento, CancellationToken cancellationToken)
        {
            var pedidoItemCompraDocumentosAnexo = _db.PedidoItemArteCompraDocumentoAnexo
                .Where(a => a.IdDocumento == idPedidoItemCompraDocumento).ToList();

            var retorno = Task.FromResult(pedidoItemCompraDocumentosAnexo);
            return retorno;
        }
        public override void Remove(PedidoItemArteCompraDocumentoAnexo obj, CancellationToken cancellationToken)
        {
            var pedidoItemCompraDocumentosAnexoDb = _db.PedidoItemArteCompraDocumentoAnexo.Find(obj.Id);
            if (pedidoItemCompraDocumentosAnexoDb != null)
                Remove(obj);
        }

        public override IQueryable<PedidoItemArteCompraDocumentoAnexo> GetAll()
        {
            return this._db.PedidoItemArteCompraDocumentoAnexo
                //.Include(c => c.Status)
                .OrderBy(o => o.Id);
            //.Include(c => c.Order);
        }
        public override void AddOrUpdate(PedidoItemArteCompraDocumentoAnexo obj, CancellationToken cancellationToken)
        {

            var pedidoItemCompraDocumentosAnexoFromDb = _db.PedidoItemArteCompraDocumentoAnexo.Find(obj.Id);

            if (pedidoItemCompraDocumentosAnexoFromDb != null)
            {
                var typeObj = pedidoItemCompraDocumentosAnexoFromDb.GetType();

                foreach (PropertyInfo propertyInfo in typeObj.GetProperties())
                {
                    if (propertyInfo.CanRead)
                    {
                        object firstValue = propertyInfo.GetValue(pedidoItemCompraDocumentosAnexoFromDb, null);
                        object secondValue = propertyInfo.GetValue(obj, null);
                        if (!Equals(firstValue, secondValue))
                            propertyInfo.SetValue(pedidoItemCompraDocumentosAnexoFromDb, secondValue);
                    }
                }

                obj = pedidoItemCompraDocumentosAnexoFromDb;
                Update(obj);
            }
            else
            {
                Add(obj, cancellationToken);
            }
        }
    }
}
