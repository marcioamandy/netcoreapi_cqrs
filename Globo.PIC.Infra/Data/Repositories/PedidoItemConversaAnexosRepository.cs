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
    public class PedidoItemConversaAnexosRepository : Repository<PedidoItemConversaAnexo>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public PedidoItemConversaAnexosRepository(PicDbContext context) : base(context) { }

        public override ValueTask<PedidoItemConversaAnexo> GetById(long id, CancellationToken cancellationToken)
        {
            var pedidoItemConversaAnexo = _db.PedidoItemConversaAnexo.Where(a => a.Id == id).FirstOrDefault();
            var retorno = new ValueTask<PedidoItemConversaAnexo>(pedidoItemConversaAnexo);
            return retorno;
        }

        public override ValueTask<PedidoItemConversaAnexo> GetByIdPedidoItemConversaAnexo(long idPedidoItemConversaAnexo, CancellationToken cancellationToken)
        {
            var pedidoItemConversaAnexo = _db.PedidoItemConversaAnexo.Where(a => a.Id == idPedidoItemConversaAnexo).FirstOrDefault();
            var retorno = new ValueTask<PedidoItemConversaAnexo>(pedidoItemConversaAnexo);
            return retorno;
        }

        public override Task<List<PedidoItemConversaAnexo>> ListByIdPedidoItemConversaAnexo(long idPedidoItemConversa, CancellationToken cancellationToken)
        {
            var pedidoItemConversaAnexo = _db.PedidoItemConversaAnexo
                .Where(a => a.IdPedidoItem == idPedidoItemConversa).ToList();

            var retorno = Task.FromResult(pedidoItemConversaAnexo);
            return retorno;
        }
        public override void Remove(PedidoItemConversaAnexo obj, CancellationToken cancellationToken)
        {
            var pedidoItemConversaAnexoDb = _db.PedidoItemConversaAnexo.Find(obj.Id);
            if (pedidoItemConversaAnexoDb != null)
                Remove(obj);
        }

        public override IQueryable<PedidoItemConversaAnexo> GetAll()
        {
            return this._db.PedidoItemConversaAnexo
                //.Include(c => c.Status)
                .OrderBy(o => o.Id);
            //.Include(c => c.Order);
        }
        public override void AddOrUpdate(PedidoItemConversaAnexo obj, CancellationToken cancellationToken)
        {

            var pedidoItemConversaAnexoFromDb = _db.PedidoItemConversaAnexo.Find(obj.Id);

            if (pedidoItemConversaAnexoFromDb != null)
            {
                var typeObj = pedidoItemConversaAnexoFromDb.GetType();

                foreach (PropertyInfo propertyInfo in typeObj.GetProperties())
                {
                    if (propertyInfo.CanRead)
                    {
                        object firstValue = propertyInfo.GetValue(pedidoItemConversaAnexoFromDb, null);
                        object secondValue = propertyInfo.GetValue(obj, null);
                        if (!Equals(firstValue, secondValue))
                            propertyInfo.SetValue(pedidoItemConversaAnexoFromDb, secondValue);
                    }
                }

                obj = pedidoItemConversaAnexoFromDb;
                Update(obj);
            }
            else
            {
                Add(obj, cancellationToken);
            }
        }
    }
}
