using System.Linq;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Infra.Data.Context;
using System.Threading;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Globo.PIC.Infra.Data.Repositories
{
    public class RCRepository : Repository<RC>
    {

        public RCRepository(PicDbContext context) : base(context) { }

        public override IQueryable<RC> GetAll()
        {
            return _db.RC
                .Include(x => x.PedidoItem).ThenInclude(x => x.Pedido)
                .Include(x => x.PedidoItem).ThenInclude(x => x.PedidoItemArte)
                .Include(x => x.PedidoItem).ThenInclude(x => x.PedidoItemVeiculo);
        }        

        public override void AddOrUpdate(RC obj, CancellationToken cancellationToken)
        {
            var pedidoItemFromDb = _db.RC.Find(obj.Id);

            if (pedidoItemFromDb != null)
            {
                var typeObj = pedidoItemFromDb.GetType();

                foreach (PropertyInfo propertyInfo in typeObj.GetProperties())
                {
                    if (propertyInfo.CanRead)
                    {
                        object firstValue = propertyInfo.GetValue(pedidoItemFromDb, null);
                        object secondValue = propertyInfo.GetValue(obj, null);
                        if (!Equals(firstValue, secondValue))
                            propertyInfo.SetValue(pedidoItemFromDb, secondValue);
                    }
                }

                obj = pedidoItemFromDb;

                Update(obj);
            }
            else
            {
                Add(obj, cancellationToken);
            }
        }
    }
}
