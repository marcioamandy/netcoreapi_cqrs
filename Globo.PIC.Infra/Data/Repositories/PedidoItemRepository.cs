using Globo.PIC.Domain.Entities;
using Globo.PIC.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;

namespace Globo.PIC.Infra.Data.Repositories
{
    public class PedidoItemRepository : Repository<PedidoItem>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public PedidoItemRepository(PicDbContext context) : base(context) { }

        public override IQueryable<PedidoItem> GetAll()
        {
            var dbPedidoItem = _db.PedidoItem;

            SetAllIncludes(dbPedidoItem);

            return _db.PedidoItem.OrderBy(o => o.Id);

        }

        public override ValueTask<PedidoItem> GetById(long id, CancellationToken cancellationToken)
        {
            var dbPedidoItem = _db.PedidoItem;

            SetAllIncludes(dbPedidoItem);

            return new ValueTask<PedidoItem>(dbPedidoItem.Find(id));            
        }

        public override void AddOrUpdate(PedidoItem obj, CancellationToken cancellationToken)
        {
            var pedidoItemFromDb = _db.PedidoItem.Find(obj.Id);

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

                obj.Pedido = _db.Pedido.Find(obj.IdPedido);
                obj.AcionamentoItens = _db.AcionamentoItem.Where(x => x.IdPedidoItem == obj.Id).ToList();
                obj.PedidoItemConversas = _db.PedidoItemConversa.Where(x => x.IdPedidoItem == obj.Id).ToList();
                obj.PedidoItensFilhos = _db.PedidoItem.Where(x => x.IdPedidoItemPai == obj.Id).ToList();
                obj.PedidoItemArte = _db.PedidoItemArte.Where(x => x.IdPedidoItem == obj.Id).FirstOrDefault();
                obj.PedidoItemVeiculo = _db.PedidoItemVeiculo.Where(x => x.IdPedidoItem == obj.Id).FirstOrDefault();

                obj.Arquivos = obj.Arquivos.Select(x =>
                {
                    x.IdPedidoItem = pedidoItemFromDb.Id;
                    x.PedidoItem = pedidoItemFromDb;
                    return x;
                }).ToList();

                Update(obj);
            }
            else
            {
                Add(obj, cancellationToken);
            }
        }

        public override void DeletarPedidoItem(long id, CancellationToken cancellationToken)
        {
            PedidoItem obj = new PedidoItem();
            obj.Id = id;
            var pedidoFromDb = _db.PedidoItem.Find(obj.Id);

            if (pedidoFromDb != null)
            {
                obj = pedidoFromDb;
                Remove(obj);
            }
        }

        void SetAllIncludes(DbSet<PedidoItem> dbPedidoItem)
        {
            dbPedidoItem
                .Include(c => c.Arquivos)
                .Include(a => a.PedidoItemConversas).ThenInclude(i => i.Arquivos)
                .Include(d => d.PedidoItemArte)
                .Include(d => d.PedidoItemArte).ThenInclude(i => i.Devolucoes)
                .Include(d => d.PedidoItemArte).ThenInclude(i => i.Compras)
                .Include(d => d.PedidoItemArte).ThenInclude(i => i.Compras).ThenInclude(d => d.Documentos).ThenInclude(i => i.Arquivos)
                .Include(d => d.PedidoItemArte).ThenInclude(i => i.Entregas)
                .Include(d => d.PedidoItemArte).ThenInclude(i => i.Comprador)
                .Include(d => d.PedidoItemVeiculo)
                .Include(d => d.PedidoItemVeiculo).ThenInclude(i => i.ItensVeiculo).Where(o => o.DataAprovacao != null)
                .Include(d => d.PedidoItemVeiculo).ThenInclude(i => i.Status).ThenInclude(t => t.Tracking)
                .Include(d => d.PedidoItemVeiculo).ThenInclude(i => i.Tipo)
                .Include(d => d.PedidoItemVeiculo).ThenInclude(i => i.SubCategoria)
                .Include(u2 => u2.CanceladoPor)
                .Include(u3 => u3.DevolvidoPor);
        }
    }
}
