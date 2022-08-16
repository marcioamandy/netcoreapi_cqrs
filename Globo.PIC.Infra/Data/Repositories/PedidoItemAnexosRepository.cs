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
    public class PedidoItemAnexosRepository : Repository<PedidoItemAnexo>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public PedidoItemAnexosRepository(PicDbContext context) : base(context) { }

        public override ValueTask<PedidoItemAnexo> GetById(long id, CancellationToken cancellationToken)
        {
            var pedidoItemImagem = _db.PedidoItemAnexo.Where(a => a.Id == id).FirstOrDefault();
            var retorno = new ValueTask<PedidoItemAnexo>(pedidoItemImagem);
            return retorno;
        }

        public override ValueTask<PedidoItemAnexo> GetByIdPedidoItemAnexo(long idPedidoItemImagem, CancellationToken cancellationToken)
        {
            var pedidoItemImagem = _db.PedidoItemAnexo.Where(a => a.Id == idPedidoItemImagem).FirstOrDefault();
            var retorno = new ValueTask<PedidoItemAnexo>(pedidoItemImagem);
            return retorno;
        }

        public override Task<List<PedidoItemAnexo>> ListByIdPedidoItemAnexo(long idPedidoItem, CancellationToken cancellationToken)
        {
            var pedidoItemAnexos = _db.PedidoItemAnexo
                .Where(a => a.IdPedidoItem == idPedidoItem).ToList();

            var retorno = Task.FromResult(pedidoItemAnexos);
            return retorno;
        }
        public override void Remove(PedidoItemAnexo obj, CancellationToken cancellationToken)
        {
            var pedidoItemImagemDb = _db.PedidoItemAnexo.Find(obj.Id);
            if (pedidoItemImagemDb != null)
                Remove(obj);
        }

        public override IQueryable<PedidoItemAnexo> GetAll()
        {
            return this._db.PedidoItemAnexo
                //.Include(c => c.Status)
                .OrderBy(o => o.Id);
            //.Include(c => c.Order);
        }
        public override void AddOrUpdate(PedidoItemAnexo obj, CancellationToken cancellationToken)
        {

            var pedidoItemImagemFromDb = _db.PedidoItemAnexo.Find(obj.Id);

            if (pedidoItemImagemFromDb != null)
            {
                var typeObj = pedidoItemImagemFromDb.GetType();

                foreach (PropertyInfo propertyInfo in typeObj.GetProperties())
                {
                    if (propertyInfo.CanRead)
                    {
                        object firstValue = propertyInfo.GetValue(pedidoItemImagemFromDb, null);
                        object secondValue = propertyInfo.GetValue(obj, null);
                        if (!Equals(firstValue, secondValue))
                            propertyInfo.SetValue(pedidoItemImagemFromDb, secondValue);
                    }
                }

                obj = pedidoItemImagemFromDb;
                Update(obj);
            }
            else
            {
                Add(obj, cancellationToken);
            }
        }
    }
}
