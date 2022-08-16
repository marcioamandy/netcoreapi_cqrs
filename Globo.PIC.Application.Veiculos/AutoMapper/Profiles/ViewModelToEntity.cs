using AutoMapper;
using Globo.PIC.Domain.ViewModels;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Application.Veiculo.Profiles
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
            CreateMap<StatusViewModel, StatusPedidoVeiculo>();
            CreateMap<StatusViewModel, StatusPedidoItemVeiculo>();
            CreateMap<CategoriaViewModel, Categoria>();
            CreateMap<SubCategoriaViewModel, Categoria>();
            CreateMap<ItemVeiculoViewModel, Item>();
            CreateMap<ItemCatalogoViewModel, ItemCatalogo>();
            CreateMap<ArquivoViewModel, ItemAnexo>();
            CreateMap<AcionamentoViewModel, Acionamento>();
            CreateMap<AcionamentoItemViewModel, AcionamentoItem>();
            CreateMap<ArquivoViewModel, AcionamentoItemAnexo>();
        }
    }
}
