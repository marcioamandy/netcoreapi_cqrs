using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Infra.Data.Context;

namespace Globo.PIC.Infra.Data.Repositories
{
    public class PedidoEquipeRepository : Repository<Equipe>
    {
        public PedidoEquipeRepository(PicDbContext context) : base(context) { }

        public Task<List<Equipe>> ListByIdPedido(long idPedido, CancellationToken cancellationToken)
        {
            var pedido = _db.Equipe
                .Include(i => i.Usuario).ThenInclude(i => i.Roles)
                .Where(a => a.IdPedido == idPedido).ToList();
            var retorno = Task.FromResult(pedido);
            return retorno;
        }

        public override ValueTask<Equipe> GetById(long id, CancellationToken cancellationToken)
        {
            var pedidoEquipe = _db.Equipe
                 .Include(i => i.Usuario).ThenInclude(i => i.Roles)
                 .Where(a => a.Id == id).FirstOrDefault();
            var retorno = new ValueTask<Equipe>(pedidoEquipe);
            return retorno;
        }
        public override IQueryable<Equipe> GetAll()
        {
            return this._db.Equipe
                .Include(i => i.Usuario).ThenInclude(i => i.Roles)
                .OrderBy(o => o.Id);
        }
        public override void Remove(Equipe obj, CancellationToken cancellationToken)
        { 
            var pedidoEquipeFromDb = _db.Pedido.Find(obj.Id);
            if (pedidoEquipeFromDb != null)
                Remove(obj);
        }
        public override void AddOrUpdate(Equipe obj, CancellationToken cancellationToken)
        {

            var pedidoEquipeFromDb = _db.Equipe.Find(obj.Id);

            if (pedidoEquipeFromDb != null)
            {
                var typeObj = pedidoEquipeFromDb.GetType();

                foreach (PropertyInfo propertyInfo in typeObj.GetProperties())
                {
                    if (propertyInfo.CanRead)
                    {
                        object firstValue = propertyInfo.GetValue(pedidoEquipeFromDb, null);
                        object secondValue = propertyInfo.GetValue(obj, null);
                        if (!Equals(firstValue, secondValue))
                            propertyInfo.SetValue(pedidoEquipeFromDb, secondValue);
                    }
                }

                obj = pedidoEquipeFromDb;
                Update(obj);
            }
            else
            {
                Add(obj, cancellationToken);
            }
        }
    }
}
