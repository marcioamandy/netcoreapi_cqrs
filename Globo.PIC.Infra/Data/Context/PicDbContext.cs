using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Infra.Data.Mappings;
using Globo.PIC.Domain.Interfaces;

namespace Globo.PIC.Infra.Data.Context
{
    /// <summary>
    /// 
    /// </summary>
    public class PicDbContext : DbContext, IPicDbContext
    {

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Acionamento> Acionamento { get; set; }

        /// <summary>
		/// 
		/// </summary>
		public DbSet<AcionamentoItem> AcionamentoItem { get; set; }

        /// <summary>
		/// 
		/// </summary>
		public DbSet<AcionamentoItemAnexo> AcionamentoItemAnexo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Assign> Assign { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Categoria> Categoria { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Conteudo> Conteudo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Departamento> Departamento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Equipe> Equipe { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Item> Item { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<ItemAnexo> ItemAnexo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<ItemCatalogo> ItemCatalogo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<ItemVeiculo> ItemVeiculo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Notificacao> Notification { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Pedido> Pedido { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<PedidoAnexo> PedidoAnexo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<PedidoArte> PedidoArte { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<PedidoItem> PedidoItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<PedidoItemAnexo> PedidoItemAnexo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<PedidoItemArte> PedidoItemArte { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<PedidoItemArteCompra> PedidoItemArteCompra { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<PedidoItemArteCompraDocumento> PedidoItemArteCompraDocumento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<PedidoItemArteCompraDocumentoAnexo> PedidoItemArteCompraDocumentoAnexo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<PedidoItemArteDevolucao> PedidoItemArteDevolucao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<PedidoItemArteAtribuicao> PedidoItemArteAtribuicao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<PedidoItemArteEntrega> PedidoItemArteEntrega { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<PedidoItemArteTracking> PedidoItemArteTracking { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<PedidoItemConversa> PedidoItemConversa { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<PedidoItemConversaAnexo> PedidoItemConversaAnexo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<PedidoItemVeiculo> PedidoItemVeiculo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<PedidoItemVeiculoTracking> PedidoItemVeiculoTracking { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<PedidoVeiculo> PedidoVeiculo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<RC> RC { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Reader> Reader { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<StatusPedidoArte> StatusPedidoArte { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<StatusPedidoItemArte> StatusPedidoItemArte { get; set; }        

        /// <summary>
        /// 
        /// </summary>
        public DbSet<StatusPedidoItemVeiculo> StatusPedidoItemVeiculo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<StatusPedidoVeiculo> StatusPedidoVeiculo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<UserRole> UserRole { get; set; }

        /// <summary>
		/// 
		/// </summary>
		public DbSet<Usuario> Usuario { get; set; }

        /// <summary>
		/// 
		/// </summary>
		public DbSet<Viewer> Viewer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<UnidadeNegocio> BusinessUnity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public PicDbContext(DbContextOptions<PicDbContext> options): base(options)
        {

        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.ApplyConfiguration(new UsuarioMap());
            builder.ApplyConfiguration(new PedidoItemArteTrackingMap());
            builder.ApplyConfiguration(new PedidoItemVeiculoTrackingMap());
            builder.ApplyConfiguration(new CategoriaMap());
            builder.ApplyConfiguration(new StatusPedidoVeiculoMap());
            builder.ApplyConfiguration(new StatusPedidoItemVeiculoMap());
            builder.ApplyConfiguration(new StatusPedidoItemArteMap());
            builder.ApplyConfiguration(new StatusPedidoArteMap());
            builder.ApplyConfiguration(new PedidoVeiculoMap());
            builder.ApplyConfiguration(new ItemMap());
            builder.ApplyConfiguration(new RCMap());
            builder.ApplyConfiguration(new ItemVeiculoMap());
            builder.ApplyConfiguration(new PedidoItemVeiculoMap());
            builder.ApplyConfiguration(new PedidoMap());
            builder.ApplyConfiguration(new PedidoItemMap());
            builder.ApplyConfiguration(new PedidoItemArteEntregaMap());
            builder.ApplyConfiguration(new PedidoItemArteDevolucaoMap());
            builder.ApplyConfiguration(new PedidoItemArteAtribuicaoMap());
            builder.ApplyConfiguration(new PedidoItemConversaMap());
            builder.ApplyConfiguration(new PedidoItemConversaAnexoMap());
            builder.ApplyConfiguration(new PedidoItemArteCompraMap());
            builder.ApplyConfiguration(new PedidoItemArteCompraDocumentoAnexoMap());
            builder.ApplyConfiguration(new PedidoItemArteCompraDocumentoMap());
            builder.ApplyConfiguration(new PedidoItemAnexoMap());
            builder.ApplyConfiguration(new PedidoEquipeMap());
            builder.ApplyConfiguration(new PedidoArteMap());
            builder.ApplyConfiguration(new PedidoItemArteMap());
            builder.ApplyConfiguration(new ItemAnexoMap());
            builder.ApplyConfiguration(new PedidoAnexoMap());
            builder.ApplyConfiguration(new NotificationMap());
            builder.ApplyConfiguration(new AcionamentoMap());
            builder.ApplyConfiguration(new ItemCatalogoMap());
            builder.ApplyConfiguration(new ConteudoMap());
            builder.ApplyConfiguration(new AssignMap());
            builder.ApplyConfiguration(new AcionamentoItemAnexoMap());
            builder.ApplyConfiguration(new AcionamentoItemMap());
            builder.ApplyConfiguration(new ReaderMap());
            builder.ApplyConfiguration(new ViewerMap());
            builder.ApplyConfiguration(new UserRoleMap());

            builder.ApplyConfiguration(new UnidadeNegocioMap());
            builder.ApplyConfiguration(new DepartamentoMap());
            builder.ApplyConfiguration(new UsuarioConteudosMap());
        }

        public List<T> ExecSQL<T>(string query)
        {
            using (var command = Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                Database.OpenConnection();

                List<T> list = new List<T>();
                using (var result = command.ExecuteReader())
                {
                    T obj = default(T);
                    while (result.Read())
                    {
                        obj = Activator.CreateInstance<T>();
                        foreach (PropertyInfo prop in obj.GetType().GetProperties())
                        {
                            if (!object.Equals(result[prop.Name], DBNull.Value))
                            {
                                prop.SetValue(obj, result[prop.Name], null);
                            }
                        }
                        list.Add(obj);
                    }
                }
                Database.CloseConnection();
                return list;
            }
        }

    }
}
