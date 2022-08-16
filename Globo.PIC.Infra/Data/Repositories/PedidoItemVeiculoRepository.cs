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
    public class PedidoItemVeiculoRepository : Repository<PedidoItemVeiculo>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public PedidoItemVeiculoRepository(PicDbContext context) : base(context) { }

        public override IQueryable<PedidoItemVeiculo> GetAll()
        {
            var pedidoItemVeiculo = SetAllIncludes(_db.PedidoItemVeiculo);

            pedidoItemVeiculo.OrderBy(o => o.Id);

            return pedidoItemVeiculo;
        }

        public override ValueTask<PedidoItemVeiculo> GetById(long id, CancellationToken cancellationToken)
        {
            var pedidoVeiculos = _db.PedidoItemVeiculo
                .Include(c => c.PedidoItem).ThenInclude(t => t.Arquivos)
                .Include(x => x.Trackings)
                .Include(a => a.Status).ThenInclude(t => t.Tracking).ThenInclude(s => s.Status)
                .Include(d => d.Tipo)
                .Include(d => d.SubCategoria)
                .Include(d => d.PedidoItem).ThenInclude(a => a.AcionamentoItens)
                //.Include(d => d.ItensVeiculo.Where(a => a.DataAprovacao != null)).ThenInclude(i => i.Item)
                .Include(d => d.ItensVeiculo).ThenInclude(i => i.Item)
                .Where(a => a.Id == id).FirstOrDefault();
            var retorno = new ValueTask<PedidoItemVeiculo>(pedidoVeiculos);
            return retorno;
        }

        public override ValueTask<PedidoItemVeiculo> GetByIdPedidoItemVeiculo(long idItem, CancellationToken cancellationToken)
        {
            var pedidoVeiculos = _db.PedidoItemVeiculo
                .Include(c => c.PedidoItem).ThenInclude(t => t.Arquivos)
                .Include(x => x.Trackings)
                .Include(a => a.Status).ThenInclude(t => t.Tracking).ThenInclude(s => s.Status)
                .Include(d => d.Tipo)
                .Include(d => d.SubCategoria)
                .Include(d => d.PedidoItem).ThenInclude(a => a.AcionamentoItens)
                //.Include(d => d.ItensVeiculo.Where(a => a.DataAprovacao != null)).ThenInclude(i => i.Item)
                .Include(d => d.ItensVeiculo).ThenInclude(i => i.Item)
                .Where(a => a.IdPedidoItem == idItem).FirstOrDefault();

            var retorno = new ValueTask<PedidoItemVeiculo>(pedidoVeiculos);
            return retorno;
        }
        /*
        public override ValueTask<PedidoItemVeiculo> GetByIdPedidoItem(long idPedidoItem, CancellationToken cancellationToken)
        {
            var pedidoItem = _db.PedidoItemVeiculo
                .Include(c => c.PedidoItem).ThenInclude(t => t.Arquivos)
                .Include(x => x.TrackingVeiculo)
                .Include(a => a.Status).ThenInclude(t => t.Tracking).ThenInclude(s => s.StatusVeiculo)
                .Where(a => a.Id == idPedidoItem).FirstOrDefault();
            var retorno = new ValueTask<PedidoItemVeiculo>(pedidoItem);
            return retorno;
        }
        */
        public override void AddOrUpdate(PedidoItemVeiculo obj, CancellationToken cancellationToken)
        {
            var pedidoItemFromDb = _db.PedidoItemVeiculo.Include(x => x.PedidoItem).FirstOrDefault(x => x.Id == obj.Id);

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


                //pedidoItemFromDb.PedidoItem.Arquivos = obj.PedidoItem.Arquivos.Select(x =>
                //    {
                //        x.IdPedidoItem = pedidoItemFromDb.IdPedidoItem;
                //        x.PedidoItem = pedidoItemFromDb.PedidoItem;
                //        return x;
                //    }).ToList();


                obj = pedidoItemFromDb;



                Update(obj);
            }
            else
            {
                obj.PedidoItem.IdItem = null;
                obj.PedidoItem.RCs = new List<RC>();
                obj.PedidoItem.Justificativa = "";
                obj.PedidoItem.JustificativaCancelamento = "";
                obj.PedidoItem.JustificativaDevolucao = "";
                obj.PedidoItem.AprovadoPorLogin = null;
                obj.PedidoItem.CanceladoPorLogin = null;
                obj.PedidoItem.DevolvidoPorLogin = null;
                obj.PedidoItem.ReprovadoPorLogin = null;

                Add(obj, cancellationToken);
            }
        }

        public override void DeletarPedidoItemVeiculo(long id, CancellationToken cancellationToken)
        {
            PedidoItemVeiculo obj = new PedidoItemVeiculo();
            obj.Id = id;
            var pedidoFromDb = _db.PedidoItemVeiculo.Find(obj.Id);

            if (pedidoFromDb != null)
            {
                obj = pedidoFromDb;
                Remove(obj);
            }
        }

        IQueryable<PedidoItemVeiculo> SetAllIncludes(IQueryable<PedidoItemVeiculo> dbItemVeiculo)
        {
            return dbItemVeiculo
                .Include(c => c.PedidoItem).ThenInclude(t => t.Arquivos)
                .Include(x => x.Trackings)
                .Include(a => a.Status).ThenInclude(t => t.Tracking).ThenInclude(s => s.Status)
                .Include(d => d.Tipo)
                .Include(d => d.SubCategoria)
                .Include(d => d.PedidoItem).ThenInclude(a => a.AcionamentoItens)
                //.Include(d => d.ItensVeiculo.Where(a => a.DataAprovacao != null)).ThenInclude(i => i.Item)
                .Include(d => d.ItensVeiculo).ThenInclude(i => i.Item);
        }
    }
}
