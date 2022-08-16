using AutoMapper;
using System;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Enums;
using Globo.PIC.Domain.Extensions;
using Globo.PIC.Domain.ViewModels;
using Globo.PIC.Application.ResolverHandlers;

namespace Globo.PIC.Application.Profiles
{
    /// <summary>
    /// 
    /// </summary>
    public class EntityToViewModel : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public EntityToViewModel()
        {

            CreateMap<PedidoItemAnexo, ArquivoViewModel>()
                .AfterMap<PresignUrlResolverHandler>();
            CreateMap<PedidoAnexo, ArquivoViewModel>()
                .AfterMap<PresignUrlResolverHandler>();
            CreateMap<PedidoItemConversaAnexo, ArquivoViewModel>()
                .AfterMap<PresignUrlResolverHandler>();
            CreateMap<PedidoItemArteCompraDocumentoAnexo, ArquivoViewModel>()
                .AfterMap<PresignUrlResolverHandler>();
            CreateMap<ItemAnexo, ArquivoViewModel>()
                .AfterMap<PresignUrlResolverHandler>();
            //CreateMap<AcionamentoItemAnexo, ArquivoViewModel>()
            //    .AfterMap<PresignUrlResolverHandler>();

            CreateMap<Notificacao, NotificationViewModel>();

            CreateMap<Pedido, PedidoViewModel>();

            CreateMap<PedidoItem, PedidoItemViewModel>();
            CreateMap<Equipe, PedidoEquipeViewModel>();
            CreateMap<PedidoItemConversa, PedidoItemConversaViewModel>();

            CreateMap<PedidoItemArteCompra, PedidoItemArteCompraViewModel>()
                .ForMember(d => d.IdPedidoItem, opt => opt.MapFrom((src) => src.PedidoItemArte.IdPedidoItem));
                //.ForMember(d => d.IdPedidoItemArte, opt => opt.MapFrom((src) => src.PedidoItemArte.Id));

            CreateMap<PedidoItemArteCompraDocumento, PedidoItemArteCompraDocumentosViewModel>();
            CreateMap<PedidoItemArteEntrega, PedidoItemArteEntregaViewModel>()
                .ForMember(d => d.IdPedidoItem, opt => opt.MapFrom((src) => src.PedidoItemArte.PedidoItem.Id));
            CreateMap<PedidoItemArteDevolucao, PedidoItemArteDevolucaoViewModel>();
            CreateMap<PedidoItemArteAtribuicao, PedidoItemArteAtribuicaoViewModel>();
            CreateMap<Conteudo, ConteudoViewModel>();
            CreateMap<StatusPedidoArte, StatusViewModel>();
            CreateMap<StatusPedidoVeiculo, StatusViewModel>();
            CreateMap<StatusPedidoItemArte, StatusViewModel>();
            CreateMap<StatusPedidoItemVeiculo, StatusViewModel>();
            CreateMap<PedidoItemArteTracking, TrackingArteViewModel>();
            CreateMap<PedidoItemVeiculoTracking, TrackingVeiculoViewModel>();

            CreateMap<Usuario, UsuarioViewModel>()
                .ForMember(d => d.Conteudos, opt => opt.MapFrom((src) => src.UsuariosConteudos))
                .ForMember(d => d.Apelido, opt => opt.MapFrom((src, o) =>
                {
                    if (string.IsNullOrEmpty(src.Apelido))
                        return src.Name + " " + src.LastName;
                    else
                        return src.Apelido;
                }))
                .ReverseMap();

            CreateMap<UsuarioConteudo, UsuarioConteudoViewModel>();
            CreateMap<UnidadeNegocio, UnidadeNegocioViewModel>();
            CreateMap<Departamento, DepartamentoViewModel>();
            CreateMap<RC, RCViewModel>();

            CreateMap<PedidoItem, PedidoItemVeiculoViewModel>()
               .IncludeMembers(s => s.PedidoItemVeiculo);
            //.IncludeMembers(s => s.PedidoItemVeiculo.ItensVeiculo);
            //Veículo de Pesquisa / Veículo de Catálogo / Veículo de Empréstimo
            /*
             if (pedido.IdConteudo != pedidoItens.PedidoItem.Pedido.IdConteudo)
                 pedidoItemVM.OrigemVeiculo = "Veículo de Empréstimo";
             else if (pedidoItemVM.IdItem == null)
                 pedidoItemVM.OrigemVeiculo = "Veículo de Pesquisa";
             else
                 pedidoItemVM.OrigemVeiculo = "Veículo de Catálogo";
            */
            /*
             .ForMember(d => d.OrigemVeiculo, opt => opt.MapFrom((src, o) =>
              {
                  if (src.IdItem == null)
                      return "Veículo de Pesquisa";
                  else
                      return "Veículo de Catálogo";
              }));
            */

            CreateMap<PedidoItemVeiculo, PedidoItemVeiculoViewModel>()
                .IncludeMembers(s => s.PedidoItem)
                //.IncludeMembers(s => s.ItensVeiculo)
                .ForMember(d => d.Id, opt => opt.MapFrom((src) => src.PedidoItem.Id))
                .ReverseMap()
                .ForMember(d => d.IdPedidoItem, opt => opt.MapFrom(src => src.Id));


            CreateMap<Pedido, PedidoVeiculoViewModel>()
                .AfterMap<ProjetoResolverHandler>()
                //.AfterMap<TarefaResolverHandler>()
                .IncludeMembers(s => s.PedidoVeiculo);

            CreateMap<PedidoVeiculo, PedidoVeiculoViewModel>()
                .IncludeMembers(s => s.Pedido)
                .ForMember(d => d.Id, opt => opt.MapFrom((src) => src.Pedido.Id))
                .ReverseMap()
                .ForMember(d => d.IdPedido, opt => opt.MapFrom(src => src.Id));

            CreateMap<Pedido, PedidoArteViewModel>()
                .AfterMap<ProjetoResolverHandler>()
                //.AfterMap<TarefaResolverHandler>()
                .IncludeMembers(s => s.PedidoArte);

            CreateMap<PedidoArte, PedidoArteViewModel>()
                .IncludeMembers(s => s.Pedido)
                .ForMember(d => d.Id, opt => opt.MapFrom((src) => src.Pedido.Id))
                .ReverseMap()
                .ForMember(d => d.IdPedido, opt => opt.MapFrom(src => src.Id));

            CreateMap<PedidoItemVeiculo, PedidoItemVeiculoViewModel>()
                .IncludeMembers(s => s.PedidoItem)
                //.IncludeMembers(s => s.ItensVeiculo)
                .ForMember(d => d.Id, opt => opt.MapFrom((src) => src.PedidoItem.Id))
                .ReverseMap()
                .ForMember(d => d.IdPedidoItem, opt => opt.MapFrom(src => src.Id));

            CreateMap<PedidoItem, PedidoItemArteViewModel>()
                .IncludeMembers(s => s.PedidoItemArte);

            CreateMap<PedidoItemArte, PedidoItemArteViewModel>()
                .IncludeMembers(s => s.PedidoItem)
                .ForMember(d => d.Id, opt => opt.MapFrom((src) => src.PedidoItem.Id))
            .ReverseMap()
                .ForMember(d => d.IdPedidoItem, opt => opt.MapFrom(src => src.Id));

            CreateMap<UserRole, UserRoleViewModel>()
                .ForMember(d => d.Description, opt => opt.MapFrom((src, o) =>
                {
                    if (Enum.TryParse(src.Name, out Role role))
                        return role.GetEnumDescription();
                    else
                        return string.Empty;
                }));
            CreateMap<UserRole, RoleViewModel>()
                .ForMember(d => d.Description, opt => opt.MapFrom((src, o) =>
                {
                    if (Enum.TryParse(src.Name, out Role role))
                        return role.GetEnumDescription();
                    else
                        return string.Empty;
                }))
                .ForMember(d => d.Type, opt => opt.MapFrom((src, o) =>
                {
                    return src.Name.ToString().StartsWith("PERFIL") ? RoleType.Profile.ToString() : RoleType.Grant.ToString();
                }));

            CreateMap<UnidadeNegocio, UnidadeNegocioViewModel>();
        }
    }
}
