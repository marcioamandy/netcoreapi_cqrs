using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Infra.Data.Context;
using System.Linq;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Enums;

namespace Globo.PIC.Infra.Data.Repositories
{
    public class ConteudoRepository : Repository<Conteudo>
    {
        private readonly IUserProvider userProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public ConteudoRepository(PicDbContext context) : base(context) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public ConteudoRepository(IDbContextFactory<PicDbContext> context) : base(context) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public ConteudoRepository(PicDbContext context, IDbContextFactory<PicDbContext> fcontext, IUserProvider _userProvider) : base(context, fcontext) {
            userProvider = _userProvider;
        }
             
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override ValueTask<Conteudo> GetById(long id, CancellationToken cancellationToken)
        {
            return new ValueTask<Conteudo>(_db.Conteudo
                //.Include(i => i.PedidosConteudo)
                .FirstOrDefaultAsync(u => u.Id.Equals(id), cancellationToken));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="cancellationToken"></param>
        public override void AddOrUpdate(Conteudo obj, CancellationToken cancellationToken)
        {
            var conteudoFromDb = _db.Conteudo.Find(obj.Id);

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
