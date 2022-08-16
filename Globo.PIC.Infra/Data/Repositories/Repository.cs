using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Infra.Data.Context;

namespace Globo.PIC.Infra.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        /// <summary>get
        /// 
        /// </summary>
        protected readonly PicDbContext _db;

        /// <summary>
        /// 
        ///</summary>
        protected readonly DbSet<TEntity> _dbSet;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public Repository(PicDbContext context)
        {
            _db = context;

            _dbSet = _db.Set<TEntity>();
        }

        public Repository(IDbContextFactory<PicDbContext> context)
        {
            _db = context.CreateDbContext();

            _dbSet = _db.Set<TEntity>();
        }

        public Repository(PicDbContext context, IDbContextFactory<PicDbContext> facContext)
        {
            _db = context;

            if(_db.Database.GetDbConnection().State == System.Data.ConnectionState.Closed)
                _db = facContext.CreateDbContext();

            _dbSet = _db.Set<TEntity>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="cancellationToken"></param>
        public virtual void Add(TEntity obj, CancellationToken cancellationToken)
        {
            _dbSet.AddAsync(obj, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objs"></param>
        /// <param name="cancellationToken"></param>
        public virtual void Add(IEnumerable<TEntity> objs, CancellationToken cancellationToken)
        {
            _dbSet.AddRangeAsync(objs, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual ValueTask<TEntity> GetById(long id, CancellationToken cancellationToken)
        {
            return _dbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        public virtual ValueTask<TEntity> GetByIdPedido(long idPedido, CancellationToken cancellationToken)
        {
            return _dbSet.FindAsync(new object[] { idPedido }, cancellationToken);
        }

        public virtual ValueTask<TEntity> GetByIdPedidoWithOutRoles(long idPedido, CancellationToken cancellationToken)
        {
            return _dbSet.FindAsync(new object[] { idPedido }, cancellationToken);
        }

        public virtual async Task<List<TEntity>> GetAllWithRules(CancellationToken cancellationToken)
        {
            return await Task.FromResult(_dbSet.ToList());
        }

        public virtual ValueTask<TEntity> GetByIdPedidoItemConversa(long idPedidoItemConversa, CancellationToken cancellationToken)
        {
            return _dbSet.FindAsync(new object[] { idPedidoItemConversa }, cancellationToken);
        }

        public virtual ValueTask<TEntity> GetByIdPedidoItemAnexo(long idPedidoItemAnexo, CancellationToken cancellationToken)
        {
            return _dbSet.FindAsync(new object[] { idPedidoItemAnexo }, cancellationToken);
        }

        public virtual ValueTask<TEntity> GetByIdPedidoItemConversaAnexo(long idPedidoItemConversaAnexo, CancellationToken cancellationToken)
        {
            return _dbSet.FindAsync(new object[] { idPedidoItemConversaAnexo }, cancellationToken);
        }

        public virtual ValueTask<TEntity> GetByIdPedidoAnexos(long idPedidoAnexo, CancellationToken cancellationToken)
        {
            return _dbSet.FindAsync(new object[] { idPedidoAnexo }, cancellationToken);
        }

        public virtual ValueTask<TEntity> GetByStatusVeiculoId(long idStatus, CancellationToken cancellationToken)
        {
            return _dbSet.FindAsync(new object[] { idStatus }, cancellationToken);
        }

        public virtual ValueTask<TEntity> GetByStatusId(long idStatus, CancellationToken cancellationToken)
        {
            return _dbSet.FindAsync(new object[] { idStatus }, cancellationToken);
        }
        public virtual ValueTask<TEntity> GetByStatusItemId(long idStatus, CancellationToken cancellationToken)
        {
            return _dbSet.FindAsync(new object[] { idStatus }, cancellationToken);
        }

        public virtual ValueTask<TEntity> GetByIdTipoVeiculos(long idTipo, CancellationToken cancellationToken)
        {
            return _dbSet.FindAsync(new object[] { idTipo }, cancellationToken);
        }

        public virtual ValueTask<TEntity> GetByIdPedidoItemVeiculo(long idItem, CancellationToken cancellationToken)
        {
            return _dbSet.FindAsync(new object[] { idItem }, cancellationToken);
        }        

        public virtual async Task<List<TEntity>> ListSubCategoriaIdByIdTipoVeiculo(long idTipoVeiculo, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_dbSet.ToList());
        }

        public virtual async Task<List<TEntity>> ListCategoria(CancellationToken cancellationToken)
        {
            return await Task.FromResult(_dbSet.ToList());
        }

        public virtual async Task<List<TEntity>> ListByIdPedidoItemConversa(long idPedidoItemConversa, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_dbSet.ToList());
        }

        public virtual async Task<List<TEntity>> ListByIdPedidoItemAnexo(long idPedidoItemAnexo, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_dbSet.ToList());
        }

        public virtual async Task<List<TEntity>> ListByIdPedidoAnexos(long idPedidoAnexo, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_dbSet.ToList());
        }

        public virtual async Task<List<TEntity>> ListByIdPedidoItemConversaAnexo(long idPedidoItemConversaAnexo, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_dbSet.ToList());
        }

        public virtual async Task<List<TEntity>> ListTrackingByIdItemPedido(long idPedidoItem, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_dbSet.ToList());
        }

        public virtual async Task<List<TEntity>> ListTrackingVeiculos(long idPedidoItem, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_dbSet.ToList());
        }

        public virtual ValueTask<TEntity> GetByIdPedidoItemCompraExistEntrega(long idPedidoItemCompra, CancellationToken cancellationToken)
        {
            return _dbSet.FindAsync(new object[] { idPedidoItemCompra }, cancellationToken);
        }
        public virtual ValueTask<TEntity> GetByIdPedidoItemExistDevolucao(long idPedidoItem, CancellationToken cancellationToken)
        {
            return _dbSet.FindAsync(new object[] { idPedidoItem }, cancellationToken);
        }

        public virtual ValueTask<TEntity> GetByIdPedidoItemCompra(long idPedidoItemCompra, CancellationToken cancellationToken)
        {
            return _dbSet.FindAsync(new object[] { idPedidoItemCompra }, cancellationToken);
        }
        public virtual async Task<List<TEntity>> ListByIdPedidoItemCompra(long idPedidoItemCompra, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_dbSet.ToList());
        }
        public virtual ValueTask<TEntity> GetByIdPedidoItemCompraDocumentosAnexos(long idPedidoItemCompraDocumentosAnexos, CancellationToken cancellationToken)
        {
            return _dbSet.FindAsync(new object[] { idPedidoItemCompraDocumentosAnexos }, cancellationToken);
        }
        public virtual async Task<List<TEntity>> ListByIdPedidoItemCompraDocumentosAnexos(long idPedidoItemCompraDocumentosAnexos, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_dbSet.ToList());
        }
        public virtual ValueTask<TEntity> GetByIdPedidoItemCompraDocumentos(long idPedidoItemDocumento, CancellationToken cancellationToken)
        {
            return _dbSet.FindAsync(new object[] { idPedidoItemDocumento }, cancellationToken);
        }
        public virtual async Task<List<TEntity>> ListByIdPedidoItemCompraDocumentos(long idPedidoItemDocumento, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_dbSet.ToList());
        }
        public virtual ValueTask<TEntity> GetByIdPedidoItemEntrega(long idPedidoItemEntrega, CancellationToken cancellationToken)
        {
            return _dbSet.FindAsync(new object[] { idPedidoItemEntrega }, cancellationToken);
        }
        public virtual async Task<List<TEntity>> ListByIdPedidoItemEntrega(long idPedidoItemEntrega, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_dbSet.ToList());
        }

        public virtual ValueTask<TEntity> GetByIdPedidoItemDevolucao(long idPedidoItemDevolucao, CancellationToken cancellationToken)
        {
            return _dbSet.FindAsync(new object[] { idPedidoItemDevolucao }, cancellationToken);
        }
        public virtual async Task<List<TEntity>> ListByIdPedidoItemDevolucao(long idPedidoItemDevolucao, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_dbSet.ToList());
        }
        public virtual async Task<List<TEntity>> ListByIdPedidoItemOriginalDevolucao(long idPedidoItemOriginalDevolucao, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_dbSet.ToList());
        }
        public virtual async Task<List<TEntity>> ListByIdPedidoItemAtribuicao(long idPedidoItemAtribuicao, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_dbSet.ToList());
        }
        public virtual ValueTask<TEntity> GetByIdPedidoItemAtribuicao(long idPedidoItemAtribuicao, CancellationToken cancellationToken)
        {
            return _dbSet.FindAsync(new object[] { idPedidoItemAtribuicao }, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual ValueTask<TEntity> GetById(IEnumerable<long> ids, CancellationToken cancellationToken)
        {
            return _dbSet.FindAsync(ids.Cast<object>().ToArray(), cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<TEntity> GetAll()
        {
            return _dbSet;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<TEntity> GetAllNotLazy()
        {
            return _dbSet;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public virtual void Update(TEntity obj)
        {
            _dbSet.Update(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public virtual void Remove(TEntity obj)
        {
            _dbSet.Remove(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objs"></param>
        public virtual void Remove(IEnumerable<TEntity> objs)
        {
            _dbSet.RemoveRange(objs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="cancellationToken"></param>
        public virtual void Remove(TEntity obj, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="cancellationToken"></param>
        public virtual void DeletarPedido(long Id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="cancellationToken"></param>
        public virtual void DeletarPedidoItemVeiculo(long Id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="cancellationToken"></param>
        public virtual void DeletarPedidoItem(long Id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="cancellationToken"></param>
        public virtual void DeletarAcionamento(TEntity obj, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="cancellationToken"></param>
        public virtual void DeletarAcionamentoItem(TEntity obj, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<List<TEntity>> ToListAsync(IQueryable<TEntity> query, CancellationToken cancellationToken)
        {
            return query.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Realiza contagem de registros de uma busca
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<int> CountAsync(IQueryable<TEntity> query, CancellationToken cancellationToken)
        {
            return query.CountAsync(cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<TEntity> GetByLogin(string login, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        /// <summary>
		/// 
		/// </summary>
		/// <param name="login"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public virtual Task<TEntity> GetByRoleName(string login, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public virtual ValueTask<List<TEntity>> ListByLogin(string login, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Executa query SQL diretamento na base de dados.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<List<TEntity>> ExecuteQuery(string query, CancellationToken cancellationToken)
        {
            return Task.FromResult(_db.ExecSQL<TEntity>(query));
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="cancellationToken"></param>
        public virtual void AddOrUpdate(TEntity obj, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

    }
}
