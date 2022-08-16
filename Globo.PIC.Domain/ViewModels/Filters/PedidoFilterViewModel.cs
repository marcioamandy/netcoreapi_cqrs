using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Globo.PIC.Domain.ViewModels.Filters
{
	public class PedidoFilterViewModel : BaseFilterViewModel
	{
		/// <summary>
		/// Filtro de Data 'Dê', range de dadas início/fim
		/// </summary>
		[Description("Filtro de Data 'Dê', range de dadas início/fim")]
		public DateTime? DataInicio { get; set; }

		/// <summary>
		/// Filtro de Data 'Até', range de dadas início/fim
		/// </summary>
		[Description("Filtro de Data 'Até', range de dadas início/fim")]
		public DateTime? DataFim { get; set; }

		/// <summary>
		/// Lista de Conteudos
		/// </summary>
		[Description("Lista de Conteudos")]
		public List<long> Conteudos { get; set; }

		/// <summary>
		/// Lista de Conteudos
		/// </summary>
		[Description("Lista de Projetos")]
		public List<long> Projetos { get; set; }

		/// <summary>
		/// Status do Pedido
		/// </summary>
		[Description("Status do Pedido")]
		public long? IdStatus { get; set; }

		/// <summary>
		/// Texto referente ao Titulo do pedido
		/// </summary>
		[Description("Texto referente ao Titulo do pedido")]
		public string Titulo { get; set; }

		/// <summary>
		/// User através do login
		/// </summary>
		[Description("usuário solicitante do pedido")]
		public string LoginSolicitadoPor { get; set; }

		/// <summary>
		/// User através do login
		/// </summary>
		[Description("usuário comprador de um item de pedido")]
		public string LoginComprador { get; set; }

		/// <summary>
		/// User através do login
		/// </summary>
		[Description("Flag se o pedido é de alimento")]
		public bool FlagPedidoAlimentos { get; set; }

		/// <summary>
		/// User através do login
		/// </summary>
		[Description("Flag se pedido é Fast Pass")]
		public bool FlagFastPass { get; set; }

		/// <summary>
		/// User através do login
		/// </summary>
		[Description("Tag de Pedidos")]
		public long? idTag { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public PedidoFilterViewModel()
		{

		}
	}
}
