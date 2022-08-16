using Microsoft.EntityFrameworkCore;
using System.Linq;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Infra.Data.Context;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;

namespace Globo.PIC.Infra.Data.Repositories
{
    public class ItemRepository : Repository<Item>
    {

        public ItemRepository(PicDbContext context) : base(context) { }

        public override IQueryable<Item> GetAll()
        {
            var item = _db.Item.AsQueryable();

            item = SetAllIncludes(item);

            item.OrderBy(o => o.Id);

            return item;
        }

        public async override ValueTask<Item> GetById(long id, CancellationToken cancellationToken)
        {
            var item = _db.Item.AsQueryable();

            item = SetAllIncludes(item);

            return await item.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public override void AddOrUpdate(Item obj, CancellationToken cancellationToken)
        {
            var pedidoItemFromDb = _db.Item.Find(obj.Id);

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

        public void Deletar(long id, CancellationToken cancellationToken)
        {
            Item obj = new Item();

            obj.Id = id;
            var pedidoFromDb = _db.Item.Find(obj.Id);

            if (pedidoFromDb != null)
            {
                obj = pedidoFromDb;
                Remove(obj);
            }
        }

        IQueryable<Item> SetAllIncludes(IQueryable<Item> dbItem)
        {
            return dbItem
                .Include(c => c.ItemVeiculo)
                .Include(c => c.Tipo)
                .Include(c => c.SubCategoria);
         }
    }
}
