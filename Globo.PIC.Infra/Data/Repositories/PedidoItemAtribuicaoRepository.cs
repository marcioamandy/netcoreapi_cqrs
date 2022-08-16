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

    public class PedidoItemAtribuicaoRepository : Repository<PedidoItemArteAtribuicao>
    {

        public PedidoItemAtribuicaoRepository(PicDbContext context) : base(context) { }

        public override IQueryable<PedidoItemArteAtribuicao> GetAll()
        {
            return this._db.PedidoItemArteAtribuicao
                .Include(p => p.PedidoItemArte).ThenInclude(a => a.PedidoItem)
                .Include(c => c.Comprador)
                .OrderBy(o => o.Id);
        }

        public override ValueTask<PedidoItemArteAtribuicao> GetById(long id, CancellationToken cancellationToken)
        {
            var pedidoItemAtribuicao = _db.PedidoItemArteAtribuicao
                .Include(p => p.PedidoItemArte).ThenInclude(a => a.PedidoItem)
                .Include(c => c.Comprador)
                .Where(a => a.Id == id).FirstOrDefault();

            var retorno = new ValueTask<PedidoItemArteAtribuicao>(pedidoItemAtribuicao);

            return retorno;
        }

        public override Task<List<PedidoItemArteAtribuicao>> ListByIdPedidoItemAtribuicao(long idPedidoItem, CancellationToken cancellationToken)
        {
            var pedidoItemAtribuicao = _db.PedidoItemArteAtribuicao
                .Include(p => p.PedidoItemArte).ThenInclude(a => a.PedidoItem)
                .Include(c => c.Comprador)
                .Where(a => a.PedidoItemArte.IdPedidoItem == idPedidoItem).ToList();

            var retorno = Task.FromResult(pedidoItemAtribuicao);

            return retorno;
        }

        public override ValueTask<PedidoItemArteAtribuicao> GetByIdPedidoItemAtribuicao(long idPedidoItemAtribuicao, CancellationToken cancellationToken)
        {
            var pedidoItemAtribuicao = _db.PedidoItemArteAtribuicao
                .Include(p => p.PedidoItemArte).ThenInclude(a => a.PedidoItem)
                .Include(c => c.Comprador)
                .Where(a => a.Id == idPedidoItemAtribuicao).FirstOrDefault();

            var retorno = new ValueTask<PedidoItemArteAtribuicao>(pedidoItemAtribuicao);

            return retorno;
        }

        public override void AddOrUpdate(PedidoItemArteAtribuicao obj, CancellationToken cancellationToken)
        {
            var pedidoItemAtribuicaoFromDb = _db.PedidoItemArteAtribuicao.Find(obj.Id);

            if (pedidoItemAtribuicaoFromDb != null)
            {
                var typeObj = pedidoItemAtribuicaoFromDb.GetType();

                foreach (PropertyInfo propertyInfo in typeObj.GetProperties())
                {
                    if (propertyInfo.CanRead)
                    {
                        object firstValue = propertyInfo.GetValue(pedidoItemAtribuicaoFromDb, null);
                        object secondValue = propertyInfo.GetValue(obj, null);
                        if (!Equals(firstValue, secondValue))
                            propertyInfo.SetValue(pedidoItemAtribuicaoFromDb, secondValue);
                    }
                }

                obj = pedidoItemAtribuicaoFromDb;
                Update(obj);
            }
            else
            {
                Add(obj, cancellationToken);
            }
        }
    }
}
