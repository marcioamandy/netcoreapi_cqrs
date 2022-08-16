using Microsoft.EntityFrameworkCore;
using System.Linq;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Infra.Data.Context;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;

namespace Globo.PIC.Infra.Data.Repositories
{
    public class ItemVeiculoRepository : Repository<ItemVeiculo>
    {

        public ItemVeiculoRepository(PicDbContext context) : base(context) { }

        public override IQueryable<ItemVeiculo> GetAll()
        {
            var itemVeiculo = _db.ItemVeiculo.AsQueryable();

            itemVeiculo = SetAllIncludes(itemVeiculo);

            itemVeiculo.OrderBy(o => o.Id);

            return itemVeiculo;
        }

        public async override ValueTask<ItemVeiculo> GetById(long id, CancellationToken cancellationToken)
        {
            var itemVeiculo = _db.ItemVeiculo.AsQueryable();

            itemVeiculo = SetAllIncludes(itemVeiculo);

            return await itemVeiculo.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public override void AddOrUpdate(ItemVeiculo obj, CancellationToken cancellationToken)
        {
            var pedidoItemFromDb = _db.ItemVeiculo.Find(obj.Id);

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
            ItemVeiculo obj = new ItemVeiculo();

            obj.Id = id;
            var pedidoFromDb = _db.ItemVeiculo.Find(obj.Id);

            if (pedidoFromDb != null)
            {
                obj = pedidoFromDb;
                Remove(obj);
            }
        }

        IQueryable<ItemVeiculo> SetAllIncludes(IQueryable<ItemVeiculo> dbItemVeiculo)
        {
            return dbItemVeiculo
                .Include(c => c.Item)
                .Include(c => c.Item).ThenInclude(x => x.ItemVeiculo)
                .Include(c => c.Item).ThenInclude(x => x.Tipo)
                .Include(c => c.Item).ThenInclude(x => x.SubCategoria)
                .Include(a => a.PedidoItemVeiculo).ThenInclude(b => b.PedidoItem);
         }
    }
}
