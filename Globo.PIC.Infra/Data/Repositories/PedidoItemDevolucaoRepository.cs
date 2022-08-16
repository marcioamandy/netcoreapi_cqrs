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

    public class PedidoItemDevolucaoRepository : Repository<PedidoItemArteDevolucao>
    {

        public PedidoItemDevolucaoRepository(PicDbContext context) : base(context) { }

        public override IQueryable<PedidoItemArteDevolucao> GetAll()
        {
            return this._db.PedidoItemArteDevolucao
                .Include(p => p.PedidoItemArte).ThenInclude(a => a.PedidoItem)
                .Include(c => c.Comprador)
                .OrderBy(o => o.Id);
        }

        public override ValueTask<PedidoItemArteDevolucao> GetById(long id, CancellationToken cancellationToken)
        {
            var pedidoItemDevolucao = _db.PedidoItemArteDevolucao
                .Include(p => p.PedidoItemArte).ThenInclude(a => a.PedidoItem)
                .Include(c => c.Comprador)
                .Where(a => a.Id == id).FirstOrDefault();

            var retorno = new ValueTask<PedidoItemArteDevolucao>(pedidoItemDevolucao);

            return retorno;
        }

        public override Task<List<PedidoItemArteDevolucao>> ListByIdPedidoItemDevolucao(long idPedidoItem, CancellationToken cancellationToken)
        {
            var pedidoItemDevolucao = _db.PedidoItemArteDevolucao
                .Include(p => p.PedidoItemArte).ThenInclude(a => a.PedidoItem)
                .Include(c => c.Comprador)
                .Where(a => a.PedidoItemArte.IdPedidoItem == idPedidoItem).ToList();

            var retorno = Task.FromResult(pedidoItemDevolucao);

            return retorno;
        }

        public override Task<List<PedidoItemArteDevolucao>> ListByIdPedidoItemOriginalDevolucao(long idPedidoItemOriginal, CancellationToken cancellationToken)
        {
            var pedidoItemDevolucao = _db.PedidoItemArteDevolucao
                .Include(p => p.PedidoItemArte).ThenInclude(a => a.PedidoItem)
                .Include(c => c.Comprador)
               .Where(a => a.IdPedidoItemArteOriginal == idPedidoItemOriginal).ToList();

            var retorno = Task.FromResult(pedidoItemDevolucao);

            return retorno;
        }

        public override ValueTask<PedidoItemArteDevolucao> GetByIdPedidoItemDevolucao(long idPedidoItemDevolucao, CancellationToken cancellationToken)
        {
            var pedidoItemDevolucao = _db.PedidoItemArteDevolucao
                .Include(p => p.PedidoItemArte).ThenInclude(a => a.PedidoItem)
                .Include(c => c.Comprador)
                .Where(a => a.Id == idPedidoItemDevolucao).FirstOrDefault();

            var retorno = new ValueTask<PedidoItemArteDevolucao>(pedidoItemDevolucao);

            return retorno;
        }

        public override void AddOrUpdate(PedidoItemArteDevolucao obj, CancellationToken cancellationToken)
        {
            var pedidoItemDevolucaoFromDb = _db.PedidoItemArteDevolucao.Find(obj.Id);

            if (pedidoItemDevolucaoFromDb != null)
            {
                var typeObj = pedidoItemDevolucaoFromDb.GetType();

                foreach (PropertyInfo propertyInfo in typeObj.GetProperties())
                {
                    if (propertyInfo.CanRead)
                    {
                        object firstValue = propertyInfo.GetValue(pedidoItemDevolucaoFromDb, null);
                        object secondValue = propertyInfo.GetValue(obj, null);
                        if (!Equals(firstValue, secondValue))
                            propertyInfo.SetValue(pedidoItemDevolucaoFromDb, secondValue);
                    }
                }

                obj = pedidoItemDevolucaoFromDb;
                Update(obj);
            }
            else
            {
                Add(obj, cancellationToken);
            }
        }
    }
}
