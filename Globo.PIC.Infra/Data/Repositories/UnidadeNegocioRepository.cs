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
    public class UnidadeNegocioRepository : Repository<UnidadeNegocio>
    { 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public UnidadeNegocioRepository(PicDbContext context) : base(context) { }

        public override ValueTask<UnidadeNegocio> GetById(long id, CancellationToken cancellationToken)
        {
            var businessUnity = _db.BusinessUnity.Where(a => a.Id == id).FirstOrDefault();
            var retorno = new ValueTask<UnidadeNegocio>(businessUnity);
            return retorno;
        }
 
        public override void Remove(UnidadeNegocio obj, CancellationToken cancellationToken)
        {
            var businessUnity = _db.BusinessUnity.Find(obj.Id);
            if (businessUnity != null)
                Remove(obj);
        }

        public override IQueryable<UnidadeNegocio> GetAll()
        {
            return this._db.BusinessUnity
                .OrderBy(o => o.Id);
        }
        public override void AddOrUpdate(UnidadeNegocio obj, CancellationToken cancellationToken)
        {

            var businessUnityFromDb = _db.BusinessUnity.Find(obj.Id);

            if (businessUnityFromDb != null)
            {
                var typeObj = businessUnityFromDb.GetType();

                foreach (PropertyInfo propertyInfo in typeObj.GetProperties())
                {
                    if (propertyInfo.CanRead)
                    {
                        object firstValue = propertyInfo.GetValue(businessUnityFromDb, null);
                        object secondValue = propertyInfo.GetValue(obj, null);
                        if (!Equals(firstValue, secondValue))
                            propertyInfo.SetValue(businessUnityFromDb, secondValue);
                    }
                }

                obj = businessUnityFromDb;
                Update(obj);
            }
            else
            {
                Add(obj, cancellationToken);
            }
        }
    }
}
