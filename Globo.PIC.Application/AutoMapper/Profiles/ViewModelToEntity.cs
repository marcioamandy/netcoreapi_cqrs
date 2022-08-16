using AutoMapper;
using Globo.PIC.Domain.ViewModels;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Application.Profiles
{
    /// <summary>
    /// 
    /// </summary>
    public class ViewModelToEntity : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public ViewModelToEntity()
        {
            CreateMap<TrackingArteViewModel, PedidoItemArteTracking>();
            CreateMap<TrackingVeiculoViewModel, PedidoItemVeiculoTracking>();
            CreateMap<ConteudoViewModel, Conteudo>();
            CreateMap<NotificationViewModel, Notificacao>();
            CreateMap<PedidoItemViewModel, PedidoItem>();
            //CreateMap<PedidoItemArteViewModel, PedidoItemArte>();
            //CreateMap<PedidoItemVeiculoViewModel, PedidoItemVeiculo>();
            CreateMap<ArquivoViewModel, PedidoItemAnexo>();
            CreateMap<PedidoItemConversaViewModel, PedidoItemConversa>();
            CreateMap<ArquivoViewModel, PedidoItemConversaAnexo>();
            CreateMap<PedidoItemArteCompraViewModel, PedidoItemArteCompra>();
            CreateMap<PedidoItemArteCompraDocumentosViewModel, PedidoItemArteCompraDocumento>();
            CreateMap<ArquivoViewModel, PedidoItemArteCompraDocumentoAnexo>();
            CreateMap<PedidoItemArteEntregaViewModel, PedidoItemArteEntrega>();
            CreateMap<PedidoItemArteDevolucaoViewModel, PedidoItemArteDevolucao>();
            CreateMap<PedidoItemArteAtribuicaoViewModel, PedidoItemArteAtribuicao>();
            CreateMap<PedidoViewModel, Pedido>();
            CreateMap<RCViewModel, RC>();

            //CreateMap<PedidoVeiculoViewModel, PedidoVeiculo>();
            //CreateMap<PedidoVeiculoViewModel, Pedido>();

            CreateMap<StatusViewModel, StatusPedidoArte>();
            CreateMap<StatusViewModel, StatusPedidoVeiculo>();
            CreateMap<StatusViewModel, StatusPedidoItemArte>();
            CreateMap<StatusViewModel, StatusPedidoItemVeiculo>();
            //CreateMap<UsuarioViewModel, Usuario>();
            CreateMap<UserRoleViewModel, UserRole>();
            CreateMap<PedidoEquipeViewModel,Equipe>();
            CreateMap<ArquivoViewModel, PedidoAnexo>();
            CreateMap<UnidadeNegocioViewModel, UnidadeNegocio>();
            CreateMap<DepartamentoViewModel, Departamento>();
            CreateMap<UsuarioConteudoViewModel, UsuarioConteudo>();

        }
    }
}
