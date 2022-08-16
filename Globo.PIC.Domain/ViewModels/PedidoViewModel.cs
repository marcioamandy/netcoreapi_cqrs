using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Globo.PIC.Domain.Enums;
using Globo.PIC.Domain.Extensions;

namespace Globo.PIC.Domain.ViewModels
{
    public class PedidoViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("Id do Pedido")]
        public long Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data Pedido")]
        public DateTime? DataPedido { get; set; }     

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(0)]
        [Description("Número de itens do pedido")]
        public long NroItens { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(0.00)]
        [Description("Valor total dos itens")]
        public decimal ValorItens { get; set; }        

        /// <summary>
		/// 
		/// </summary>
		[Description("Titulo")]
        public string Titulo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id do Conteudo - tabela de conteúdo")]
        public long IdConteudo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id do projeto - API DL")]
        public long IdProjeto { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id da Tarefa - API DL")]
        public long IdTarefa { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Descrição da Tarefa (Task DL)")]
        public string DescricaoTarefa { get; set; }

        /// <summary>
		/// 
		/// </summary>
		[Description("Local de Entrega")]
        public string LocalEntrega { get; set; }

        /// <summary>
		/// 
		/// </summary>
		[Description("Observação do Pedido")]
        public string Observacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Tipo de Pedido")]
        public string Tipo { get { return ((TipoPedido)IdTipo).GetEnumDescription(); } }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id do Tipo de Pedido")]
        public long IdTipo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Tag do Pedido")]
        public string Tag { get { return ((TagPedido)IdTag).GetEnumDescription(); } }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id da Tag do Pedido de Compra")]
        public long IdTag { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data de criação do pedido")]
        public DateTime? DataCriacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Login do Usuario Criador")]
        public string CriadoPorLogin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Login do Usuario Alterador")]
        public string AtualizadoPorLogin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Pedido ativo")]
        public bool Ativo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Finalidade do pedido")]
        public string Finalidade { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Centro de Custo do pedido")]
        public string CentroCusto { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Conta do Centro de Custo do pedido")]
        public string Conta { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data da devolução do pedido")]
        public DateTime? DataDevolucao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Login da Devolução")]
        public string DevolvidoPorLogin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Justificativa Devolução do Pedido")]
        [MaxLength(500)]
        public string JustificativaDevolucao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data de cancelamento do pedido")]
        public DateTime? DataCancelamento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Login que Cancelou")]
        public string CanceladoPorLogin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Justificativa Cancelamento do Pedido")]
        public string JustificativaCancelamento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Lista PedidoEquipeViewModel")]
        public List<PedidoEquipeViewModel> Equipe { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Lista PedidoAnexosViewModel")]
        public List<ArquivoViewModel> Arquivos { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Conteudo do Pedido")]
        public ConteudoViewModel Conteudo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Projeto do Pedido")]
        public ProjetoViewModel Projeto { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Usuario Criador")]
        public UsuarioViewModel CriadoPor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Usuario que Cancelou")]
        public UsuarioViewModel CanceladoPor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Usuario Alterador")]
        public UsuarioViewModel AtualizadoPor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Usuario da Base")]
        public UsuarioViewModel DevolvidoPor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("DestinationOrganizationId")]
        public long DestinationOrganizationId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("DestinationOrganizationCode")]
        public string DestinationOrganizationCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("DeliverToLocationId")]
        public long DeliverToLocationId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("DeliverToLocationCode")]
        public string DeliverToLocationCode { get; set; }

    }
}
