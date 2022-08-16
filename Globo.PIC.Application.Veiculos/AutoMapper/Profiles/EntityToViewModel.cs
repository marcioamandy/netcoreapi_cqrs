using AutoMapper;
using System;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Enums;
using Globo.PIC.Domain.Extensions;
using Globo.PIC.Domain.ViewModels;

namespace Globo.PIC.Application.Veiculo.Profiles
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
            CreateMap<Acionamento, AcionamentoViewModel>();
            CreateMap<AcionamentoItem, AcionamentoItemViewModel>();
            CreateMap<AcionamentoItemAnexo, ArquivoViewModel>();
            CreateMap<PedidoVeiculo, PedidoVeiculoViewModel>();
            CreateMap<StatusPedidoItemVeiculo, StatusViewModel>();
            CreateMap<StatusPedidoVeiculo, StatusViewModel>();
            CreateMap<Categoria, CategoriaViewModel>();
            CreateMap<Categoria, SubCategoriaViewModel>();

            CreateMap<ItemAnexo, ArquivoViewModel>();

            CreateMap<Item, ItemViewModel>();

            CreateMap<ItemCatalogo, ItemCatalogoViewModel>();

            CreateMap<Item, ItemVeiculoViewModel>()
                .IncludeMembers(s => s.ItemVeiculo);

            CreateMap<ItemVeiculo, ItemVeiculoViewModel>()
               .IncludeMembers(s => s.Item)
            .ReverseMap();
        }
    }
}
