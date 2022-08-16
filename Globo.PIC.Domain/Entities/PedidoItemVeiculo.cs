using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.Entities
{
    public class PedidoItemVeiculo
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Range(0, long.MaxValue)]
        [Description("Id")]
        public long Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id do Pedido item")]
        [Range(0, long.MaxValue)]
        public long IdPedidoItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Personagem Utilização")]
        [MaxLength(200)]
        public string PersonagemUtilizacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Modelo")]
        [MaxLength(200)]
        public string Modelo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id do Tipo de Veículos")]
        [Range(0, long.MaxValue)]
        public long IdTipo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id da SubCategoria de Vewículos")]
        [Range(0, long.MaxValue)]
        public long IdSubCategoria { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Número de Opções")]
        [Range(0, long.MaxValue)]
        public long NroOpcoes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Ano")]
        [Range(0, long.MaxValue)]
        public long Ano { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Cor")]
        [MaxLength(30)]
        public string Cor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data Chegada Veículo")]
        public DateTime? DataChegadaVeiculo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data Devolucao Veículo")]
        public DateTime? DataDevolucaoVeiculo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Continuidade")]
        public bool Continuidade { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Cena de Ação")]
        public bool CenaAcao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Sobre a cena de ação")]
        [MaxLength(500)]
        public string SobreCenaAcao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id do Status")]
        [Range(1, long.MaxValue)]
        public long IdStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id da Origem")]
        [Range(1, long.MaxValue)]
        public long IdOrigem { get; set; }

        /// <summary>
		/// 
		/// </summary>
		[Description("Observação do Pedido Veiculos Item")]
        [MaxLength(500)]
        public string Observacao { get; set; }

        /// <summary>
		/// 
		/// </summary>
		[Description("Local de gravação do veículo")]
        [MaxLength(200)]
        public string LocalGravacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Nro de passageiros")]
        [Range(0, long.MaxValue)]
        public long Passageiros { get; set; }

        /// <summary>
		/// 
		/// </summary>
		[Description("Horas de voo")]
        [MaxLength(50)]
        public string HorasVoo { get; set; }

        /// <summary>
		/// 
		/// </summary>
		[Description("Horas com o veículo parado")]
        [MaxLength(50)]
        public string HorasParado { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Tag (Continuidade / Facilidade)")]
        [Range(1, long.MaxValue)]
        public long Tag { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Necessidades")]
        [MaxLength(500)]
        public string Necessidades { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(0.00)]
        [Description("Valor Maximo")]
        public decimal ValorMaximo { get; set; }

        /// <summary>
		/// 
		/// </summary>
		[Description("Local de Faturamento")]
        [MaxLength(100)]
        public string LocalFaturamento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data Devolver")]
        public DateTime? DataDevolver { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Justificativa Devolver")]
        [MaxLength(500)]
        public string JustificativaDevolver { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PedidoItemVeiculo()
        {
        }

        #region Relationship one to many properties

        public virtual IEnumerable<PedidoItemVeiculoTracking> Trackings { get; set; }

        public virtual IEnumerable<ItemVeiculo> ItensVeiculo { get; set; }

        #endregion

        #region Relationship many to one properties

        public virtual StatusPedidoItemVeiculo Status { get; set; }

        public virtual Categoria Tipo { get; set; }

        public virtual Categoria SubCategoria { get; set; }

        public virtual PedidoItem PedidoItem { get; set; }

        #endregion
    }
}
