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
    public class PedidoItemConversaRepository : Repository<PedidoItemConversa>
    {
        public PedidoItemConversaRepository(PicDbContext context) : base(context) { }

        public override IQueryable<PedidoItemConversa> GetAll()
        {
            return this._db.PedidoItemConversa
                .Include(c => c.Arquivos)
                .Include(c => c.Usuario)
                .OrderBy(o => o.Id); 
        }

        public override ValueTask<PedidoItemConversa> GetById(long id, CancellationToken cancellationToken)
        {
            var pedidoItemConversa = _db.PedidoItemConversa
                .Include(c => c.Arquivos)
                .Include(c => c.Usuario)
                .Where(a => a.Id == id).FirstOrDefault();
            var retorno = new ValueTask<PedidoItemConversa>(pedidoItemConversa);
            return retorno;
        }

        public override Task<List<PedidoItemConversa>> ListByIdPedidoItemConversa(long idPedidoItem, CancellationToken cancellationToken)
        {
            var pedidoItemConversa = _db.PedidoItemConversa
                .Include(c => c.Arquivos)
                .Include(c=>c.Usuario)
                .Where(a => a.IdPedidoItem == idPedidoItem).ToList();

            var retorno = Task.FromResult(pedidoItemConversa);
            return retorno;
        }

        public override ValueTask<PedidoItemConversa> GetByIdPedidoItemConversa(long idPedidoItemConversa, CancellationToken cancellationToken)
        {
            var pedidoItemConversa = _db.PedidoItemConversa
                .Include(c => c.Arquivos)
                .Include(c => c.Usuario)
                .Where(a => a.Id == idPedidoItemConversa).FirstOrDefault();
            var retorno = new ValueTask<PedidoItemConversa>(pedidoItemConversa);
            return retorno;
        }

        public override void AddOrUpdate(PedidoItemConversa obj, CancellationToken cancellationToken)
        {

            var pedidoItemConversaFromDb = _db.PedidoItemConversa.Find(obj.Id);

            if (pedidoItemConversaFromDb != null)
            {
                var typeObj = pedidoItemConversaFromDb.GetType();

                foreach (PropertyInfo propertyInfo in typeObj.GetProperties())
                {
                    if (propertyInfo.CanRead)
                    {
                        object firstValue = propertyInfo.GetValue(pedidoItemConversaFromDb, null);
                        object secondValue = propertyInfo.GetValue(obj, null);
                        if (!Equals(firstValue, secondValue))
                            propertyInfo.SetValue(pedidoItemConversaFromDb, secondValue);
                    }
                }

                obj = pedidoItemConversaFromDb;
                Update(obj);
            }
            else
            {
                Add(obj, cancellationToken);
            }
        }
    }
}
