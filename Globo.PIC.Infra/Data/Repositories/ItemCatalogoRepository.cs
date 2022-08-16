using Microsoft.EntityFrameworkCore;
using System.Linq;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Infra.Data.Context;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;

namespace Globo.PIC.Infra.Data.Repositories
{
    public class ItemCatalogoRepository : Repository<ItemCatalogo>
    {

        public ItemCatalogoRepository(PicDbContext context) : base(context) { }

        public override IQueryable<ItemCatalogo> GetAll()
        {
            var itemCatalogo = _db.ItemCatalogo.AsQueryable();

            itemCatalogo = SetAllIncludes(itemCatalogo);

            itemCatalogo.OrderBy(o => o.Id);

            return itemCatalogo;
        }

        public async override ValueTask<ItemCatalogo> GetById(long id, CancellationToken cancellationToken)
        {
            var itemCatalogo = _db.ItemCatalogo.AsQueryable();

            itemCatalogo = SetAllIncludes(itemCatalogo);

            return await itemCatalogo.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public override void AddOrUpdate(ItemCatalogo obj, CancellationToken cancellationToken)
        {
            var pedidoItemFromDb = _db.ItemCatalogo.Find(obj.Id);

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
            ItemCatalogo obj = new ItemCatalogo();

            obj.Id = id;
            var pedidoFromDb = _db.ItemCatalogo.Find(obj.Id);

            if (pedidoFromDb != null)
            {
                obj = pedidoFromDb;
                Remove(obj);
            }
        }

        IQueryable<ItemCatalogo> SetAllIncludes(IQueryable<ItemCatalogo> dbItemCatalogo)
        {
            return dbItemCatalogo
                .Include(c => c.Item)
                .Include(c => c.Item).ThenInclude(x => x.ItemVeiculo).ThenInclude(y => y.PedidoItemVeiculo).ThenInclude(z => z.PedidoItem)
                .Include(c => c.Item).ThenInclude(x => x.Tipo)
                .Include(c => c.Item).ThenInclude(x => x.SubCategoria);
         }
    }
}
