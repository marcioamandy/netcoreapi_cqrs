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
    public class PedidoAnexosRepository : Repository<PedidoAnexo>
    { 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public PedidoAnexosRepository(PicDbContext context) : base(context) { }

        public override ValueTask<PedidoAnexo> GetById(long id, CancellationToken cancellationToken)
        {
            var pedidoAnexos = _db.PedidoAnexo.Where(a => a.Id == id).FirstOrDefault();
            var retorno = new ValueTask<PedidoAnexo>(pedidoAnexos);
            return retorno;
        }

        public override ValueTask<PedidoAnexo> GetByIdPedidoAnexos(long idPedidoAnexos, CancellationToken cancellationToken)
        {
            var pedidoAnexos = _db.PedidoAnexo.Where(a => a.Id == idPedidoAnexos).FirstOrDefault();
            var retorno = new ValueTask<PedidoAnexo>(pedidoAnexos);
            return retorno;
        }

        public override Task<List<PedidoAnexo>> ListByIdPedidoAnexos(long idPedido, CancellationToken cancellationToken)
        {
            var pedidoAnexos = _db.PedidoAnexo
                .Where(a => a.IdPedido == idPedido).ToList();

            var retorno = Task.FromResult(pedidoAnexos);
            return retorno;
        }
        public override void Remove(PedidoAnexo obj, CancellationToken cancellationToken)
        {
            var pedidoAnexosDb = _db.PedidoAnexo.Find(obj.Id);
            if (pedidoAnexosDb != null)
                Remove(obj);
        }

        public override IQueryable<PedidoAnexo> GetAll()
        {
            return this._db.PedidoAnexo
                //.Include(c => c.Status)
                .OrderBy(o => o.Id);
            //.Include(c => c.Order);
        }
        public override void AddOrUpdate(PedidoAnexo obj, CancellationToken cancellationToken)
        {

            var pedidoAnexosFromDb = _db.PedidoAnexo.Find(obj.Id);

            if (pedidoAnexosFromDb != null)
            {
                var typeObj = pedidoAnexosFromDb.GetType();

                foreach (PropertyInfo propertyInfo in typeObj.GetProperties())
                {
                    if (propertyInfo.CanRead)
                    {
                        object firstValue = propertyInfo.GetValue(pedidoAnexosFromDb, null);
                        object secondValue = propertyInfo.GetValue(obj, null);
                        if (!Equals(firstValue, secondValue))
                            propertyInfo.SetValue(pedidoAnexosFromDb, secondValue);
                    }
                }

                obj = pedidoAnexosFromDb;
                Update(obj);
            }
            else
            {
                Add(obj, cancellationToken);
            }
        }
    }
}
