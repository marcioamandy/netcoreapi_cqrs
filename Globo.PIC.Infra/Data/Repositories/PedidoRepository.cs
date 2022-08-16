using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Enums;
using Globo.PIC.Infra.Data.Context;
using Globo.PIC.Domain.Interfaces;
using System;

namespace Globo.PIC.Infra.Data.Repositories
{

    public class PedidoRepository : Repository<Pedido>
    {
        private readonly IUserProvider userProvider;

        public PedidoRepository(PicDbContext context, IUserProvider _userProvider) : base(context)
        {
            userProvider = _userProvider;
        }

        public override void AddOrUpdate(Pedido obj, CancellationToken cancellationToken)
        {
            var pedidoFromDb = _db.Pedido.Find(obj.Id);

            if (pedidoFromDb != null)
            {

                var typeObj = pedidoFromDb.GetType();

                foreach (PropertyInfo propertyInfo in typeObj.GetProperties())
                {
                    if (propertyInfo.CanRead)
                    {
                        object firstValue = propertyInfo.GetValue(pedidoFromDb, null);
                        object secondValue = propertyInfo.GetValue(obj, null);
                        if (!Equals(firstValue, secondValue))
                        {
                            propertyInfo.SetValue(pedidoFromDb, secondValue);
                        }
                    }
                }

                obj = pedidoFromDb;

                if (userProvider.User != null)
                    obj.AtualizadoPorLogin = userProvider.User?.Login;

                Update(obj);
            }
            else
            {
                obj.CriadoPorLogin = userProvider.User.Login;
                obj.DataPedido = System.DateTime.Now;
                Add(obj, cancellationToken);
            }

        }

        public override void DeletarPedido(long id, CancellationToken cancellationToken)
        {
            Pedido obj = new Pedido();
            obj.Id = id;

            var pedidoFromDb = _db.Pedido.Find(obj.Id);
            var pedidoArteFromDb = _db.PedidoArte.Where(a => a.IdPedido == obj.Id).FirstOrDefault();
            var pedidoVeiculoFromDb = _db.PedidoVeiculo.Where(a => a.IdPedido == obj.Id).FirstOrDefault();

            if (pedidoFromDb != null)
            {
                if (pedidoArteFromDb != null)
                {
                    if (pedidoArteFromDb.IdStatus != 0 && pedidoArteFromDb.IdStatus == (int)PedidoArteStatus.PEDIDO_RASCUNHO)
                    {
                        obj = pedidoFromDb;
                        Remove(obj);
                    }
                    else
                    {
                        obj = pedidoFromDb;
                        obj.Ativo = false;
                        Update(obj);
                    }
                }
                else
                {
                    if (pedidoVeiculoFromDb != null)
                    {
                        if (pedidoVeiculoFromDb.IdStatus != 0 && pedidoVeiculoFromDb.IdStatus == (int)PedidoVeiculoStatus.PEDIDO_RASCUNHO)
                        {
                            obj = pedidoFromDb;
                            Remove(obj);
                        }
                        else
                        {
                            obj = pedidoFromDb;
                            obj.Ativo = false;
                            Update(obj);
                        }
                    }
                }
            }
        }

        public async override ValueTask<Pedido> GetById(long id, CancellationToken cancellationToken)
        {
            var r = await SetAllInclude(_db.Pedido).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return r;
        }

        public override ValueTask<Pedido> GetByIdPedidoWithOutRoles(long idPedido, CancellationToken cancellationToken)
        {
            var pedidos = SetAllIncludeWithoutRules(_db.Pedido);

            var pedido = pedidos.FirstOrDefault(a => a.Id == idPedido);

            return new ValueTask<Pedido>(pedido);
        }

        public override IQueryable<Pedido> GetAll()
        {
            return SetAllInclude(_db.Pedido);
        }

        IQueryable<Pedido> SetAllIncludeWithoutRules(IQueryable<Pedido> pedidos)
        {
            pedidos = SetAllIncludeArte(pedidos);

            pedidos = SetAllIncludeVeiculo(pedidos);

            pedidos = SetAllIncludePedido(pedidos);

            return pedidos.OrderByDescending(o => o.Id);
        }

        IQueryable<Pedido> SetAllInclude(IQueryable<Pedido> pedidos)
        {
            var user = userProvider.User;

            pedidos = SetAllIncludePedido(pedidos);

            ///Perfil de Suprimentos e Gestor de Produção/Demandantes, enxergam os pedidos que não são rascunho
            if (userProvider.IsRole(Role.PERFIL_BASE_SUPRIMENTOS))
            {
                pedidos = SetAllIncludeArte(pedidos);

                pedidos =
                    pedidos
                    .Include(i =>
                    i.Itens.Where(a => string.IsNullOrWhiteSpace(a.RCs.FirstOrDefault().Acordo)));

                pedidos = pedidos.Where(a =>
                    a.IdTipo == (int)TipoPedido.ARTE &&
                    a.PedidoArte.IdStatus != (int)PedidoArteStatus.PEDIDO_RASCUNHO &&
                    a.Itens.Where(b => string.IsNullOrWhiteSpace(b.RCs.FirstOrDefault().Acordo)).Count() > 0
                );
            }
            //Perfis de Compram enxergam os pedidos cujo o login é comprador e apenas itens sem acordo. também não vê item que não foi atribuído a ele.
            else if (userProvider.IsRole(Role.PERFIL_COMPRADOR_ESTRUTURADA)
                || userProvider.IsRole(Role.PERFIL_COMPRADOR_EXTERNA))
            {
                pedidos = SetAllIncludeArte(pedidos);

                pedidos = pedidos.Include(i =>
                i.Itens.Where(a => string.IsNullOrEmpty(a.RCs.FirstOrDefault().Acordo)
                && a.PedidoItemArte.CompradoPorLogin.Equals(user.Login)));

                pedidos = pedidos.Where(a =>
                    a.IdTipo == (int)TipoPedido.ARTE &&
                    a.Itens.Any(x => x.PedidoItemArte.CompradoPorLogin == user.Login) &&
                    a.PedidoArte.IdStatus != (int)PedidoArteStatus.PEDIDO_RASCUNHO &&
                    a.PedidoArte.IdStatus != (int)PedidoArteStatus.PEDIDO_ENVIADO);
            }
            //Perfil Demandante enxerga os pedidos em que ele é o solicitante.
            else if (userProvider.IsRole(Role.PERFIL_DEMANDANTE))
            {
                pedidos = SetAllIncludeArte(pedidos);

                pedidos = SetAllIncludeVeiculo(pedidos);

                //pedidos = pedidos.Where(a =>
                //    a.Ativo == true && (a.CriadoPorLogin == user.Login || a.Equipe.Any(x => x.Login == user.Login))
                //);
            }
            else if (userProvider.IsRole(Role.PERFIL_COMPRADOR_VEICULOS))
            {
                pedidos = SetAllIncludeVeiculo(pedidos);

                pedidos = pedidos.Where(a =>
                    a.IdTipo == (int)TipoPedido.VEICULOCENA &&
                    //(a.PedidoVeiculo.CompradoPorLogin == user.Login || a.Equipe.Any(x => x.Login == user.Login)) ||
                    a.PedidoVeiculo.IdStatus != (int)PedidoVeiculoStatus.PEDIDO_RASCUNHO
                );
            }

            long[] idsSigilosos = userProvider.User.Conteudos.Where(x => x.Sigiloso).Select(x => x.Id).ToArray() ?? new long[] { };

            //Caso usuario possuir conteúdo sigiloso
            if (userProvider.User.Conteudos.Any(x => x.Sigiloso))
                pedidos = pedidos.Where(x => !x.Conteudo.Sigiloso || idsSigilosos.Any(i => i.Equals(x.IdConteudo)));
            else
                pedidos = pedidos.Where(x => !x.Conteudo.Sigiloso);

            //pedidos = pedidos.Where(x => x.Ativo == true);

            return pedidos.OrderByDescending(o => o.Id);
        }

        IQueryable<Pedido> SetAllIncludeArte(IQueryable<Pedido> dbPedido)
        {
            return dbPedido
                .Include(a => a.PedidoArte).ThenInclude(s => s.Status)
                .Include(a => a.PedidoArte).ThenInclude(u7 => u7.Base)
                .Include(i => i.Itens).ThenInclude(i => i.PedidoItemArte).ThenInclude(i => i.Status)
                .Include(i => i.Itens).ThenInclude(i => i.PedidoItemArte).ThenInclude(i => i.TrackingArte)
                .Include(i => i.Itens).ThenInclude(i => i.PedidoItemArte).ThenInclude(i => i.Comprador)
                .Include(i => i.Itens).ThenInclude(i => i.PedidoItemArte).ThenInclude(i => i.Devolucoes)
                .Include(i => i.Itens).ThenInclude(i => i.PedidoItemArte).ThenInclude(i => i.Compras)
                .Include(i => i.Itens).ThenInclude(i => i.PedidoItemArte).ThenInclude(i => i.Entregas)
                .Include(i => i.Itens).ThenInclude(i => i.PedidoItemArte).ThenInclude(i => i.Compras).ThenInclude(d => d.Documentos).ThenInclude(i => i.Arquivos);
        }

        IQueryable<Pedido> SetAllIncludeVeiculo(IQueryable<Pedido> dbPedido)
        {
            return dbPedido
               .Include(a => a.Arquivos)
               .Include(i => i.Conteudo)
               .Include(u1 => u1.CriadoPor)
               .Include(u6 => u6.AtualizadoPor)
               .Include(a => a.PedidoVeiculo).ThenInclude(s => s.Status)
               .Include(i => i.Itens).ThenInclude(i => i.Arquivos)
               .Include(i => i.Itens).ThenInclude(i => i.PedidoItemVeiculo).ThenInclude(x => x.Status)
               .Include(i => i.Itens).ThenInclude(i => i.PedidoItemVeiculo).ThenInclude(x => x.Trackings)
               .Include(i => i.Itens).ThenInclude(i => i.PedidoItemVeiculo).ThenInclude(x => x.Tipo)
               .Include(i => i.Itens).ThenInclude(i => i.PedidoItemVeiculo).ThenInclude(x => x.SubCategoria)
               .Include(i => i.Itens).ThenInclude(i => i.PedidoItemVeiculo).ThenInclude(t => t.Trackings).ThenInclude(s => s.Status);

        }

        IQueryable<Pedido> SetAllIncludePedido(IQueryable<Pedido> dbPedido)
        {
            return dbPedido
                .Include(a => a.Arquivos)
                .Include(i => i.Conteudo)
                .Include(u1 => u1.CriadoPor)
                .Include(u4 => u4.CanceladoPor)
                .Include(u6 => u6.AtualizadoPor)
                .Include(i => i.Itens).ThenInclude(i => i.RCs)
                .Include(e => e.Equipe).ThenInclude(i => i.Usuario)
                .Include(i => i.Itens).ThenInclude(i => i.CanceladoPor)
                .Include(i => i.Itens).ThenInclude(i => i.Arquivos)
                .Include(i => i.Itens).ThenInclude(i => i.PedidoItemConversas).ThenInclude(i => i.Arquivos);
        }

        /// <summary>
        /// Função que gera próximo número para um novo item de um pedido.
        /// </summary>
        /// <param name="pedidoId"></param>
        /// <returns></returns>
        public int GetProximoNumItem(long pedidoId)
        {
            try
            {
                var itens = _db.Pedido.Include(i => i.Itens).Where(x => x.Id == pedidoId).SelectMany(x => x.Itens).ToList();

                if (itens.Count() == 0) return 1;

                var max = itens.Select(x =>
                {
                    var p1 = (x.Numero ?? string.Empty).Split('.');

                    int.TryParse(p1[0], out int num);

                    return num;
                }).Max();

                return ++max;

            }
            catch { }

            return 0;
        }
    }
}
