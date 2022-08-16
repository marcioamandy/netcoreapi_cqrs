using AutoMapper;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Models;
using Globo.PIC.Domain.ViewModels;

namespace Globo.PIC.Application.AutoMapper
{

	/// <summary>
	/// 
	/// </summary>
	public class EntityToModel : Profile
	{

		/// <summary>
		/// 
		/// </summary>
		public EntityToModel()
		{
			CreateMap<PedidoItemConversa, PedidoItemConversaModel>();
			CreateMap<PedidoItemConversaAnexo, PedidoItemConversaAnexosModel>();
			CreateMap<PedidoItemArteCompra, PedidoItemCompraModel>();
			CreateMap<PedidoItemArteCompraDocumento, PedidoItemCompraDocumentosModel>();
			CreateMap<PedidoItemArteCompraDocumentoAnexo, PedidoItemCompraDocumentosAnexosModel>();


			CreateMap<OrganizationStructureLocation, OrganizationStructureLocationViewModel>();
			CreateMap<OrganizationBusinessUnit, OrganizationBusinessUnitViewModel>();
			CreateMap<LegalEntity, LegalEntityViewModel>();
			CreateMap<ManagementBusinessUnit, ManagementBusinessUnitViewModel>();
			CreateMap<OrganizationStructure, OrganizacaoViewModel>();
		
		}
	}
}
