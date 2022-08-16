using AutoMapper;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Models;
using Globo.PIC.Domain.ViewModels;
using Globo.PIC.Domain.ViewModels.Statistics;

namespace Globo.PIC.Application.Profiles
{

    /// <summary>
    /// 
    /// </summary>
    public class ModelToViewModel : Profile
    {

        /// <summary>
        /// 
        /// </summary>
        public ModelToViewModel()
        {

            CreateMap<IdentityUser, IdentityUserViewModel>()
                .ForMember(d => d.Apelido, opt => opt.MapFrom((src, o) =>
                {
                    if (string.IsNullOrEmpty(src.Apelido))
                        return src.Name + " " + src.LastName;
                    else
                        return src.Apelido;
                }))
                .ReverseMap();
            CreateMap<StatisticDictionary, StatisticDictionaryViewModel>();
            CreateMap<Authorization, AuthorizationViewModel>();
            CreateMap<Tarefa, TarefaViewModel>();
            CreateMap<SortTarefa, SortNivelViewModel>();
            CreateMap<ProjetoModel, ProjetoViewModel>()
                .ForMember(d => d.Id, opt => opt.MapFrom((src) => src.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom((src) => src.Name))
                .ForMember(d => d.Status, opt => opt.MapFrom((src) => src.Status))
                .ForMember(d => d.UnidadeNegocio, opt => opt.MapFrom((src) => src.BusinessUnit))
                //.ForMember(d => d.SourceCode, opt => opt.MapFrom((src) => src.SourceCode))
                .ForMember(d => d.Description, opt => opt.MapFrom((src) => src.Description))
                .ForMember(d => d.Number, opt => opt.MapFrom((src) => src.Number));

            CreateMap<BusinessUnit, UnidadeNegocio>();
            CreateMap<UserRole, UserRoleViewModel>();

            CreateMap<PedidoItemConversaModel, PedidoItemConversaViewModel>();
            CreateMap<PedidoItemConversaAnexosModel, ArquivoViewModel>();
            CreateMap<PedidoItemCompraModel, PedidoItemArteCompraViewModel>();
            CreateMap<PedidoItemCompraDocumentosModel, PedidoItemArteCompraDocumentosViewModel>();
            CreateMap<PedidoItemCompraDocumentosAnexosModel, ArquivoViewModel>();
            CreateMap<Supplier, FornecedorViewModel>();
            CreateMap<Agreements, AgreementsViewModel>();
            CreateMap<AgreementItems, AgreementItemsViewModel>();
            CreateMap<RequisitionBusinessUnits, UnidadeNegocioViewModel>();

            CreateMap<ShoppingCatalogItem, ShoppingCatalogItemViewModel>()
                .ForMember(d => d.Valor, opt => opt.MapFrom((src) => src.combined_price))
                .ForMember(d => d.CatalogItemKey, opt => opt.MapFrom((src) => src.document_id))
                .ForMember(d => d.TipoDocumento, opt => opt.MapFrom((src) => src.document_type))

                .ForMember(d => d.Categoria, opt => opt.MapFrom((src) => src.category.category_name))
                .ForMember(d => d.CategoriaId, opt => opt.MapFrom((src) => src.category.category_id))

                .ForMember(d => d.UOM, opt => opt.MapFrom((src) => src.item.item_uom_info.uom))
                .ForMember(d => d.UOMCode, opt => opt.MapFrom((src) => src.item.item_uom_info.uom_code))

                .ForMember(d => d.Acordo, opt => opt.MapFrom((src) => src.agreement_line.agreement))
                .ForMember(d => d.AcordoId, opt => opt.MapFrom((src) => src.agreement_line.agreement_id))
                .ForMember(d => d.AcordoLinhaId, opt => opt.MapFrom((src) => src.agreement_line.agreement_line_id))
                .ForMember(d => d.AcordoLinhaStatus, opt => opt.MapFrom((src) => src.agreement_line.line_status))
                .ForMember(d => d.AcordoStatus, opt => opt.MapFrom((src) => src.agreement_line.document_status))
                .ForMember(d => d.Moeda, opt => opt.MapFrom((src) => src.agreement_line.currency_code))

                .ForMember(d => d.FornecedorId, opt => opt.MapFrom((src) =>
                    src.agreement_line.agreement_header.agreement_supplier.supplier_id))

                .ForMember(d => d.Fornecedor, opt => opt.MapFrom((src) =>
                    src.agreement_line.agreement_header.agreement_supplier.agreement_supplier_name.party_name))

                .ForMember(d => d.ItemId, opt => opt.MapFrom((src) => src.item.item_id))
                .ForMember(d => d.ImagemUrl, opt => opt.MapFrom((src) => src.item.image_url))
                .ForMember(d => d.ItemCodigo, opt => opt.MapFrom((src) => src.item.item_number))
                .ForMember(d => d.Descricao, opt => opt.MapFrom((src, x) =>
                {
                    if (src.item != null)
                        return src.item.description;

                    if (src.agreement_line != null)
                        return src.agreement_line.description;

                    return null;
                }))
                .ForMember(d => d.DescricaoCompleta, opt => opt.MapFrom((src, x) =>
                {
                    if (src.item != null)
                        return src.item.long_description;

                    if (src.agreement_line != null)
                        return src.agreement_line.long_description;

                    return null;
                }));
            CreateMap<Location, LocationViewModel>();
            CreateMap<items, ItemsViewModel>();
        }
    }
}
