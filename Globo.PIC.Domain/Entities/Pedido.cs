using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.Entities
{
    public class Pedido
    {

        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Description("Id")]
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
        [Description("Id do Conteudo ou conteúdo")]
        public long IdConteudo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id do Projeto")]
        public long IdProjeto { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id da Tarefa (Task DL)")]
        public long IdTarefa { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Descrição da Tarefa (Task DL)")]
        public string DescricaoTarefa{ get; set; }

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
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data de criação do pedido")]
        public DateTime? DataCriacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Login de criação do pedido")]
        public string CriadoPorLogin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Login de atualização do pedido")]
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
        [Description("Login Cancelamento")]
        public string CanceladoPorLogin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Justificativa Cancelamento do Pedido")]
        [MaxLength(500)]
        public string JustificativaCancelamento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Tipo de Pedido")]
        public long IdTipo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Tag do Pedido")]
        public long IdTag { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("DestinationOrganizationId")]
        public long DestinationOrganizationId{ get; set; }

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

        /// <summary>
        /// 
        /// </summary>  
        public Pedido()
        {
        }
        
        #region Relationship one to many properties

        public virtual IEnumerable<Equipe> Equipe { get; set; }

        public virtual IEnumerable<PedidoItem> Itens { get; set; }

        public virtual IEnumerable<PedidoAnexo> Arquivos { get; set; }

        #endregion

        #region Relationship one to one properties

        public virtual PedidoArte PedidoArte { get; set; }

        public virtual PedidoVeiculo PedidoVeiculo { get; set; }

        #endregion

        #region Relationship many to one properties

        public virtual Usuario CanceladoPor { get; set; }

        public virtual Usuario CriadoPor { get; set; }

        public virtual Usuario AtualizadoPor { get; set; }

        public virtual Usuario DevolvidoPor { get; set; }

        public virtual Conteudo Conteudo { get; set; }         

        #endregion

    }
}
