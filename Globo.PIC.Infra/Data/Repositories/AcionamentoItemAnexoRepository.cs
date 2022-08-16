using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Enums;
using Globo.PIC.Infra.Data.Context;
using Globo.PIC.Domain.Interfaces;

namespace Globo.PIC.Infra.Data.Repositories
{
    public class AcionamentoItemAnexoRepository : Repository<AcionamentoItemAnexo>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public AcionamentoItemAnexoRepository(PicDbContext context) : base(context) { }

        public override ValueTask<AcionamentoItemAnexo> GetById(long id, CancellationToken cancellationToken)
        {
            var acionamentoItemImagem = _db.AcionamentoItemAnexo.Where(a => a.Id == id).FirstOrDefault();
            var retorno = new ValueTask<AcionamentoItemAnexo>(acionamentoItemImagem);
            return retorno;
        }

        public override ValueTask<AcionamentoItemAnexo> GetByIdPedidoItemAnexo(long idPedidoItemImagem, CancellationToken cancellationToken)
        {
            var acionamentoItemImagem = _db.AcionamentoItemAnexo.Where(a => a.Id == idPedidoItemImagem).FirstOrDefault();
            var retorno = new ValueTask<AcionamentoItemAnexo>(acionamentoItemImagem);
            return retorno;
        }

        public override Task<List<AcionamentoItemAnexo>> ListByIdPedidoItemAnexo(long idAcionamentoItem, CancellationToken cancellationToken)
        {
            var acionamentoItemAnexos = _db.AcionamentoItemAnexo
                .Where(a => a.IdAcionamentoItem == idAcionamentoItem).ToList();

            var retorno = Task.FromResult(acionamentoItemAnexos);
            return retorno;
        }
        public override void Remove(AcionamentoItemAnexo obj, CancellationToken cancellationToken)
        {
            var acionamentoItemImagemDb = _db.AcionamentoItemAnexo.Find(obj.Id);
            if (acionamentoItemImagemDb != null)
                Remove(obj);
        }

        public override IQueryable<AcionamentoItemAnexo> GetAll()
        {
            return this._db.AcionamentoItemAnexo
                //.Include(c => c.Status)
                .OrderBy(o => o.Id);
            //.Include(c => c.Order);
        }
        public override void AddOrUpdate(AcionamentoItemAnexo obj, CancellationToken cancellationToken)
        {

            var acionamentoItemImagemFromDb = _db.AcionamentoItemAnexo.Find(obj.Id);

            if (acionamentoItemImagemFromDb != null)
            {
                var typeObj = acionamentoItemImagemFromDb.GetType();

                foreach (PropertyInfo propertyInfo in typeObj.GetProperties())
                {
                    if (propertyInfo.CanRead)
                    {
                        object firstValue = propertyInfo.GetValue(acionamentoItemImagemFromDb, null);
                        object secondValue = propertyInfo.GetValue(obj, null);
                        if (!Equals(firstValue, secondValue))
                            propertyInfo.SetValue(acionamentoItemImagemFromDb, secondValue);
                    }
                }

                obj = acionamentoItemImagemFromDb;
                Update(obj);
            }
            else
            {
                Add(obj, cancellationToken);
            }
        }
    }
}
