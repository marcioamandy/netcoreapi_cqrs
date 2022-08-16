using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Infra.Data.Context;
using Globo.PIC.Infra.Data.Repositories;
using Globo.PIC.Infra.Data.ServiceProxies;
using Globo.PIC.Infra.Data.UnitOfWork;
using Globo.PIC.Infra.Email;
using Globo.PIC.Infra.Notifications;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Globo.PIC.Infra.AWS;
using Globo.PIC.Infra.Hangfire;
using Globo.PIC.Application.AutoMapper;
using Globo.PIC.Application.Arte.AutoMapper;
using Globo.PIC.Application.Veiculo.AutoMapper;

namespace Globo.PIC.Infra.IoC
{
	public class InjectorBootStrapper
	{

		/// <summary>
		/// 
		/// </summary>
		/// <param name="services"></param>
		public static void RegisterServices(IServiceCollection services)
		{

            //Configuração dos Perfis do AutoMapper
            services.AddAutoMapperApplication();
            services.AddAutoMapperApplicationArte();
            services.AddAutoMapperApplicationVeiculo();

            services.AddMediatR(AppDomain.CurrentDomain.Load("Globo.PIC.Application.Veiculo"));
            services.AddMediatR(AppDomain.CurrentDomain.Load("Globo.PIC.Application.Arte"));
            services.AddMediatR(AppDomain.CurrentDomain.Load("Globo.PIC.Application"));

            string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            services.AddDbContext<PicDbContext>(options => {
                options.EnableSensitiveDataLogging(true);
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            services.AddDbContextFactory<PicDbContext>(options => 
                options.UseMySql(ServerVersion.AutoDetect(connectionString)));

            //Infra - Repositories
            services.AddScoped<IRepository<Assign>, AssignRepository>();
            services.AddScoped<IRepository<Acionamento>, AcionamentoRepository>();
            services.AddScoped<IRepository<AcionamentoItem>, AcionamentoItemRepository>();
            services.AddScoped<IRepository<AcionamentoItemAnexo>, AcionamentoItemAnexoRepository>();
            services.AddScoped<IRepository<Notificacao>, NotificationRepository>();
            services.AddScoped<IRepository<PedidoItemConversa>, PedidoItemConversaRepository>();
            services.AddScoped<IRepository<PedidoAnexo>, PedidoAnexosRepository>();
            services.AddScoped<IRepository<PedidoItemAnexo>, PedidoItemAnexosRepository>();
            services.AddScoped<IRepository<PedidoArte>, PedidoArteRepository>();
            services.AddScoped<IRepository<PedidoItemArte>, PedidoItemArteRepository>();
            services.AddScoped<IRepository<PedidoItemConversaAnexo>, PedidoItemConversaAnexosRepository>();
            services.AddScoped<IRepository<PedidoItemArteCompraDocumentoAnexo>, PedidoItemCompraDocumentosAnexosRepository>();
            services.AddScoped<IRepository<PedidoItemArteCompraDocumento>, PedidoItemCompraDocumentosRepository>();
            services.AddScoped<IRepository<PedidoItemArteCompra>, PedidoItemCompraRepository>();
            services.AddScoped<IRepository<PedidoItemArteEntrega>, PedidoItemEntregaRepository>();
            services.AddScoped<IRepository<PedidoItemArteDevolucao>, PedidoItemDevolucaoRepository>();
            services.AddScoped<IRepository<PedidoItemArteAtribuicao>, PedidoItemAtribuicaoRepository>();
            services.AddScoped<IRepository<Equipe>, PedidoEquipeRepository>();
            services.AddScoped<IRepository<PedidoItem>, PedidoItemRepository>();
            services.AddScoped<IRepository<Pedido>, PedidoRepository>();
            services.AddScoped<IRepository<PedidoVeiculo>, PedidoVeiculoRepository>();
            services.AddScoped<IRepository<PedidoItemVeiculo>, PedidoItemVeiculoRepository>();
            services.AddScoped<IRepository<ItemVeiculo>, ItemVeiculoRepository>();
            services.AddScoped<IRepository<ItemCatalogo>, ItemCatalogoRepository>();
            services.AddScoped<IRepository<Item>, ItemRepository>();
            services.AddScoped<IRepository<Conteudo>, ConteudoRepository>();
            services.AddScoped<IRepository<Reader>, ReaderRepository>();
            services.AddScoped<IRepository<Categoria>, CategoriaRepository>();
            services.AddScoped<IRepository<StatusPedidoArte>, StatusArteRepository>();
            services.AddScoped<IRepository<StatusPedidoVeiculo>, StatusVeiculoRepository>();
            services.AddScoped<IRepository<StatusPedidoItemVeiculo>, StatusVeiculoItemRepository>();
            services.AddScoped<IRepository<StatusPedidoItemArte>, StatusArteItemRepository>();
            services.AddScoped<IRepository<PedidoItemArteTracking>, TrackingArteRepository>();
            services.AddScoped<IRepository<PedidoItemVeiculoTracking>, TrackingVeiculoRepository>();
            services.AddScoped<IRepository<Usuario>, UserRepository>();
            services.AddScoped<IRepository<Viewer>, ViewerRepository>();
            services.AddScoped<IRepository<UserRole>, RoleRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRepository<UnidadeNegocio>, UnidadeNegocioRepository>();
            services.AddScoped<IRepository<RC>, RCRepository>();
            services.AddScoped<IRepository<UsuarioConteudo>, UsuarioConteudoRepository>();

            // Infra - Proxies Serviceslo
            services.AddScoped<IShoppingCatalogProxy, ShoppingCatalogProxy>();
            services.AddScoped<IPurchaseRequisitionProxy, PurchaseRequisitionProxy>();
            services.AddScoped<ITasksProxy, TaskProxy>();
            services.AddScoped<IProjectProxy, ProjectProxy>();
            services.AddScoped<INodeServiceProxy, NodeServiceProxy>();
            services.AddScoped<IConteudoServiceProxy, ConteudoServiceProxy>();
            services.AddScoped<IOrganizationStructureProxy, OrganizationStructureServiceProxy>();
            services.AddScoped<ISupplierProxy, SupplierServiceProxy>();
            services.AddScoped<ILineProxy, LineProxy>();
            services.AddScoped<IHCMProxy, HCMProxy>();
            services.AddScoped<INotificationMethods, NotificationMethods>();
            services.AddScoped<IExpenditureProxy, ExpenditureServiceProxy>();
            services.AddScoped<IRepository<Departamento>, DepartamentoRepository>();
            // Infra - Configuração Cron
            services.AddSingleton<ICronEvents, CronEvents>();

            // Infra - Cliente Services
            services.AddScoped<IS3Client, S3Client>();
            services.AddScoped<IEmailSender, EmailSender>();

        }
	}
}
