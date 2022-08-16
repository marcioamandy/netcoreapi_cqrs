using Globo.PIC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Globo.PIC.Domain.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="cancellationToken"></param>
        void Add(TEntity obj, CancellationToken cancellationToken);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="objs"></param>
        /// <param name="cancellationToken"></param>
        void Add(IEnumerable<TEntity> objs, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //ValueTask<TEntity> GetById(Guid id, CancellationToken cancellationToken);

        ValueTask<TEntity> GetByIdPedido(long idPedido, CancellationToken cancellationToken);
        ValueTask<TEntity> GetByIdPedidoWithOutRoles(long idPedido, CancellationToken cancellationToken);

        Task<List<TEntity>> GetAllWithRules(CancellationToken cancellationToken);
        ValueTask<TEntity> GetByIdPedidoItemAnexo(long idPedidoItemAnexo, CancellationToken cancellationToken);
        ValueTask<TEntity> GetByIdPedidoItemConversa(long idPedidoItemConversa, CancellationToken cancellationToken);
        ValueTask<TEntity> GetByIdPedidoItemConversaAnexo(long idPedidoItemConversaAnexo, CancellationToken cancellationToken);
        ValueTask<TEntity> GetByIdPedidoAnexos(long idPedidoAnexo, CancellationToken cancellationToken);
        ValueTask<TEntity> GetByIdPedidoItemCompra(long idPedidoItemCompra, CancellationToken cancellationToken);
        ValueTask<TEntity> GetByIdPedidoItemCompraDocumentosAnexos(long idPedidoItemCompraAnexo, CancellationToken cancellationToken);
        ValueTask<TEntity> GetByIdPedidoItemCompraDocumentos(long idPedidoItemDocumentos, CancellationToken cancellationToken);
        ValueTask<TEntity> GetByIdPedidoItemEntrega(long idPedidoItemEntrega, CancellationToken cancellationToken);
        ValueTask<TEntity> GetByIdTipoVeiculos(long idTipo, CancellationToken cancellationToken);
        ValueTask<TEntity> GetByIdPedidoItemVeiculo(long idItem, CancellationToken cancellationToken);

        Task<List<TEntity>> ListTrackingByIdItemPedido(long idPedidoItem, CancellationToken cancellationToken);
        Task<List<TEntity>> ListTrackingVeiculos(long idPedidoItem, CancellationToken cancellationToken);
        Task<List<TEntity>> ListByIdPedidoItemAnexo(long idPedidoItemAnexo, CancellationToken cancellationToken);
        Task<List<TEntity>> ListByIdPedidoItemConversa(long idPedidoItemConversa, CancellationToken cancellationToken);
        Task<List<TEntity>> ListByIdPedidoItemConversaAnexo(long idPedidoItemConversaAnexo, CancellationToken cancellationToken);
        Task<List<TEntity>> ListByIdPedidoAnexos(long idPedidoAnexo, CancellationToken cancellationToken);
        Task<List<TEntity>> ListByIdPedidoItemCompra(long idPedidoItemCompra, CancellationToken cancellationToken);
        Task<List<TEntity>> ListByIdPedidoItemCompraDocumentosAnexos(long idPedidoItemCompraAnexo, CancellationToken cancellationToken);
        Task<List<TEntity>> ListByIdPedidoItemCompraDocumentos(long idPedidoItemDocumentos, CancellationToken cancellationToken);
        Task<List<TEntity>> ListByIdPedidoItemEntrega(long idPedidoItemEntrega, CancellationToken cancellationToken);
        Task<List<TEntity>> ListSubCategoriaIdByIdTipoVeiculo(long idTipoVeiculo, CancellationToken cancellationToken); 

        //ValueTask<List<TEntity>> GetByIdPedido(long idPedido, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ValueTask<TEntity> GetById(long id, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        ValueTask<TEntity> GetById(IEnumerable<long> ids, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetAll();
        
        /// <summary>f
        /// 
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetAllNotLazy();

		/// <summary>
		/// 
		/// </summary>add
		/// <param name="obj"></param>
		void Update(TEntity obj);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        void Remove(TEntity obj);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objs"></param>
        void Remove(IEnumerable<TEntity> objs);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        void Remove(TEntity obj, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        void DeletarPedido(long Id, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        void DeletarPedidoItemVeiculo(long Id, CancellationToken cancellationToken);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        void DeletarPedidoItem(long Id, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        void DeletarAcionamento(TEntity obj, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        void DeletarAcionamentoItem(TEntity obj, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        void AddOrUpdate(TEntity obj, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<TEntity>> ToListAsync(IQueryable<TEntity> query, CancellationToken cancellationToken);

		/// <summary>
		/// Realiza contagem de registros de uma busca
		/// </summary>
		/// <param name=""></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<int> CountAsync(IQueryable<TEntity> query, CancellationToken cancellationToken);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="login"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<TEntity> GetByLogin(string login, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <param name="roleName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TEntity> GetByRoleName(string login, string roleName, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask<List<TEntity>> ListByLogin(string login, CancellationToken cancellationToken);

        /// <summary>
        /// Executa query SQL diretamento na base de dados.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<TEntity>> ExecuteQuery(string query, CancellationToken cancellationToken);
    }
}
