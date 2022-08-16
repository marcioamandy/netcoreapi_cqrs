using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Infra.Data.Context;


namespace Globo.PIC.Infra.Data.Repositories
{
    public class DepartamentoRepository : Repository<Departamento>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public DepartamentoRepository(PicDbContext context) : base(context) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public DepartamentoRepository(IDbContextFactory<PicDbContext> context) : base(context) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public DepartamentoRepository(PicDbContext context, IDbContextFactory<PicDbContext> fcontext) : base(context, fcontext) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override ValueTask<Departamento> GetById(long id, CancellationToken cancellationToken)
        {
            return new ValueTask<Departamento>(_db.Departamento.Include(x => x.Usuarios)
                .FirstOrDefaultAsync(u => u.Id.Equals(id), cancellationToken));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="cancellationToken"></param>
        public override void AddOrUpdate(Departamento obj, CancellationToken cancellationToken)
        {
            var conteudoFromDb = _db.Departamento.Find(obj.Id);

            if (conteudoFromDb != null)
            {
                var typeObj = conteudoFromDb.GetType();

                foreach (PropertyInfo propertyInfo in typeObj.GetProperties())
                {
                    if (propertyInfo.CanRead)
                    {
                        object firstValue = propertyInfo.GetValue(conteudoFromDb, null);
                        object secondValue = propertyInfo.GetValue(obj, null);
                        if (!Equals(firstValue, secondValue))
                        {
                            propertyInfo.SetValue(conteudoFromDb, secondValue);
                        }
                    }
                }

                obj = conteudoFromDb;
                Update(obj);
            }
            else
            {
                Add(obj, cancellationToken);
            }
        }
    }
}
